angular.module('virtoCommerce.exportModule')
    .controller('virtoCommerce.exportModule.exporterController', ['$scope', 'platformWebApp.bladeNavigationService', 'virtoCommerce.exportModule.exportModuleApi', function ($scope, bladeNavigationService, exportApi) {
        var blade = $scope.blade;
        blade.canStartProcess = false;
        blade.isLoading = true;

        function initializeBlade() {

            exportApi.getProviders(function (result) {
                if (result && result.length) {
                    blade.providers =
                        _.map(result, function (item) { return { id: item.typeName, name: item.typeName } });
                    blade.exportDataRequest.providerName = blade.providers[0];
                }
            });

            blade.isLoading = false;
        }

        $scope.$on("new-notification-event", function (event, notification) {
            if (blade.notification && notification.id === blade.notification.id) {
                angular.copy(notification, blade.notification);
                if (notification.errorCount > 0) {
                    bladeNavigationService.setError('Export error', blade);
                }
            }
        });

        var commandCancel = {
            name: 'platform.commands.cancel',
            icon: 'fa fa-times',
            canExecuteMethod: function () {
                return blade.notification && !blade.notification.finished;
            },
            executeMethod: function () {
                exportApi.cancel({ jobId: blade.notification.jobId }, function (data) {
                });
            }
        };

        $scope.startExport = function () {
            blade.isLoading = true;
            blade.exportDataRequest.providerName = blade.exportDataRequest.providerName.name;
            exportApi.runExport(blade.exportDataRequest,
                function (data) {
                    blade.notification = data;
                    blade.isLoading = false;
                });

            blade.toolbarCommands = [commandCancel];
        };

        $scope.blade.headIcon = 'fa-upload';

        initializeBlade();
    }]);
