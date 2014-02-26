/*jslint unparam: true, white: true, browser: true, devel: true */
/*global define, console */
bettercms.define('bcms.store', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.ko.extenders', 'bcms.ko.grid'],
    function ($, bcms, modal, siteSettings, dynamicContent, ko, kogrid) {
        'use strict';
        var store = {},
            selectors = {},
            links = {
                loadSiteSettingsProductCategoriesUrl: null,
                loadProductCategoriesUrl: null,
                saveProductCategoryUrl: null,
                deleteProductCategoryUrl: null
            },
            globalization = {
                productCategoryDialogTitle: null,
                deleteProductCategoryDialogTitle: null
            };

        store.links = links;
        store.globalization = globalization;
        store.selectors = selectors;

        var ProductCategoriesListViewModel = (function (_super) {
            bcms.extendsClass(ProductCategoriesListViewModel, _super);
            function ProductCategoriesListViewModel(container, items, gridOptions) {
                _super.call(this, container, links.loadProductCategoriesUrl, items, gridOptions);
            }
            ProductCategoriesListViewModel.prototype.createItem = function (item) {
                return new ProductCategoryViewModel(this, item);
            };
            return ProductCategoriesListViewModel;
        })(kogrid.ListViewModel);

        var ProductCategoryViewModel = (function (_super) {
            bcms.extendsClass(ProductCategoryViewModel, _super);
            function ProductCategoryViewModel(parent, item) {
                _super.call(this, parent, item);
                var self = this;
                self.name = ko.observable().extend({ required: "", name: "", maxLength: { maxLength: ko.maxLength.name } });
                self.registerFields(self.name);
                self.name(item.Name);

                self.parentId = ko.observable().extend({ required: "", name: "", maxLength: { maxLength: ko.maxLength.name } });
                self.registerFields(self.parentId);
                self.parentId(item.ParentId);
            }
            ProductCategoryViewModel.prototype.getDeleteConfirmationMessage = function () {
                return $.format(globalization.deleteProductCategoryDialogTitle, this.name());
            };
            ProductCategoryViewModel.prototype.getSaveParams = function () {
                var params = _super.prototype.getSaveParams.call(this);
                params.Name = this.name();
                params.ParentId = this.parentId();
                return params;
            };
            return ProductCategoryViewModel;
        })(kogrid.ItemViewModel);

        function initializeSiteSettingsStoreProductCategories(container, json) {
            var data = (json.Success == true) ? json.Data : {};
            var viewModel = new ProductCategoriesListViewModel(container, data.Items, data.GridOptions);
            viewModel.deleteUrl = links.deleteProductCategoryUrl;
            viewModel.saveUrl = links.saveProductCategoryUrl;
            ko.applyBindings(viewModel, container.get(0));
        }

        store.loadSiteSettingsStoreProductCategories = function () {
            dynamicContent.bindSiteSettings(siteSettings, links.loadSiteSettingsProductCategoriesUrl, {
                contentAvailable: function (json) {
                    var container = siteSettings.getModalDialog().container.find('.bcms-rightcol');
                    initializeSiteSettingsStoreProductCategories(container, json);
                }
            });
        };

        store.loadDialogStoreProductCategories = function () {
            modal.edit({
                title: store.globalization.productCategoryDialogTitle,
                disableSaveDraft: true,
                isPreviewAvailable: false,
                disableSaveAndPublish: true,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, links.loadSiteSettingsProductCategoriesUrl, {
                        contentAvailable: function (dialog, json) {
                            var container = dialog.container.find('.bcms-scroll-window');
                            initializeSiteSettingsStoreProductCategories(container, json);
                        }
                    });
                }
            });
        };

        store.init = function () {
            console.log('Initializing bcms.store module.');
        };

        bcms.registerInit(store.init);
        return store;
    });