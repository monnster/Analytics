(function(ng, $) {
	'use strict';

	ng.module('controllers.price', ['services.price', 'services.dictionary', 'services.product', 'directory', 'dialog', 'ui.router'])
		.config([
			'$stateProvider',
			function($stateProvider) {
				$stateProvider
					.state('int.price', {
						abstract: true,
						url: '/price',
						template: '<ui-view/>',
						proxy: 'int.price.list',
					})
					.state('int.price.list', {
						url: '/list',
						templateUrl: 'content/views/price/price-list.html',
						controller: 'PriceListCtrl',
					})
					.state('int.price.add-bulk', {
						url: '/add-bulk',
						templateUrl: 'content/views/price/price-add-bulk.html',
						controller: 'PriceBulkCtrl',
					});
			}
		])
		.controller('PriceListCtrl', [
			'$scope', '$list', '$state', '$notify', 'Manufacturer', 'RawMaterial', 'RawMaterialType', 'Price',
			function($scope, $list, $state, $notify, Manufacturer, RawMaterial, RawMaterialType, Price) {
				$scope.manufacturers = [];
				$scope.alloyTypes = $list('alloyTypes');
				$scope.rollTypes = $list('rollTypes');
				$scope.priceHistory = null;
				$scope.competitors = null;
				$scope.filter = {
					manufacturer: {},
					alloyType: null,
					rollType: null,
					date: new Date(),
				};
				$scope.activeRow = null;

				$scope.dateFormat = 'dd.MM.yyyy';
				$scope.dateOptions = {
					'year-format': "'yy'",
					'starting-day': 1
				};

				Manufacturer.query(function(data) {
					$scope.manufacturers = data;
				});

				$scope.getMargin = function (retail, price) {
					if (price > 0 && retail > 0) {
						return retail - price;
					}
					return null;
				}

				$scope.onItemClicked = function (row) {
					if ($scope.activeRow === row) {
						$scope.activeRow = null;
						$scope.competitors = null;
						return;
					}

					$scope.activeRow = row;
					$scope.getCompetitorPrices();
				};

				$scope.showPricelist = function() {
					Price.getPricelist({
						manufacturerId: $scope.filter.manufacturer.manufacturerId,
						alloyType: $scope.filter.alloyType,
						rollType: $scope.filter.rollType,
						date: $scope.filter.date,
					}, function (data) {
						$scope.activeRow = null;
						$scope.activePrice = null;
						$scope.competitors = null;
						$scope.priceHistory = null;

						$scope.data = data;
					}, function(err) {
						$notify.error('Ошибка загрузки цен.');
					});
				}

				$scope.getCompetitorPrices = function () {
					var filter = {
						productName: $scope.data.rows[$scope.activeRow],
						thicknesses: $scope.data.columns,
						alloyType: $scope.filter.alloyType,
						rollType: $scope.filter.rollType,
						date: $scope.filter.date,
						manufacturerId: $scope.filter.manufacturer.manufacturerId,
					};


					Price.getCompetitorPrices(filter, function (data) {
						$scope.competitors = data;
					}, function (err) {
						$notify.error('Ошибка загрузки цен конкурентов.');
					});
				};

				$scope.openDate = function ($event) {
					$event.preventDefault();
					$event.stopPropagation();

					$scope.openedDate = true;
				};

			}
		])
		.controller('PriceBulkCtrl', [
			'$scope', '$list', '$state', '$notify', 'Manufacturer', 'RawMaterial', 'RawMaterialType', 'PriceExtraCategory', 'Price',
			function ($scope, $list, $state, $notify, Manufacturer, RawMaterial, RawMaterialType, PriceExtraCategory, Price) {
				$scope.manufacturers = [];
				$scope.priceExtraCategories = [];
				$scope.materialTypes = [];
				$scope.alloyTypes = $list('alloyTypes');
				$scope.rollTypes = $list('rollTypes');
				$scope.priceTypes = $list('priceTypes');

				$scope.dateFormat = 'dd.MM.yyyy';
				$scope.dateOptions = {
					'year-format': "'yy'",
					'starting-day': 1
				};

				$scope.settings = {
					manufacturer: null,
					supplier: null,
					priceExtraCategory: null,
					alloyType: null,
					rollType: null,
					priceType: null,
					date: new Date(),
					prices: '',
				};


				Manufacturer.query(function(data) {
					$scope.manufacturers = data;
				});

				RawMaterialType.query(function(data) {
					$scope.materialTypes = data;
				});

				PriceExtraCategory.query(function(data) {
					$scope.priceExtraCategories = data;
				});


				$scope.dateFormat = 'dd.MM.yyyy';
				$scope.dateOptions = {
					'year-format': "'yy'",
					'starting-day': 1
				};

				$scope.parse = function (settings) {
					var data = {
						manufacturerId: settings.manufacturer.manufacturerId,
						supplierId: settings.supplier.manufacturerId,
						rollType: settings.rollType,
						alloyType: settings.alloyType,
						priceType: settings.priceType,
						priceExtraCategoryId: !!settings.priceExtraCategory
							? settings.priceExtraCategory.priceExtraCategoryId
							: 0,
						date: settings.date,
						prices: settings.prices,
					};

					Price.storePrices(data, function(response) {
						$notify.success('Цены сохранены.');
						$state.go('int.price.list');
					}, function (err) {
						console.log(err);
						$notify.error(err.data.message);
					});

				}

				$scope.openDate = function ($event) {
					$event.preventDefault();
					$event.stopPropagation();

					$scope.openedDate = true;
				};
			}
		])
	;

})(angular, jQuery);