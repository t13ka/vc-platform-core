using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.CoreModule.Core.Conditions.Browse;
using VirtoCommerce.CoreModule.Core.Conditions.GeoConditions;
using VirtoCommerce.MarketingModule.Core;
using VirtoCommerce.MarketingModule.Core.Events;
using VirtoCommerce.MarketingModule.Core.Model;
using VirtoCommerce.MarketingModule.Core.Model.DynamicContent;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Conditions;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.MarketingModule.Core.Services;
using VirtoCommerce.MarketingModule.Data.ExportImport;
using VirtoCommerce.MarketingModule.Data.Handlers;
using VirtoCommerce.MarketingModule.Data.Promotions;
using VirtoCommerce.MarketingModule.Data.Repositories;
using VirtoCommerce.MarketingModule.Data.Search;
using VirtoCommerce.MarketingModule.Data.Services;
using VirtoCommerce.MarketingModule.Web.Authorization;
using VirtoCommerce.MarketingModule.Web.ExportImport;
using VirtoCommerce.MarketingModule.Web.JsonConverters;
using VirtoCommerce.Platform.Core.Bus;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.DynamicProperties;
using VirtoCommerce.Platform.Core.ExportImport;
using VirtoCommerce.Platform.Core.Modularity;
using VirtoCommerce.Platform.Core.Security;
using VirtoCommerce.Platform.Core.Settings;
using VirtoCommerce.Platform.Data.Extensions;

namespace VirtoCommerce.MarketingModule.Web
{
    public class Module : IModule, IExportSupport, IImportSupport
    {
        private IApplicationBuilder _appBuilder;
        public ManifestModuleInfo ModuleInfo { get; set; }

        public void Initialize(IServiceCollection serviceCollection)
        {
            var configuration = serviceCollection.BuildServiceProvider().GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("VirtoCommerce.Marketing") ?? configuration.GetConnectionString("VirtoCommerce");

            serviceCollection.AddTransient<IMarketingRepository, MarketingRepository>();
            serviceCollection.AddDbContext<MarketingDbContext>(options => options.UseSqlServer(connectionString));
            serviceCollection.AddTransient<Func<IMarketingRepository>>(provider => () => provider.CreateScope().ServiceProvider.GetRequiredService<IMarketingRepository>());

            var promotionExtensionManager = new DefaultMarketingExtensionManager();


            #region Services

            serviceCollection.AddSingleton<IMarketingExtensionManager>(promotionExtensionManager);
            serviceCollection.AddTransient<IPromotionService, PromotionService>();
            serviceCollection.AddTransient<ICouponService, CouponService>();
            serviceCollection.AddTransient<IPromotionUsageService, PromotionUsageService>();
            serviceCollection.AddTransient<IMarketingDynamicContentEvaluator, DefaultDynamicContentEvaluator>();
            serviceCollection.AddTransient<IDynamicContentService, DynamicContentService>();
            serviceCollection.AddTransient<ICouponService, CouponService>();

            #endregion

            #region Search

            serviceCollection.AddTransient<IContentItemsSearchService, ContentItemsSearchService>();
            serviceCollection.AddTransient<IContentPlacesSearchService, ContentPlacesSearchService>();
            serviceCollection.AddTransient<IContentPublicationsSearchService, ContentPublicationsSearchService>();
            serviceCollection.AddTransient<ICouponSearchService, CouponSearchService>();
            serviceCollection.AddTransient<IFolderSearchService, FolderSearchService>();
            serviceCollection.AddTransient<IPromotionSearchService, PromotionSearchService>();
            serviceCollection.AddTransient<IPromotionUsageSearchService, PromotionUsageSearchService>();

            #endregion

            serviceCollection.AddTransient<CsvCouponImporter>();

            serviceCollection.AddSingleton<IMarketingPromoEvaluator>(provider =>
            {
                var settingsManager = provider.GetService<ISettingsManager>();
                var promotionService = provider.GetService<IPromotionSearchService>();
                var promotionCombinePolicy = settingsManager.GetValue(ModuleConstants.Settings.General.CombinePolicy.Name, "BestReward");
                if (promotionCombinePolicy.EqualsInvariant("CombineStackable"))
                {
                    return new CombineStackablePromotionPolicy(promotionService);
                }
                return new BestRewardPromotionPolicy(promotionService);
            });

            AbstractTypeFactory<DynamicPromotion>.RegisterType<DynamicPromotion>().WithFactory(() =>
            {
                var couponSearchService = serviceCollection.BuildServiceProvider().GetService<ICouponSearchService>();
                var promotionUsagesSearchService = serviceCollection.BuildServiceProvider().GetService<IPromotionUsageSearchService>();
                return new DynamicPromotion(couponSearchService, promotionUsagesSearchService);
            });

            serviceCollection.AddTransient<DynamicContentItemEventHandlers>();
            serviceCollection.AddTransient<MarketingExportImport>();

            serviceCollection.AddTransient<IAuthorizationHandler, MarketingAuthorizationHandler>();
        }

        public void PostInitialize(IApplicationBuilder appBuilder)
        {
            _appBuilder = appBuilder;

            var settingsRegistrar = appBuilder.ApplicationServices.GetRequiredService<ISettingsRegistrar>();
            settingsRegistrar.RegisterSettings(ModuleConstants.Settings.General.AllSettings, ModuleInfo.Id);

            var permissionsRegistrar = appBuilder.ApplicationServices.GetRequiredService<IPermissionsRegistrar>();
            permissionsRegistrar.RegisterPermissions(ModuleConstants.Security.Permissions.AllPermissions.Select(x =>
                new Permission()
                {
                    GroupName = "Marketing",
                    ModuleId = ModuleInfo.Id,
                    Name = x
                }).ToArray());

            //Register Permission scopes
            AbstractTypeFactory<PermissionScope>.RegisterType<MarketingStoreSelectedScope>();
            permissionsRegistrar.WithAvailabeScopesForPermissions(new[] { ModuleConstants.Security.Permissions.Read }, new MarketingStoreSelectedScope());


            var eventHandlerRegistrar = appBuilder.ApplicationServices.GetService<IHandlerRegistrar>();
            //Create order observer. record order coupon usage
            eventHandlerRegistrar.RegisterHandler<DynamicContentItemChangedEvent>(async (message, token) => await appBuilder.ApplicationServices.GetService<DynamicContentItemEventHandlers>().Handle(message));

            using (var serviceScope = appBuilder.ApplicationServices.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<MarketingDbContext>();
                dbContext.Database.MigrateIfNotApplied(MigrationName.GetUpdateV2MigrationName(ModuleInfo.Id));
                dbContext.Database.EnsureCreated();
                dbContext.Database.Migrate();
            }

            var dynamicPropertyRegistrar = appBuilder.ApplicationServices.GetRequiredService<IDynamicPropertyRegistrar>();
            dynamicPropertyRegistrar.RegisterType<DynamicContentItem>();

            var dynamicContentService = appBuilder.ApplicationServices.GetService<IDynamicContentService>();
            foreach (var id in new[] { ModuleConstants.MarketingConstants.ContentPlacesRootFolderId, ModuleConstants.MarketingConstants.CotentItemRootFolderId })
            {
                var folders = dynamicContentService.GetFoldersByIdsAsync(new[] { id }).GetAwaiter().GetResult();
                var rootFolder = folders.FirstOrDefault();
                if (rootFolder == null)
                {
                    rootFolder = new DynamicContentFolder
                    {
                        Id = id,
                        Name = id
                    };
                    dynamicContentService.SaveFoldersAsync(new[] { rootFolder }).GetAwaiter().GetResult();
                }
            }

            //Create standard dynamic properties for dynamic content item
            var dynamicPropertyService = appBuilder.ApplicationServices.GetService<IDynamicPropertyService>();
            var contentItemTypeProperty = new DynamicProperty
            {
                Id = "Marketing_DynamicContentItem_Type_Property",
                IsDictionary = true,
                Name = "Content type",
                ObjectType = typeof(DynamicContentItem).FullName,
                ValueType = DynamicPropertyValueType.ShortText,
                CreatedBy = "Auto",
            };

            dynamicPropertyService.SaveDynamicPropertiesAsync(new[] { contentItemTypeProperty }).GetAwaiter().GetResult();

            var extensionManager = appBuilder.ApplicationServices.GetService<IMarketingExtensionManager>();
            extensionManager.PromotionCondition = new PromotionConditionAndRewardTree { Children = GetConditionsAndRewards() };
            extensionManager.ContentCondition = new DynamicContentConditionTree() { Children = GetDynamicContentConditions() };

            //Next lines allow to use polymorph types in API controller methods
            var mvcJsonOptions = appBuilder.ApplicationServices.GetService<IOptions<MvcJsonOptions>>();
            mvcJsonOptions.Value.SerializerSettings.Converters.Add(new PolymorphicMarketingJsonConverter(appBuilder.ApplicationServices.GetService<IMarketingExtensionManager>()));
            mvcJsonOptions.Value.SerializerSettings.Converters.Add(new RewardJsonConverter());

            AbstractTypeFactory<IConditionTree>.RegisterType<PromotionConditionAndRewardTree>();
            AbstractTypeFactory<IConditionTree>.RegisterType<DynamicContentConditionTree>();

            AbstractTypeFactory<IConditionTree>.RegisterType<BlockContentCondition>();

            AbstractTypeFactory<IConditionTree>.RegisterType<BlockCustomerCondition>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionIsRegisteredUser>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionIsEveryone>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionIsFirstTimeBuyer>();

            AbstractTypeFactory<IConditionTree>.RegisterType<BlockCatalogCondition>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionAtNumItemsInCart>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionAtNumItemsInCategoryAreInCart>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionAtNumItemsOfEntryAreInCart>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionCartSubtotalLeast>();

            AbstractTypeFactory<IConditionTree>.RegisterType<BlockCartCondition>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionCategoryIs>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionCodeContains>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionCurrencyIs>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionEntryIs>();
            AbstractTypeFactory<IConditionTree>.RegisterType<ConditionInStockQuantity>();

            AbstractTypeFactory<IConditionTree>.RegisterType<BlockReward>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardCartGetOfAbsSubtotal>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardCartGetOfRelSubtotal>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardItemGetFreeNumItemOfProduct>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardItemGetOfAbs>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardItemGetOfAbsForNum>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardItemGetOfRel>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardItemGetOfRelForNum>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardItemGiftNumItem>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardShippingGetOfAbsShippingMethod>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardShippingGetOfRelShippingMethod>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardPaymentGetOfAbs>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardPaymentGetOfRel>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardItemForEveryNumInGetOfRel>();
            AbstractTypeFactory<IConditionTree>.RegisterType<RewardItemForEveryNumOtherItemInGetOfRel>();

            AbstractTypeFactory<PromotionReward>.RegisterType<GiftReward>();
            AbstractTypeFactory<PromotionReward>.RegisterType<CartSubtotalReward>();
            AbstractTypeFactory<PromotionReward>.RegisterType<CatalogItemAmountReward>();
            AbstractTypeFactory<PromotionReward>.RegisterType<PaymentReward>();
            AbstractTypeFactory<PromotionReward>.RegisterType<ShipmentReward>();
            AbstractTypeFactory<PromotionReward>.RegisterType<SpecialOfferReward>();
        }

        public void Uninstall()
        {
        }

        public async Task ExportAsync(Stream outStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback,
            ICancellationToken cancellationToken)
        {
            await _appBuilder.ApplicationServices.GetRequiredService<MarketingExportImport>().DoExportAsync(outStream, progressCallback, cancellationToken);
        }

        public async Task ImportAsync(Stream inputStream, ExportImportOptions options, Action<ExportImportProgressInfo> progressCallback,
            ICancellationToken cancellationToken)
        {
            await _appBuilder.ApplicationServices.GetRequiredService<MarketingExportImport>().DoImportAsync(inputStream, progressCallback, cancellationToken);
        }

        private List<IConditionTree> GetConditionsAndRewards()
        {
            return new List<IConditionTree>
            {
                new BlockCustomerCondition
                {
                    AvailableChildren = new List<IConditionTree>
                    {
                        new ConditionIsRegisteredUser(), new ConditionIsEveryone(),
                        new ConditionIsFirstTimeBuyer(), new UserGroupsContainsCondition()
                    }
                },
                new BlockCatalogCondition
                {
                    AvailableChildren = new List<IConditionTree>
                    {
                        new ConditionCategoryIs(), new ConditionCodeContains(), new ConditionCurrencyIs(),
                        new ConditionEntryIs(), new ConditionInStockQuantity()
                    }
                },
                new BlockCartCondition
                {
                    AvailableChildren = new List<IConditionTree>
                    {
                        new ConditionAtNumItemsInCart(), new ConditionAtNumItemsInCategoryAreInCart(),
                        new ConditionAtNumItemsOfEntryAreInCart(), new ConditionCartSubtotalLeast()
                    }
                },
                new BlockReward
                {
                    AvailableChildren = new List<IConditionTree>
                    {
                        new RewardCartGetOfAbsSubtotal(), new RewardCartGetOfRelSubtotal(), new RewardItemGetFreeNumItemOfProduct(), new RewardItemGetOfAbs(),
                        new RewardItemGetOfAbsForNum(), new RewardItemGetOfRel(), new RewardItemGetOfRelForNum(),
                        new RewardItemGiftNumItem(), new RewardShippingGetOfAbsShippingMethod(), new RewardShippingGetOfRelShippingMethod(), new RewardPaymentGetOfAbs(),
                        new RewardPaymentGetOfRel(), new RewardItemForEveryNumInGetOfRel(), new RewardItemForEveryNumOtherItemInGetOfRel()
                    }
                }
            };
        }

        private List<IConditionTree> GetDynamicContentConditions()
        {
            var retVal = new List<IConditionTree>
            {
                new BlockContentCondition { AvailableChildren = new List<IConditionTree>
                {
                    new ConditionGeoTimeZone(), new ConditionGeoZipCode(), new ConditionStoreSearchedPhrase(), new ConditionAgeIs()
                    , new ConditionGenderIs(), new ConditionGeoCity(), new ConditionGeoCountry(), new ConditionGeoState()
                    , new ConditionLanguageIs(), new UserGroupsContainsCondition()
                }}
            };
            return retVal;
        }
    }
}
