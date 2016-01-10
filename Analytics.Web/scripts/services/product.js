(function(ng, $) {

	ng.module('services.product', ['ngResource'])
		.factory('Product', [
			'$resource',
			function($resource) {
				return $resource(
					$.url + '/api/product/:id',
					{ id: '@productId' },
					{
						'getPriceExtras': { method: 'GET', url: $.url + '/api/product/getPriceExtras', isArray: true },
						'getFiltered': { method: 'POST', url: $.url + '/api/product/getFiltered', isArray: true },
						'getFilteredWithPrices': { method: 'POST', url: $.url + '/api/product/getFilteredWithPrices', isArray: true },
						'getPriceHistory': { method: 'GET', url: $.url + '/api/product/getPriceHistory', isArray: true },
					});
			}
		]);

})(angular, jQuery);