﻿(function(ng, $) {
	'use strict';

	ng.module('directory', [])
		.factory('$list', ['$http',
			function ($http) {
				var data = {};

				data.active = {
					true: 'Активен',
					false: 'Отключен',
				};

				data.alloyTypes = {
					'Undefined': 'Не задано',
					'Regular': 'Обычный',
					'LowAlloy': 'Низколегированный',
				};

				data.rollTypes = {
					'Undefined': 'Не задано',
					'Hot': 'Горячий',
					'Cold': 'Холодный',
				};

				data.priceTypes = {
					'rawMaterial': 'Цена за материалы',
					'priceExtra': 'Наценка',
				};
			
				return function (key) {
					return data[key];
				}
			}
		])
		.filter('list', function () {
			return function (value, list) {
				return list
					? list[value]
					: value;
			};
		});
})(angular, jQuery);