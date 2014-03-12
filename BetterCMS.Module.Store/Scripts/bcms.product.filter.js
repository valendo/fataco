/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.product.filter', ['bcms.jquery', 'bcms', 'bcms.ko.extenders'],
    function ($, bcms, ko) {
        'use strict';

        var filter = {},
            selectors = {
                filterTemplate: '#bcms-filter-template'
            };

        /**
        * Assign objects to module.
        */
        filter.selectors = selectors;

        function FilterViewModel(container, onSearchClick) {
            var self = this;
            self.isVisible = ko.observable(false);

            // Actions.
            self.toggleFilter = function () {
                self.isVisible(!self.isVisible());
            };
            self.closeFilter = function () {
                self.isVisible(false);
            };
            self.searchWithFilter = function () {
                if ($.isFunction(onSearchClick)) {
                    onSearchClick();
                }
            };
        }

        filter.bind = function (container, onSearchClick) {
            var filterViewModel = new FilterViewModel(container, onSearchClick);
            ko.applyBindings(filterViewModel, container.find(selectors.filterTemplate).get(0));
        };

        /**
        * Initializes page module.
        */
        filter.init = function () {
            bcms.logger.debug('Initializing bcms.product.filter module.');
        };

        /**
        * Register initialization
        */
        bcms.registerInit(filter.init);

        return filter;
    });
