(function (ng, $) {
	ng.module('controllers.dialog', ['ui.bootstrap'])
		.controller('defaultDialogCtrl', [
			'$scope', '$modalInstance', 'title', 'message',
			function($scope, $modalInstance, title, message) {

				$scope.title = title;
				$scope.message = message;

				$scope.ok = function() {
					$modalInstance.close('ok');
					$scope.$destroy();
				};

				$scope.cancel = function() {
					$modalInstance.dismiss('cancel');
					$scope.$destroy();
				};
			}
		]);

	ng.module('dialog', ['ui.bootstrap.modal', 'controllers.dialog'])
		.factory('$dialog', ['$modal', function ($modal) {
			return {
				notify: function (title, message) {
					return $modal.open({
						templateUrl: '/content/views/common/dialog-info.html',
						controller: 'defaultDialogCtrl',
						backdrop: 'static',
						keyboard: true,
						resolve: {
							title: function () { return ng.copy(title); },
							message: function () { return ng.copy(message); }
						}
					});
				},
				confirm: function (title, message) {
					return $modal.open({
						templateUrl: '/content/views/common/dialog-confirm.html',
						controller: 'defaultDialogCtrl',
						backdrop: 'static',
						keyboard: true,
						resolve: {
							title: function () { return ng.copy(title); },
							message: function () { return ng.copy(message); }
						}
					});
				},
			};
		}]);


})(angular, jQuery);
