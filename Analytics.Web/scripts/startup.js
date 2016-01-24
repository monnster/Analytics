
(function(ng, $) {
	$.url = 'http://localhost:9000';

	ng.module('controllers', ['controllers.dictionary', 'controllers.material', 'controllers.price', 'controllers.product']);

	ng.module('services', []);

	ng.module('extensions', ['extensions.common', 'extensions.http']);

	ng.module('appAnalytics', ['controllers', 'services', 'extensions', 'ui.router'])
		.config(['$stateProvider', '$urlRouterProvider',
			function ($stateProvider, $urlRouterProvider) {
				// default route
				$urlRouterProvider.otherwise('/home');

				// each module defines its own states
				$stateProvider
					.state('int', {
						url: '^',
						abstract: true,
						template: '<ui-view />',
						proxy: 'int.home'
					});

				$stateProvider
					.state('int.home', {
						url: '/home',
						templateUrl: 'content/views/home/home.html',
						controller: 'HomeCtrl'
					});
			}
		])
		.controller('HomeCtrl', [
			'$scope',
			function($scope) {
			
			}
		]);

})(angular, jQuery);