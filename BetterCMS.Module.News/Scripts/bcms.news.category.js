/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.news.category', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.role', 'bcms.media', 'bcms.messages', 'bcms.grid', 'bcms.ko.extenders', 'bcms.redirect','bcms.news'],
    function ($, bcms, modal, siteSettings, dynamicContent, role, media, messages, grid, ko, redirect, news) {
        'use strict';

        var category = {},
            selectors = {
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
            links = {
                //categories link
                loadSiteSettingsCategoriesUrl: null,
                loadCreateCategoryUrl: null,
                loadEditCategoryUrl: null,
                deleteCategoryUrl: null
            },
            categoriesContainer = null,
            categoryViewModel = null;

        // Assign objects to module.
        //category.globalization = globalization;
        category.links = links;
        category.selectors = selectors;

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
            var form = categoriesContainer.find(selectors.categoriesForm);

            grid.bindGridForm(form, function (htmlContent) {
                categoriesContainer.html(htmlContent);
                initializeSiteSettingsCategoriesList();
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSiteSettingsCategories(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.categoriesSearchField), {
                preventedEnter: function () {
                    searchSiteSettingsCategories(form);
                },
            });

            form.find(selectors.categoriesSearchButton).on('click', function () {
                searchSiteSettingsCategories(form);
            });

            categoriesContainer.find(selectors.siteSettingsCategoryCreateButton).on('click', function () {
                createCategory();

            });

            initializeSiteSettingsCategoriesListItem(categoriesContainer);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(form.find(selectors.categoriesSearchField), true);
        }

        /**
        * Initailizes site settings categories list items
        */
        function initializeSiteSettingsCategoriesListItem(container) {
            container.find(selectors.categoryCells).on('click', function () {
                editCategory($(this));
                return false;
            });

            container.find(selectors.categoryRowDeleteButton).on('click', function () {
                deleteCategory($(this));
                return false;
            });
        }

        /**
        * Opens site settings store section's categories and products tabs
        */
        category.loadSiteSettingsNewsModule = function () {
            var tabs = [],
                onShow = function (container) {
                    var firstVisibleInputField = container.find('input[type=text],textarea,select').filter(':visible:first');
                    if (firstVisibleInputField) {
                        firstVisibleInputField.focus();
                    }
                },
                categories = new siteSettings.TabViewModel("Categories", links.loadSiteSettingsCategoriesUrl, function (container) {
                    categoriesContainer = container;
                    initializeSiteSettingsCategoriesList();
                }, onShow),
                newsList = new siteSettings.TabViewModel("News", news.links.loadSiteSettingsNewsUrl, news.initializeNewsList, onShow);

            tabs.push(newsList);
            tabs.push(categories);

            siteSettings.initContentTabs(tabs);
        };

        /**
        * Opens dialog for creating a category.
        */
        function createCategory() {
            var onSaveCallback = function (json) {
                messages.refreshBox(categoriesContainer, json);
                if (json.Success) {
                    var rowtemplate = $(selectors.categoryRowTemplate),
                        newRow = $(rowtemplate.html()).find(selectors.categoryRowTemplateFirstRow);

                    setCategoryFields(newRow, json.Data);
                    newRow.insertBefore(categoriesContainer.find(selectors.categoryTableFirstRow));
                    initializeSiteSettingsCategoriesListItem(newRow);
                    grid.showHideEmptyRow(categoriesContainer);
                }
            };

            openCategoryEditForm('add new', links.loadCreateCategoryUrl, onSaveCallback);
        }

        /**
        * Opens dialog for editing a category.
        */
        function editCategory(self) {
            var row = self.parents(selectors.categoryParentRow),
                id = row.find(selectors.categoryEditButton).data('id'),
                onSaveCallback = function (json) {
                    messages.refreshBox(categoriesContainer, json);
                    if (json.Success) {
                        setCategoryFields(row, json.Data);
                        grid.showHideEmptyRow(categoriesContainer);
                    }
                };

            openCategoryEditForm('edit category', $.format(links.loadEditCategoryUrl, id), onSaveCallback);
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
            form = dialog.container.find(selectors.categoryForm);

            categoryViewModel = new CategoryViewModel(categoryData);
            ko.applyBindings(categoryViewModel, form.get(0));
        }

        /**
        * Set values, returned from server to row fields
        */
        function setCategoryFields(row, json) {
            row.find(selectors.categoryEditButton).data('id', json.Id);
            row.find(selectors.categoryRowDeleteButton).data('id', json.Id);
            row.find(selectors.categoryRowDeleteButton).data('version', json.Version);
            row.find(selectors.categoryNameCell).html(json.Name);
            row.find(selectors.categoryName_enCell).html(json.Name_en);
        }

        /**
        * Deletes category from site settings categories list.
        */
        function deleteCategory(self) {
            var row = self.parents(selectors.categoryParentRow),
                id = self.data('id'),
                version = self.data('version'),
                name = row.find(selectors.categoryNameCell).html(),
                url = $.format(links.deleteCategoryUrl, id, version),
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
            bcms.logger.debug('Initializing bcms.news module.');
        };

        bcms.registerInit(category.init);

        return category;
    });