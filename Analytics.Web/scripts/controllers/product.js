(function (ng, $) {

	ng.module('controllers.product', ['ui.router', 'services.dictionary', 'services.product', 'directory', 'dialog'])
		.config([
			'$stateProvider',
			function($stateProvider) {
				$stateProvider
					.state('int.product', {
						abstract: true,
						template: '<ui-view/>',
						url: '/product',
						proxy: 'int.product.list',
					})
					.state('int.product.list', {
						url: '/list',
						templateUrl: 'content/views/product/product-list.html',
						controller: 'ProductListCtrl',
					})
					.state('int.product.new', {
						url: '/new',
						templateUrl: 'content/views/product/product-edit.html',
						controller: 'ProductEditCtrl',
					})
					.state('int.product.edit', {
						url: '/:id/edit',
						templateUrl: 'content/views/product/product-edit.html',
						controller: 'ProductEditCtrl',
					});
			}
		])
		.controller('ProductListCtrl', [
			'$scope', '$list', '$dialog', '$notify', 'Manufacturer', 'Product',
			function ($scope, $list, $dialog, $notify, Manufacturer, Product) {
				$scope.products = [];
				$scope.alloyTypes = $list('alloyTypes');
				$scope.rollTypes = $list('rollTypes');

				$scope.filter = {
					manufacturerId: null,
					alloyType: null,
					rollType: null,
					thickness: null,
					name: '',
					date: null,
				};

				Manufacturer.query(function(data) {
					$scope.manufacturers = data;
				});

				$scope.filterProducts = function(filter) {
					Product.getFiltered(filter, function(data) {
						$scope.products = data;
					});
				};

				$scope.setManufacturer = function(manufacturer) {
					$scope.filter.manufacturerId = manufacturer.manufacturerId;
				}

				$scope.delete = function(product) {
					$dialog.confirm('Подтвердите удаление', 'Продукт будет удален. Продолжить?')
						.result.then(function() {
							var entity = new Product(product);
							entity.$remove()
								.then(function() {
									$scope.products = $.grep($scope.products, function (item) {
										return item.productId != product.productId;
									});
								}, function() {
									$notify.error('Ошибка удаления.');
								});

							
						});
				}

				//$scope.filterProducts($scope.filter);
			}
		])
		.controller('ProductEditCtrl', [
			'$scope', '$stateParams', '$state', '$list', 'Product', 'RawMaterial', 'RawMaterialType', 'Manufacturer',
			function($scope, $stateParams, $state, $list, Product, RawMaterial, RawMaterialType, Manufacturer) {
				$scope.product = {
					productId: 0,
					manufacturerId: 0,
					rawMaterialId: 0,
					name: '',
					thickness: 0,
				};
				$scope.alloyTypes = $list('alloyTypes');
				$scope.rollTypes = $list('rollTypes');
				$scope.manufacturers = [];
				$scope.rawMaterials = [];
				$scope.rawMaterialsFiltered = [];

				Manufacturer.query(function(data) {
					$scope.manufacturers = data;
				});

				RawMaterial.query(function(data) {
					$scope.rawMaterials = data;
				});

				if (ng.isDefined($stateParams.id)) {
					Product.get({ id: $stateParams.id }, function(data) {
						$scope.product = data;
					});
				}

				$scope.setSupplier = function(supplier) {
					$scope.rawMaterialsFiltered = $.grep($scope.rawMaterials, function(rm) {
						return rm.manufacturerId == supplier.manufacturerId;
					});
				};

				$scope.setManufacturer = function(manufacturer) {
					$scope.product.manufacturerId = manufacturer.manufacturerId;
				}

				$scope.setRawMaterial = function (material) {
					var rawMaterial = $.grep($scope.rawMaterials, function(rm) {
						return rm.rawMaterialId == material.rawMaterialId;
					})[0];

					RawMaterialType.get({ id: rawMaterial.rawMaterialTypeId }, function(rawMaterialType) {
						$scope.product.thickness = rawMaterialType.thickness;
						$scope.product.rawMaterialId = material.rawMaterialId;
					});
				}

				$scope.save = function(product) {
					var entity = new Product({
						productId: product.productId,
						manufacturerId: product.manufacturerId,
						rawMaterialId: product.rawMaterialId,
						thickness: product.thickness,
						name: product.name,
					});

					entity.$save();

					$state.go('int.product.list');
				}
			}
		]);

})(angular, jQuery);