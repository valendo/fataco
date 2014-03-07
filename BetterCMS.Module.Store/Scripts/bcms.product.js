/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.product', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.role', 'bcms.media', 'bcms.messages', 'bcms.grid', 'bcms.ko.extenders', 'bcms.redirect'],
    function ($, bcms, modal, siteSettings, dynamicContent, role, media, messages, grid, ko, redirect) {
        'use strict';

        var product = {},
            productSelectors = {
                siteSettingsProductCreateButton: "#bcms-create-product-button",
                productsForm: '#bcms-products-form',
                productForm: 'form:first',
                productsSearchButton: '#bcms-products-search-btn',
                productsSearchField: '.bcms-search-block input.bcms-editor-field-box',
                productCells: 'td',
                productEditButton: '.bcms-icn-edit',
                productRowDeleteButton: '.bcms-grid-item-delete-button',
                productParentRow: 'tr:first',
                productCodeCell: '.bcms-product-Code',
                productRowTemplate: '#bcms-products-list-row-template',
                productTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                productRowTemplateFirstRow: 'tr:first'
            },
            productLinks = {
                //products link
                loadSiteSettingsProductsUrl: null,
                loadCreateProductUrl: null,
                loadEditProductUrl: null,
                deleteProductUrl: null
            },
            productsContainer = null,
            productViewModel = null;

        // Assign objects to module.
        product.links = productLinks;
        //product.globalization = globalization;
        product.selectors = productSelectors;

        /**
        * Initializes roles list
        */
        product.initializeProductsList = function (container) {
            productsContainer = container;
            initializeSiteSettingsProductsList();
        };

        /**
        * Submits products list seach/sort/paging form
        */
        function searchSiteSettingsProducts(form) {
            grid.submitGridForm(form, function (htmlContent) {
                productsContainer.html(htmlContent);
                initializeSiteSettingsProductsList();
            });
        }

        /**
        * Initailizes site settings products list
        */
        function initializeSiteSettingsProductsList() {
            var form = productsContainer.find(product.selectors.productsForm);

            grid.bindGridForm(form, function (htmlContent) {
                productsContainer.html(htmlContent);
                initializeSiteSettingsProductsList();
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSiteSettingsProducts(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(product.selectors.productsSearchField), {
                preventedEnter: function () {
                    searchSiteSettingsProducts(form);
                },
            });

            form.find(product.selectors.productsSearchButton).on('click', function () {
                searchSiteSettingsProducts(form);
            });

            productsContainer.find(product.selectors.siteSettingsProductCreateButton).on('click', function () {
                createProduct();

            });

            initializeSiteSettingsProductsListItem(productsContainer);
            //filter.bind(productsContainer, function () {
            //    searchSiteSettingsProducts(form);
            //});

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(form.find(product.selectors.productsSearchField), true);
        }

        /**
        * Initailizes site settings products list items
        */
        function initializeSiteSettingsProductsListItem(container) {
            container.find(product.selectors.productCells).on('click', function () {
                editProduct($(this));
                return false;
            });

            container.find(product.selectors.productRowDeleteButton).on('click', function () {
                deleteProduct($(this));
                return false;
            });
        }

        /**
        * Opens dialog for creating a product.
        */
        function createProduct() {
            var onSaveCallback = function (json) {
                messages.refreshBox(productsContainer, json);
                if (json.Success) {
                    var rowtemplate = $(product.selectors.productRowTemplate),
                        newRow = $(rowtemplate.html()).find(product.selectors.productRowTemplateFirstRow);

                    setProductFields(newRow, json.Data);
                    newRow.insertBefore(productsContainer.find(product.selectors.productTableFirstRow));
                    initializeSiteSettingsProductsListItem(newRow);
                    grid.showHideEmptyRow(productsContainer);
                }
            };

            openProductEditForm('add new', product.links.loadCreateProductUrl, onSaveCallback);
        }

        /**
        * Opens dialog for editing a product.
        */
        function editProduct(self) {
            var row = self.parents(product.selectors.productParentRow),
                id = row.find(product.selectors.productEditButton).data('id'),
                onSaveCallback = function (json) {
                    messages.refreshBox(productsContainer, json);
                    if (json.Success) {
                        setProductFields(row, json.Data);
                        grid.showHideEmptyRow(productsContainer);
                    }
                };

            openProductEditForm('edit product', $.format(product.links.loadEditProductUrl, id), onSaveCallback);
        }

        /**
        * Open dialog for edit/create a product
        */
        function openProductEditForm(title, url, onSaveCallback) {
            modal.open({
                title: title,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: initializeEditProductForm,

                        postSuccess: onSaveCallback
                    });
                }
            });
        }

        function ProductViewModel(productData) {
            var self = this;
            self.id = productData.Id;
            self.code = productData.Code;
            return self;
        }

        /**
        * Initializes edit/create product form
        */
        function initializeEditProductForm(dialog, content) {

            var productData = content.Data,
            form = dialog.container.find(product.selectors.productForm);

            productViewModel = new ProductViewModel(productData);
            ko.applyBindings(productViewModel, form.get(0));
        }

        /**
        * Set values, returned from server to row fields
        */
        function setProductFields(row, json) {
            row.find(product.selectors.productEditButton).data('id', json.Id);
            row.find(product.selectors.productRowDeleteButton).data('id', json.Id);
            row.find(product.selectors.productRowDeleteButton).data('version', json.Version);
            row.find(product.selectors.productCodeCell).html(json.Code);
        }

        /**
        * Deletes product from site settings products list.
        */
        function deleteProduct(self) {
            var row = self.parents(product.selectors.productParentRow),
                id = self.data('id'),
                version = self.data('version'),
                code = row.find(product.selectors.productCodeCell).html(),
                url = $.format(product.links.deleteProductUrl, id, version),
                message = $.format("Are you sure delete '{0}'?", code),
                onDeleteCompleted = function (json) {
                    try {
                        messages.refreshBox(productsContainer, json);
                        if (json.Success) {
                            row.remove();
                            grid.showHideEmptyRow(productsContainer);
                        }
                    } finally {
                        confirmDialog.close();
                    }
                },
                confirmDialog = modal.confirm({
                    content: message,
                    onAccept: function () {
                        $.ajax({
                            type: 'POST',
                            url: url,
                            contentType: 'application/json; charset=utf-8',
                            dataType: 'json',
                            cache: false
                        })
                            .done(function (json) {
                                onDeleteCompleted(json);
                            })
                            .fail(function (response) {
                                onDeleteCompleted(bcms.parseFailedResponse(response));
                            });
                        return false;
                    }
                });
        }

        /**
        * Initializes product module
        */
        product.init = function () {
            bcms.logger.debug('Initializing bcms.store module.');
        };

        bcms.registerInit(product.init);

        return product;
    });