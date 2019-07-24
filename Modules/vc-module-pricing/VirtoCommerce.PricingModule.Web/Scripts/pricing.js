//Call this to register our module to main application
var moduleName = "virtoCommerce.pricingModule";

if (AppDependencies != undefined) {
    AppDependencies.push(moduleName);
}

angular.module(moduleName, ['ui.grid.cellNav', 'ui.grid.edit', 'ui.grid.validate'])
.config(
  ['$stateProvider', function ($stateProvider) {
      $stateProvider
          .state('workspace.pricingModule', {
              url: '/pricing',
              templateUrl: '$(Platform)/Scripts/common/templates/home.tpl.html',
              controller: [
                  '$scope', 'platformWebApp.bladeNavigationService', function ($scope, bladeNavigationService) {
                      var blade = {
                          id: 'pricing',
                          title: 'pricing.blades.pricing-main.title',
                          subtitle: 'pricing.blades.pricing-main.subtitle',
                          controller: 'virtoCommerce.pricingModule.pricingMainController',
                          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/blades/pricing-main.tpl.html',
                          isClosingDisabled: true
                      };
                      bladeNavigationService.showBlade(blade);
                      //Need for isolate and prevent conflict module css to other modules 
                      $scope.moduleName = "vc-pricing";
                  }
              ]
          });
  }]
)
.run(
  ['$http', '$compile', 'platformWebApp.mainMenuService', 'platformWebApp.widgetService', '$state', 'platformWebApp.authService', 'platformWebApp.metaFormsService',
  function ($http, $compile, mainMenuService, widgetService, $state, authService, metaFormsService) {
      //Register module in main menu
      var menuItem = {
          path: 'browse/pricing',
          icon: 'fa fa-usd',
          title: 'pricing.main-menu-title',
          priority: 30,
          action: function () { $state.go('workspace.pricingModule'); },
          permission: 'pricing:access'
      };
      mainMenuService.addMenuItem(menuItem);

      //Register item prices widget
      var itemPricesWidget = {
          isVisible: function (blade) { return authService.checkPermission('pricing:read'); },
          controller: 'virtoCommerce.pricingModule.itemPricesWidgetController',
          size: [2, 1],
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/itemPricesWidget.tpl.html',
      };
      widgetService.registerWidget(itemPricesWidget, 'itemDetail');

      //Register pricelist widgets
      widgetService.registerWidget({
          isVisible: function (blade) { return !blade.isNew; },
          controller: 'virtoCommerce.pricingModule.pricesWidgetController',
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/pricesWidget.tpl.html',
      }, 'pricelistDetail');
      widgetService.registerWidget({
          controller: 'virtoCommerce.pricingModule.assignmentsWidgetController',
          template: 'Modules/$(VirtoCommerce.Pricing)/Scripts/widgets/assignmentsWidget.tpl.html',
      }, 'pricelistDetail');
      
      $http.get('Modules/$(VirtoCommerce.Pricing)/Scripts/dynamicConditions/templates.html').then(function (response) {
            // compile the response, which will put stuff into the cache
            $compile(response.data);
        });

        //
        metaFormsService.registerMetaFields('Price' + 'ExportFilter', [
        {
            name: 'pricelistSelector',
            title: "pricing.blades.assignment-detail.labels.price-list",
            templateUrl: 'Modules/$(VirtoCommerce.Pricing)/Scripts/selectors/pricelist-selector.tpl.html',
        },
        {
            name: 'startDate',
            title: "pricing.blades.assignment-detail.labels.enable-date",
            valueType: "DateTime"
        }
    ]);
  }]);
