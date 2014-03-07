/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.category', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.role', 'bcms.media', 'bcms.messages', 'bcms.grid', 'bcms.ko.extenders', 'bcms.redirect','bcms.product'],
    function ($, bcms, modal, siteSettings, dynamicContent, role, media, messages, grid, ko, redirect, product) {
        'use strict';

        var category = {},
            categorySelectors = {
                siteSettingsCategoryCreateButton: "#bcms-create-category-button",
                categoriesForm: '#bcms-categories-form',
                categoryForm: 'form:first',
                categoriesSearchButton: '#bcms-categories-search-btn',
                categoriesSearchField: '.bcms-search-block input.bcms-editor-field-box',
                categoryCells: 'td',
                categoryEditButton: '.bcms-icn-edit',
                categoryRowDeleteButton: '.bcms-grid-item-delete-button',
                categoryParentRow: 'tr:first',
                categoryNameCell: '.bcms-category-Name',
                categoryName_enCell: '.bcms-category-Name_en',
                categoryRowTemplate: '#bcms-categories-list-row-template',
                categoryTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                categoryRowTemplateFirstRow: 'tr:first'
            },
            categoryLinks = {
                //categories link
                loadSiteSettingsCategoriesUrl: null,
                loadCreateCategoryUrl: null,
                loadEditCategoryUrl: null,
                deleteCategoryUrl: null,
            },
            //globalization = {
            //    categoriesListTabTitle: null,
            //    categoriesAddNewTitle: null,
            //    editCategoryTitle: null,
            //    deleteCategoryConfirmMessage: null
            //},
            categoriesContainer = null,
            categoryViewModel = null;

        // Assign objects to module.
        category.links = categoryLinks;
        //category.globalization = globalization;
        category.selectors = categorySelectors;

        /**
        * Submits categories list seach/sort/paging form
        */
        function searchSiteSettingsCategories(form) {
            grid.submitGridForm(form, function (htmlContent) {
                categoriesContainer.html(htmlContent);
                initializeSiteSettingsCategoriesList();
            });
        }

        /**
        * Initailizes site settings categories list
        */
        function initializeSiteSettingsCategoriesList() {
            var form = categoriesContainer.find(category.selectors.categoriesForm);

            grid.bindGridForm(form, function (htmlContent) {
                categoriesContainer.html(htmlContent);
                initializeSiteSettingsCategoriesList();
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSiteSettingsCategories(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(category.selectors.categoriesSearchField), {
                preventedEnter: function () {
                    searchSiteSettingsCategories(form);
                },
            });

            form.find(category.selectors.categoriesSearchButton).on('click', function () {
                searchSiteSettingsCategories(form);
            });

            categoriesContainer.find(category.selectors.siteSettingsCategoryCreateButton).on('click', function () {
                createCategory();

            });

            initializeSiteSettingsCategoriesListItem(categoriesContainer);
            //filter.bind(categoriesContainer, function () {
            //    searchSiteSettingsCategories(form);
            //});

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(form.find(category.selectors.categoriesSearchField), true);
        }

        /**
        * Initailizes site settings categories list items
        */
        function initializeSiteSettingsCategoriesListItem(container) {
            container.find(category.selectors.categoryCells).on('click', function () {
                editCategory($(this));
                return false;
            });

            container.find(category.selectors.categoryRowDeleteButton).on('click', function () {
                deleteCategory($(this));
                return false;
            });
        }

        /**
        * Opens site settings store section's categories and products tabs
        */
        category.loadSiteSettingsStoreModule = function () {
            var tabs = [],
                onShow = function (container) {
                    var firstVisibleInputField = container.find('input[type=text],textarea,select').filter(':visible:first');
                    if (firstVisibleInputField) {
                        firstVisibleInputField.focus();
                    }
                },
                categories = new siteSettings.TabViewModel("Categories", category.links.loadSiteSettingsCategoriesUrl, function (container) {
                    categoriesContainer = container;
                    initializeSiteSettingsCategoriesList();
                }, onShow),
                //products = new siteSettings.TabViewModel("Products", product.links.loadSiteSettingsProductsUrl, function (container) {
                //    product.productsContainer = container;
                //    product.initializeSiteSettingsProductsList;
                //}, onShow);
                products = new siteSettings.TabViewModel("Products", product.links.loadSiteSettingsProductsUrl, product.initializeProductsList, onShow);

            tabs.push(categories);
            tabs.push(products);

            siteSettings.initContentTabs(tabs);
        };

        /**
        * Opens dialog for creating a category.
        */
        function createCategory() {
            var onSaveCallback = function (json) {
                messages.refreshBox(categoriesContainer, json);
                if (json.Success) {
                    var rowtemplate = $(category.selectors.categoryRowTemplate),
                        newRow = $(rowtemplate.html()).find(category.selectors.categoryRowTemplateFirstRow);

                    setCategoryFields(newRow, json.Data);
                    newRow.insertBefore(categoriesContainer.find(category.selectors.categoryTableFirstRow));
                    initializeSiteSettingsCategoriesListItem(newRow);
                    grid.showHideEmptyRow(categoriesContainer);
                }
            };

            openCategoryEditForm('add new', category.links.loadCreateCategoryUrl, onSaveCallback);
        }

        /**
        * Opens dialog for editing a category.
        */
        function editCategory(self) {
            var row = self.parents(category.selectors.categoryParentRow),
                id = row.find(category.selectors.categoryEditButton).data('id'),
                onSaveCallback = function (json) {
                    messages.refreshBox(categoriesContainer, json);
                    if (json.Success) {
                        setCategoryFields(row, json.Data);
                        grid.showHideEmptyRow(categoriesContainer);
                    }
                };

            openCategoryEditForm('edit category', $.format(category.links.loadEditCategoryUrl, id), onSaveCallback);
        }

        /**
        * Open dialog for edit/create a category
        */
        function openCategoryEditForm(title, url, onSaveCallback) {
            modal.open({
                title: title,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: initializeEditCategoryForm,

                        postSuccess: onSaveCallback
                    });
                }
            });
        }

        function CategoryViewModel(categoryData) {
            var self = this;
            self.id = categoryData.Id;
            self.name = categoryData.Name;
            self.name_en = categoryData.Name_en;
            return self;
        }

        /**
        * Initializes edit/create category form
        */
        function initializeEditCategoryForm(dialog, content) {

            var categoryData = content.Data,
            form = dialog.container.find(category.selectors.categoryForm);

            categoryViewModel = new CategoryViewModel(categoryData);
            ko.applyBindings(categoryViewModel, form.get(0));
        }

        /**
        * Set values, returned from server to row fields
        */
        function setCategoryFields(row, json) {
            row.find(category.selectors.categoryEditButton).data('id', json.Id);
            row.find(category.selectors.categoryRowDeleteButton).data('id', json.Id);
            row.find(category.selectors.categoryRowDeleteButton).data('version', json.Version);
            row.find(category.selectors.categoryNameCell).html(json.Name);
            row.find(category.selectors.categoryName_enCell).html(json.Name_en);
        }

        /**
        * Deletes category from site settings categories list.
        */
        function deleteCategory(self) {
            var row = self.parents(category.selectors.categoryParentRow),
                id = self.data('id'),
                version = self.data('version'),
                name = row.find(category.selectors.categoryNameCell).html(),
                url = $.format(category.links.deleteCategoryUrl, id, version),
                message = $.format("Are you sure delete '{0}'?", name),
                onDeleteCompleted = function (json) {
                    try {
                        messages.refreshBox(categoriesContainer, json);
                        if (json.Success) {
                            row.remove();
                            grid.showHideEmptyRow(categoriesContainer);
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
        * Initializes category module
        */
        category.init = function () {
            bcms.logger.debug('Initializing bcms.store module.');
        };

        bcms.registerInit(category.init);

        return category;
    });