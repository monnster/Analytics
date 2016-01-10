(function(ng, $) {

	ng.module('controllers.dictionary', ['ui.router', 'services.dictionary', 'services.notification', 'directory'])
		// define module states
		.config([
			'$stateProvider', function($stateProvider) {
				$stateProvider
					.state('int.dict', {
						abstract: true,
						template: '<ui-view/>',
						url: '/dict',
						proxy: 'int.dict.all'
					})
					.state('int.dict.all', {
						url: '/all',
						templateUrl: 'content/views/dict/dictionaries.html',
						controller: 'DictionaryHomeCtrl'
					})
					.state('int.dict.manufacturer', {
						abstract: true,
						url: '/manufacturer',
						template: '<ui-view/>',
						proxy: 'int.dict.all',
					})
					.state('int.dict.manufacturer.new', {
						url: '/new',
						templateUrl: 'content/views/dict/manufacturer-edit.html',
						controller: 'ManufacturerEditCtrl',
					})
					.state('int.dict.manufacturer.edit', {
						url: '/:id/edit',
						templateUrl: 'content/views/dict/manufacturer-edit.html',
						controller: 'ManufacturerEditCtrl',
					})
					.state('int.dict.material-type', {
						abstract: true,
						url: '/material-type',
						template: '<ui-view/>',
						proxy: 'int.dict.all',
					})
					.state('int.dict.material-type.new', {
						url: '/new',
						templateUrl: 'content/views/dict/material-type-edit.html',
						controller: 'MaterialTypeEditCtrl',
					})
					.state('int.dict.material-type.edit', {
						url: '/:id/edit',
						templateUrl: 'content/views/dict/material-type-edit.html',
						controller: 'MaterialTypeEditCtrl',
					})
					.state('int.dict.price-category', {
						abstract: true,
						url: '/price-category',
						template: '<ui-view/>',
						proxy: 'int.dict.all',
					})
					.state('int.dict.price-category.new', {
						url: '/new',
						templateUrl: 'content/views/dict/price-category-edit.html',
						controller: 'PriceCategoryEditCtrl',
					})
					.state('int.dict.price-category.edit', {
						url: '/:id/edit',
						templateUrl: 'content/views/dict/price-category-edit.html',
						controller: 'PriceCategoryEditCtrl',
					})
				;

			}
		])
		.controller('DictionaryHomeCtrl', [
			'$scope', '$list', 'Manufacturer', 'RawMaterialType', 'PriceExtraCategory',
			function($scope, $list, Manufacturer, RawMaterialType, PriceExtraCategory) {
				$scope.manufacturers = [];
				$scope.materialTypes = [];
				$scope.priceCategories = [];
				$scope.rollTypes = $list('rollTypes');
				$scope.alloyTypes = $list('alloyTypes');
 
				Manufacturer.query(function(data) {
					$scope.manufacturers = data;
				});

				RawMaterialType.query(function(data) {
					$scope.materialTypes = data;
				});

				PriceExtraCategory.query(function(data) {
					$scope.priceCategories = data;
				});
			}
		])
		.controller('ManufacturerEditCtrl', [
			'$scope', '$stateParams', '$state', '$notify', 'Manufacturer',
			function($scope, $stateParams, $state, $notify, Manufacturer) {
				$scope.manufacturer = {
					manufacturerId:  0,
					name: '',
					isPrimary: false,
				};

				if (ng.isDefined($stateParams.id)) {
					Manufacturer.get({ id: $stateParams.id }, function(data) {
						$scope.manufacturer = data;
					});
				}

				$scope.save = function(manufacturer) {
					var entity = new Manufacturer({
						manufacturerId: manufacturer.manufacturerId,
						name: manufacturer.name,
						isPrimary: manufacturer.isPrimary,
					});

					entity.$save()
						.then(function () {
							$notify.success('Сохранено');
							$state.go('int.dict.all');
						});

				}

			}
		])
		.controller('MaterialTypeEditCtrl', [
			'$scope', '$stateParams', '$state', '$list', '$notify', 'RawMaterialType',
			function ($scope, $stateParams, $state, $list, $notify, RawMaterialType) {
				$scope.materialType = {
					rawMaterialTypeId: 0,
					name: '',
					alloyType: null,
					rollType: null,
					thickness: 0,
				};

				$scope.rollTypes = $list('rollTypes');
				$scope.alloyTypes = $list('alloyTypes');

				if (ng.isDefined($stateParams.id)) {
					RawMaterialType.get({ id: $stateParams.id }, function (data) {
						$scope.materialType = data;
					});
				}

				$scope.save = function (materialType) {
					var entity = new RawMaterialType({
						rawMaterialTypeId: materialType.rawMaterialTypeId,
						name: materialType.name,
						alloyType: materialType.alloyType,
						rollType: materialType.rollType,
						thickness: materialType.thickness,
					});

					entity.$save()
						.then(function () {
							$notify.success('Сохранено');
							$state.go('int.dict.all');
						});
				}

			}
		])
		.controller('PriceCategoryEditCtrl', [
			'$scope', '$stateParams', '$state', '$notify', 'PriceExtraCategory',
			function ($scope, $stateParams, $state, $notify, PriceExtraCategory) {
				$scope.priceCategory = {
					priceExtraCategoryId: 0,
					name: '',
				};

				if (ng.isDefined($stateParams.id)) {
					PriceExtraCategory.get({ id: $stateParams.id }, function (data) {
						$scope.priceCategory = data;
					});
				}

				$scope.save = function (priceCategory) {
					var entity = new PriceExtraCategory({
						priceExtraCategoryId: priceCategory.priceExtraCategoryId,
						name: priceCategory.name,
					});

					entity.$save()
						.then(function () {
							$notify.success('Сохранено');
							$state.go('int.dict.all');
						});
				}


			}
	]);

})(angular, jQuery);