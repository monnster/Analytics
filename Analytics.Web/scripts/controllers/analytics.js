(function(ng, $) {
	'use strict';

	ng.module('analytics', [])
		.config([
			'$stateProvider',
			function ($stateProvider) {
				$stateProvider
					.state('int.analytics', {
						abstract: true,
						template: '<ui-view/>',
						url: '/analytics',
						proxy: 'int.analytics.all'
					})
					.state('int.analytics.all', {
						templateUrl: '',
						url: '/all',
						proxy: 'int.dict.all'
					})
			}
		])
		


})(angular, jQuery);