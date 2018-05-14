﻿angular.module('virtoCommerce.notificationsModule')
.controller('virtoCommerce.notificationsModule.notificationsEditController', ['$rootScope', '$scope', '$timeout', 'virtoCommerce.notificationsModule.notificationsModuleApi', 'platformWebApp.bladeNavigationService', 'platformWebApp.dialogService',
function ($rootScope, $scope, $timeout, notifications, bladeNavigationService, dialogService) {
	$scope.setForm = function (form) { 
        $scope.formScope = form; 
    }

	var blade = $scope.blade;
	blade.updatePermission = 'notiications:update';
	var codemirrorEditor;
	blade.parametersForTemplate = [];
    $scope.isValid = false;

	blade.initialize = function () {
		blade.isLoading = true;
        notifications.getNotificationByType({ type: blade.type }, function(data) {
            blade.isLoading = false;
            setNotification(data);
        })
	};
    
	function setNotification(data) {
		blade.tenantId = data.tenantId;
		blade.tenantType = data.tenantType;
        blade.currentEntity = angular.copy(data);
		blade.origEntity = angular.copy(blade.currentEntity);
        $scope.isValid = false;
	};

	function pluckAddress(address) {
		if (address) {
			return _(address).pluck('value');	
		}
		return address;
	}

	blade.updateNotification = function () {
		blade.isLoading = true;
        blade.currentEntity.cc = pluckAddress(blade.currentEntity.cc);
        blade.currentEntity.bcc = pluckAddress(blade.currentEntity.bcc);
		notifications.updateNotification({ type: blade.type }, blade.currentEntity, function () {
			blade.isLoading = false;
			blade.origEntity = angular.copy(blade.currentEntity);
			blade.parentBlade.refresh();
		}, function (error) {
			bladeNavigationService.setError('Error ' + error.status, blade);
		});
	};

	$scope.blade.toolbarCommands = [
		{
			name: "platform.commands.save", icon: 'fa fa-save',
			executeMethod: blade.updateNotification,
			canExecuteMethod: canSave,
			permission: blade.updatePermission
		},
		{
			name: "platform.commands.undo", icon: 'fa fa-undo',
			executeMethod: function () {
				blade.currentEntity = angular.copy(blade.origEntity);
			},
			canExecuteMethod: isDirty,
			permission: blade.updatePermission
		}
	];

	$scope.editorOptions = {
		lineWrapping: true,
		lineNumbers: true,
		parserfile: "liquid.js",
		extraKeys: { "Ctrl-Q": function (cm) { cm.foldCode(cm.getCursor()); } },
		foldGutter: true,
		gutters: ["CodeMirror-linenumbers", "CodeMirror-foldgutter"],
		onLoad: function (_editor) {
			codemirrorEditor = _editor;
		},
		mode: "liquid-html"
	};
    
    $scope.$watch("blade.currentEntity", function () {
		$scope.isValid = $scope.formScope && $scope.formScope.$valid;
	}, true); 

	function isDirty() {
        return (!angular.equals(blade.origEntity, blade.currentEntity) || blade.isNew) && blade.hasUpdatePermission();
	}

	function canSave() {
        return isDirty() && $scope.isValid;
	}

	blade.onClose = function (closeCallback) {
		bladeNavigationService.showConfirmationIfNeeded(isDirty(), canSave(), blade, blade.updateTemplate, closeCallback, "notifications.dialogs.notification-details-save.title", "notifications.dialogs.notification-details-save.message");
	};

	blade.headIcon = 'fa-envelope';

	blade.initialize();
}]);