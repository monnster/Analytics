(function(ng, $) {
	'use strict';

	ng.module('controllers.material', ['services.material', 'services.dictionary', 'services.notification', 'ui.router', 'extensions.common', 'directory'])
		.config([
			'$stateProvider',
			function($stateProvider) {
				$stateProvider
					.state('int.raw-material', {
						abstract: true,
						url: '/material',
						template: '<ui-view/>',
						proxy: 'int.raw-material.list',
					})
					.state('int.raw-material.list', {
						url: '/setup',
						templateUrl: 'content/views/material/material-list.html',
						controller: 'RawMaterialSetupCtrl',
					})
					.state('int.raw-material.detailed', {
						url: '/:manufacturerId',
						templateUrl: 'content/views/material/material-list.html',
						controller: 'RawMaterialListCtrl',
					});
			}
		])
		.controller('RawMaterialSetupCtrl', [
			'$scope', '$stateParams', '$state', '$list', 'RawMaterial', 'RawMaterialType', 'Manufacturer',
			function ($scope, $stateParams, $state, $list, RawMaterial, RawMaterialType, Manufacturer) {
				$scope.manufacturers = [];
				$scope.manufacturer = {};

				Manufacturer.query(function (data) {
					$scope.manufacturers = $.grep(data, function(mnf) {
						return mnf.isPrimary;
					});
				});

				$scope.reload = function () {
					$state.go(
						'int.raw-material.detailed',
						{ manufacturerId: $scope.manufacturer.manufacturerId },
						{ reload: true }
					);
				};
			}
		])
		.controller('RawMaterialListCtrl', [
			'$scope', '$stateParams', '$state', '$list', '$notify', 'RawMaterial', 'RawMaterialType', 'Manufacturer', 'rollFilter', 'alloyFilter',
			function($scope, $stateParams, $state, $list, $notify, RawMaterial, RawMaterialType, Manufacturer, rollFilter, alloyFilter) {
				$scope.materials = [];
				$scope.materialTypes = [];
				$scope.manufacturers = [];
				$scope.manufacturer = {};
				$scope.manufacturerId = $stateParams.manufacturerId;
				$scope.alloyTypes = $list('alloyTypes');
				$scope.rollTypes = $list('rollTypes');

				$scope.filter = {
					rollType: null,
					alloyType: null,
				};

				Manufacturer.query(function (data) {
					$scope.manufacturers = data;

					$scope.manufacturer = $.grep(data, function (item) {
						return item.manufacturerId == $scope.manufacturerId;
					})[0];
				});

				$scope.reload = function () {
					$state.go(
						'int.raw-material.detailed',
						{ manufacturerId: $scope.manufacturer.manufacturerId },
						{ reload: true }
					);
					//$scope.reloadItems();
				};

				$scope.reloadItems = function() {
					RawMaterialType.query(function (materialTypes) {

						RawMaterial.query(function (materials) {
							$scope.materialTypes = $.map(materialTypes, function (mtype, i) {
								var material = $.grep(materials, function (item) {
									return item.manufacturerId == $scope.manufacturerId
										&& item.rawMaterialTypeId == mtype.rawMaterialTypeId;
								})[0] || {};

								return ng.extend(mtype, { exists: ng.isDefined(material.rawMaterialId), rawMaterial: material });
							});

						});
					});
				}


				$scope.apply = function(materials) {
					$.each(materials, function(i, item) {
						if (ng.isDefined(item.rawMaterial.rawMaterialId) && !item.exists) {
							item.rawMaterial.$remove();
						}
						if (!ng.isDefined(item.rawMaterial.rawMaterialId) && item.exists) {
							var entity = new RawMaterial({
								rawMaterialTypeId: item.rawMaterialTypeId,
								manufacturerId: $scope.manufacturerId,
							});

							entity.$save();
						}
					});

					$notify.success('Сохранено.');
				}

				$scope.selectAllVisible = function(select) {
					var items = $.grep($scope.materialTypes, function(item) {
						return (!$scope.filter.alloyType || $scope.filter.alloyType == item.alloyType)
							&& (!$scope.filter.rollType || $scope.filter.rollType == item.rollType);
					});

					$.each(items, function (i, material) {
						material.exists = select;
					});

				}



				$scope.reloadItems();
			}
		]);

})(angular, jQuery);