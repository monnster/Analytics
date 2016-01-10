(function(ng, $) {
	'use strict';

	ng.module('services.price', ['ngResource'])
		.factory('Price', [
			'$resource',
			function($resource) {
				return $resource(
					$.url + '/api/price/:id',
					{ id: '@priceId' },
					{
						'storePrices': { method: 'POST', url: $.url + '/api/price/store-prices' },
						'getPricelist': {method: 'POST', url: $.url + '/api/price/get-price-list' },
						'getCompetitorPrices': {method: 'POST', url: $.url + '/api/price/get-competitor-prices', isArray: true },
					}
				);
			}
		]);

})(angular, jQuery);