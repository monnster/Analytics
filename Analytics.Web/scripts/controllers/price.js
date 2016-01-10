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
				$scope.pricelist = {};
				$scope.manufacturers = [];
				$scope.alloyTypes = $list('alloyTypes');
				$scope.rollTypes = $list('rollTypes');
				$scope.priceHistory = null;
				$scope.competitorPrices = null;
				$scope.filter = {
					manufacturer: {},
					alloyType: null,
					rollType: null,
					date: new Date(),
				};
				$scope.activeCell = {
					x: null,
					y: null,
				};
				$scope.activePrice = null;

				Manufacturer.query(function(data) {
					$scope.manufacturers = data;
				});

				$scope.onItemClicked = function(col, row) {
					$scope.activeCell.x = col;
					$scope.activeCell.y = row;
					$scope.activePrice = $scope.data.prices[row][col];
					$scope.getCompetitorPrices();
				};

				$scope.showPricelist = function() {
					Price.getPricelist({
						manufacturerId: $scope.filter.manufacturer.manufacturerId,
						alloyType: $scope.filter.alloyType,
						rollType: $scope.filter.rollType,
						date: $scope.filter.date,
					}, function (data) {
						$scope.activeCell.x = null;
						$scope.activeCell.y = null;
						$scope.activePrice = null;
						$scope.competitorPrices = null;
						$scope.priceHistory = null;

						$scope.data = data;
					}, function(err) {
						$notify.error('Ошибка загрузки цен.');
					});
				}


				$scope.getCompetitorPrices = function() {
					var filter = {
						productName: $scope.data.rows[$scope.activeCell.y],
						thickness: $scope.data.columns[$scope.activeCell.x],
						alloyType: $scope.filter.alloyType,
						rollType: $scope.filter.rollType,
						date: $scope.filter.date,
						manufacturerId: $scope.filter.manufacturer.manufacturerId,
					};
					console.log($scope.activeCell);
					console.log($scope.activePrice);
					Price.getCompetitorPrices(filter, function(data) {
						$scope.competitorPrices = data;
					}, function(err) {
						$notify.error('Ошибка загрузки цен конкурентов.');
					});
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