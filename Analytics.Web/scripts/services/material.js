(function(ng, $) {
	'use strict';

	ng.module('services.material', ['ngResource'])
		.factory('RawMaterial', [
			'$resource',
			function($resource) {
				return $resource($.url + '/api/rawMaterial/:id', { id: '@rawMaterialId' });
			}
		]);

})(angular, jQuery);