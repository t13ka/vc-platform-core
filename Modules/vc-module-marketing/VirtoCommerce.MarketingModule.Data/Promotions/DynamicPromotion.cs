using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VirtoCommerce.CoreModule.Core.Common;
using VirtoCommerce.CoreModule.Core.Conditions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions;
using VirtoCommerce.MarketingModule.Core.Model.Promotions.Search;
using VirtoCommerce.MarketingModule.Core.Search;
using VirtoCommerce.Platform.Core.Common;

namespace VirtoCommerce.MarketingModule.Data.Promotions
{
    public class DynamicPromotion : Promotion
    {
        private readonly ICouponSearchService _couponSearchService;
        private readonly IPromotionUsageSearchService _promotionUsageSearchService;

        private Condition[] _conditions;
        private PromotionReward[] _rewards;
        private PromotionConditionAndRewardTree _promotionConditionAndRewardTree;

        public DynamicPromotion(ICouponSearchService couponSearchService, IPromotionUsageSearchService promotionUsageSearchService)
        {
            _couponSearchService = couponSearchService;
            _promotionUsageSearchService = promotionUsageSearchService;
        }

        /// <summary>
        /// If this flag is set to true, it allows this promotion to combine with itself.
        /// Special for case when need to return same promotion rewards for multiple coupons
        /// </summary>
        public bool IsAllowCombiningWithSelf { get; set; }

        public string PredicateVisualTreeSerialized { get; set; }

        protected PromotionConditionAndRewardTree PromotionConditionAndRewardTree
        {
            get
            {
                if (_promotionConditionAndRewardTree == null && !string.IsNullOrEmpty(PredicateVisualTreeSerialized))
                {
                    _promotionConditionAndRewardTree = JsonConvert.DeserializeObject<PromotionConditionAndRewardTree>(PredicateVisualTreeSerialized
                        , new ConditionJsonConverter(), new RewardJsonConverter());
                }

                return _promotionConditionAndRewardTree;
            }
        }

        protected Condition[] Conditions => _conditions ?? (_conditions = PromotionConditionAndRewardTree.GetConditions());

        protected PromotionReward[] Rewards => _rewards ?? (_rewards = PromotionConditionAndRewardTree.GetRewards());

        public override PromotionReward[] EvaluatePromotion(IEvaluationContext context)
        {
            var result = new List<PromotionReward>();

            if (!(context is PromotionEvaluationContext promoContext))
            {
                throw new ArgumentException("context should be PromotionEvaluationContext");
            }

            IEnumerable<Coupon> validCoupons = null;
            if (HasCoupons)
            {
                validCoupons = FindValidCouponsAsync(promoContext.Coupons, promoContext.CustomerId).GetAwaiter().GetResult();
            }
            //Check coupon
            var couponIsValid = !HasCoupons || validCoupons.Any();

            //Evaluate reward for all promoEntry in context
            foreach (var promoEntry in promoContext.PromoEntries)
            {
                //Set current context promo entry for evaluation
                promoContext.PromoEntry = promoEntry;
                foreach (var reward in Rewards)
                {
                    var clonedReward = reward.Clone() as PromotionReward;
                    EvaluateReward(promoContext, couponIsValid, clonedReward);
                    //Add coupon to reward only for case when promotion contains associated coupons
                    if (!validCoupons.IsNullOrEmpty())
                    {
                        //Need to return promotion rewards for each valid coupon if promotion IsAllowCombiningWithSelf flag set
                        foreach (var validCoupon in IsAllowCombiningWithSelf ? validCoupons : validCoupons.Take(1))
                        {
                            clonedReward.Promotion = this;
                            clonedReward.Coupon = validCoupon.Code;
                            result.Add(clonedReward);
                            //Clone reward for next iteration
                            clonedReward = clonedReward.Clone() as PromotionReward;
                        }
                    }
                    else
                    {
                        result.Add(clonedReward);
                    }
                }
            }
            return result.ToArray();
        }

        protected virtual void EvaluateReward(PromotionEvaluationContext promoContext, bool couponIsValid, PromotionReward reward)
        {
            reward.Promotion = this;
            reward.IsValid = couponIsValid && Conditions.All(c => c.Evaluate(promoContext));

            //Set productId for catalog item reward
            if (reward is CatalogItemAmountReward catalogItemReward && catalogItemReward.ProductId == null)
            {
                catalogItemReward.ProductId = promoContext.PromoEntry.ProductId;
            }
        }

        protected virtual async Task<IEnumerable<Coupon>> FindValidCouponsAsync(ICollection<string> couponCodes, string userId)
        {
            var result = new List<Coupon>();
            if (!couponCodes.IsNullOrEmpty())
            {
                //Remove empty codes from input list
                couponCodes = couponCodes.Where(x => !string.IsNullOrEmpty(x)).ToList();
                if (!couponCodes.IsNullOrEmpty())
                {
                    var coupons = await _couponSearchService.SearchCouponsAsync(new CouponSearchCriteria { Codes = couponCodes, PromotionId = Id });
                    foreach (var coupon in coupons.Results.OrderBy(x => x.TotalUsesCount))
                    {
                        var couponIsValid = true;
                        if (coupon.ExpirationDate != null)
                        {
                            couponIsValid = coupon.ExpirationDate > DateTime.UtcNow;
                        }
                        if (couponIsValid && coupon.MaxUsesNumber > 0)
                        {
                            var usage = await _promotionUsageSearchService.SearchUsagesAsync(new PromotionUsageSearchCriteria { PromotionId = Id, CouponCode = coupon.Code, Take = 0 });
                            couponIsValid = usage.TotalCount < coupon.MaxUsesNumber;
                        }
                        if (couponIsValid && coupon.MaxUsesPerUser > 0 && !string.IsNullOrWhiteSpace(userId))
                        {
                            var usage = await _promotionUsageSearchService.SearchUsagesAsync(new PromotionUsageSearchCriteria { PromotionId = Id, CouponCode = coupon.Code, UserId = userId, Take = int.MaxValue });
                            couponIsValid = usage.TotalCount < coupon.MaxUsesPerUser;
                        }
                        if (couponIsValid)
                        {
                            result.Add(coupon);
                        }
                    }
                }
            }
            return result;
        }
    }
}
