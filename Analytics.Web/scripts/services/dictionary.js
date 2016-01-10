(function (ng, $) {

	ng.module('services.dictionary', ['ngResource'])
		.factory('Manufacturer', [
			'$resource',
			function($resource) {
				return $resource($.url + '/api/manufacturer/:id/', { id: '@manufacturerId' });
			}
		])
		.factory('RawMaterialType', [
			'$resource',
			function($resource) {
				return $resource($.url + '/api/rawMaterialType/:id', { id: '@rawMaterialTypeId' });
			}
		])
		.factory('PriceExtraCategory', [
			'$resource',
			function($resource) {
				return $resource($.url + '/api/priceExtraCategory/:id', { id: '@priceExtraCategoryId' });
			}
		]);


})(angular, jQuery);