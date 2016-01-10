(function (ng, $) {
	'use strict';

	ng.module('extensions.common', [])
		.filter('primary', [
			function() {
				return function(entities) {
					return $.grep(entities, function(entity) {
						return entity.isPrimary;
					});
				};
			}
		])
		.filter('alloy', [
			function() {
				return function (entities, alloyType) {
					if (!alloyType)
						return entities;

					return $.grep(entities, function(entity) {
						return entity.alloyType == alloyType;
					});
				}
			}
		])
		.filter('roll', [
			function() {
				return function (entities, rollType) {
					if (!rollType)
						return entities;

					return $.grep(entities, function(entity) {
						return entity.rollType == rollType;
					});
				}
			}
		])
		.filter('manufacturer', [
			function() {
				return function (entities, manufacturerId) {
					if (!manufacturerId)
						return entities;

					return $.grep(entities, function(entity) {
						return entity.manufacturerId == manufacturerId;
					});
				}
			}
		])
		.directive('btnBack', [
			'$location', '$state',
			function ($location, $state) {
				return {
					restrict: 'A',
					link: function (scope, element, attrs) {
						$(element[0]).on('click', function () {
							var current = $state.$current.name;
							var prev = getUpperState(current, 1);

							var level = 2;
							while (prev == current && prev != '') {
								prev = getUpperState(current, level++);
							}

							if (prev != '')
								$state.go(prev);
							else
								$location.url('/');
						});

						var getUpperState = function (original, level) {
							var path = original.split('.');
							for (var i = 0; i < level; i++) {
								if (path.length)
									path.pop(); // remove last part
								else
									return '';
							}

							var state = $state.get(path.join('.'));

							// for breadcrumbs to work, abstract states contain a special property 
							// which points to a state which should be default for current abstract one.
							if (state.abstract) {
								state = $state.get(state.proxy);
							};

							return state.name;
						}
					}
				};
			}
		])
		.directive('btnReload', [
			'$window',
			function ($window) {
				return {
					restrict: 'A',
					link: function (scope, element) {
						$(element[0]).on('click', function () {
							$window.location.reload();
							scope.$apply();
						});
					}
				}
			}
		]);

	ng.module('services.notification', [])
		.factory('$notify', [
			function () {
				toastr.options.positionClass = "toast-bottom-right";

				return toastr;
			}
		]);

})(angular, jQuery);