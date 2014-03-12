/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.product', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.media', 'bcms.messages', 'bcms.grid', 'bcms.ko.extenders', 'bcms.redirect', 'bcms.htmlEditor', 'bcms.product.filter'],
    function ($, bcms, modal, siteSettings, dynamicContent, media, messages, grid, ko, redirect, htmlEditor, filter) {
        'use strict';

        var product = {},
            selectors = {
                htmlEditorDescription: 'bcms-description',
                htmlEditorDescription_en: 'bcms-description_en',
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
                productImageCell: '.bcms-product-Image',
                productRowTemplate: '#bcms-products-list-row-template',
                productTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                productRowTemplateFirstRow: 'tr:first'
            },
            links = {
                //products link
                loadSiteSettingsProductsUrl: null,
                loadCreateProductUrl: null,
                loadEditProductUrl: null,
                deleteProductUrl: null
            },
            productsContainer = null,
            productViewModel = null;

        // Assign objects to module.
        //product.globalization = globalization;
        product.links = links;
        product.selectors = selectors;

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
            var form = productsContainer.find(selectors.productsForm);

            grid.bindGridForm(form, function (htmlContent) {
                productsContainer.html(htmlContent);
                initializeSiteSettingsProductsList();
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSiteSettingsProducts(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.productsSearchField), {
                preventedEnter: function () {
                    searchSiteSettingsProducts(form);
                },
            });

            form.find(selectors.productsSearchButton).on('click', function () {
                searchSiteSettingsProducts(form);
            });

            productsContainer.find(selectors.siteSettingsProductCreateButton).on('click', function () {
                createProduct();

            });

            initializeSiteSettingsProductsListItem(productsContainer);
            filter.bind(productsContainer, function () {
                searchSiteSettingsProducts(form);
            });

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(form.find(selectors.productsSearchField), true);
        }

        /**
        * Initailizes site settings products list items
        */
        function initializeSiteSettingsProductsListItem(container) {
            container.find(selectors.productCells).on('click', function () {
                editProduct($(this));
                return false;
            });

            container.find(selectors.productRowDeleteButton).on('click', function () {
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
                    var rowtemplate = $(selectors.productRowTemplate),
                        newRow = $(rowtemplate.html()).find(selectors.productRowTemplateFirstRow);

                    setProductFields(newRow, json.Data);
                    newRow.insertBefore(productsContainer.find(selectors.productTableFirstRow));
                    initializeSiteSettingsProductsListItem(newRow);
                    grid.showHideEmptyRow(productsContainer);
                }
            };

            openProductEditForm('add new', links.loadCreateProductUrl, onSaveCallback);
        }

        /**
        * Opens dialog for editing a product.
        */
        function editProduct(self) {
            var row = self.parents(selectors.productParentRow),
                id = row.find(selectors.productEditButton).data('id'),
                onSaveCallback = function (json) {
                    messages.refreshBox(productsContainer, json);
                    if (json.Success) {
                        setProductFields(row, json.Data);
                        grid.showHideEmptyRow(productsContainer);
                    }
                };

            openProductEditForm('edit product', $.format(links.loadEditProductUrl, id), onSaveCallback);
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
                        beforePost: function () {
                            htmlEditor.updateEditorContent(selectors.htmlEditorDescription);
                            htmlEditor.updateEditorContent(selectors.htmlEditorDescription_en);
                        },
                        postSuccess: onSaveCallback
                    });
                },
                onAccept: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
                },
                onClose: function () {
                    htmlEditor.destroyAllHtmlEditorInstances();
                }
            });
        }

        function ProductViewModel(imageData, productData) {
            var self = this;
            self.id = productData.Id;
            self.code = productData.Code;
            self.image = ko.observable(new media.ImageSelectorViewModel(imageData));
            return self;
        }

        /**
        * Initializes edit/create product form
        */
        function initializeEditProductForm(dialog, content) {

            var imageData = content.Data.Image,
            productData = content.Data,
            form = dialog.container.find(selectors.productForm);

            htmlEditor.initializeHtmlEditor(selectors.htmlEditorDescription);
            htmlEditor.initializeHtmlEditor(selectors.htmlEditorDescription_en);

            productViewModel = new ProductViewModel(imageData, productData);
            ko.applyBindings(productViewModel, form.get(0));
        }

        /**
        * Set values, returned from server to row fields
        */
        function setProductFields(row, json) {
            row.find(selectors.productEditButton).data('id', json.Id);
            row.find(selectors.productRowDeleteButton).data('id', json.Id);
            row.find(selectors.productRowDeleteButton).data('version', json.Version);
            row.find(selectors.productCodeCell).html(json.Code);
            row.find(selectors.productImageCell).attr('src', json.Image.ThumbnailUrl);
        }

        /**
        * Deletes product from site settings products list.
        */
        function deleteProduct(self) {
            var row = self.parents(selectors.productParentRow),
                id = self.data('id'),
                version = self.data('version'),
                code = row.find(selectors.productCodeCell).html(),
                url = $.format(links.deleteProductUrl, id, version),
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