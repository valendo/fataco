/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.news', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.media', 'bcms.messages', 'bcms.grid', 'bcms.ko.extenders', 'bcms.redirect', 'bcms.htmlEditor', 'bcms.news.filter', 'bcms.datepicker'],
    function ($, bcms, modal, siteSettings, dynamicContent, media, messages, grid, ko, redirect, htmlEditor, filter, datepicker) {
        'use strict';

        var news = {},
            selectors = {
                datePickers: '.bcms-datepicker',
                htmlEditorContent: 'bcms-content',
                htmlEditorContent_en: 'bcms-content_en',
                siteSettingsNewsCreateButton: "#bcms-create-news-button",
                newsListForm: '#bcms-news-form',
                newsForm: 'form:first',
                newsSearchButton: '#bcms-news-search-btn',
                newsSearchField: '.bcms-search-block input.bcms-editor-field-box',
                newsCells: 'td',
                newsEditButton: '.bcms-icn-edit',
                newsRowDeleteButton: '.bcms-grid-item-delete-button',
                newsParentRow: 'tr:first',
                newsTitleCell: '.bcms-news-Title',
                newsTitle_enCell: '.bcms-news-Title_en',
                newsImageCell: '.bcms-news-Image',
                newsRowTemplate: '#bcms-news-list-row-template',
                newsTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                newsRowTemplateFirstRow: 'tr:first'
            },
            links = {
                loadSiteSettingsNewsUrl: null,
                loadCreateNewsUrl: null,
                loadEditNewsUrl: null,
                deleteNewsUrl: null
            },
            newsContainer = null,
            newsViewModel = null;

        // Assign objects to module.
        news.links = links;
        news.selectors = selectors;

        news.initializeNewsList = function (container) {
            newsContainer = container;
            initializeSiteSettingsNewsList();
        };

        function searchSiteSettingsNews(form) {
            grid.submitGridForm(form, function (htmlContent) {
                newsContainer.html(htmlContent);
                initializeSiteSettingsNewsList();
            });
        }

        function initializeSiteSettingsNewsList() {
            var form = newsContainer.find(selectors.newsListForm);

            grid.bindGridForm(form, function (htmlContent) {
                newsContainer.html(htmlContent);
                initializeSiteSettingsNewsList();
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSiteSettingsNews(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.newsSearchField), {
                preventedEnter: function () {
                    searchSiteSettingsNews(form);
                },
            });

            form.find(selectors.newsSearchButton).on('click', function () {
                searchSiteSettingsNews(form);
            });

            newsContainer.find(selectors.siteSettingsNewsCreateButton).on('click', function () {
                createNews();

            });

            initializeSiteSettingsNewsListItem(newsContainer);
            filter.bind(newsContainer, function () {
                searchSiteSettingsNews(form);
            });

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(form.find(selectors.newsSearchField), true);
        }

        function initializeSiteSettingsNewsListItem(container) {
            container.find(selectors.newsCells).on('click', function () {
                editNews($(this));
                return false;
            });

            container.find(selectors.newsRowDeleteButton).on('click', function () {
                deleteNews($(this));
                return false;
            });
        }

        function createNews() {
            var onSaveCallback = function (json) {
                messages.refreshBox(newsContainer, json);
                if (json.Success) {
                    var rowtemplate = $(selectors.newsRowTemplate),
                        newRow = $(rowtemplate.html()).find(selectors.newsRowTemplateFirstRow);

                    setNewsFields(newRow, json.Data);
                    newRow.insertBefore(newsContainer.find(selectors.newsTableFirstRow));
                    initializeSiteSettingsNewsListItem(newRow);
                    grid.showHideEmptyRow(newsContainer);
                }
            };

            openNewsEditForm('add new', links.loadCreateNewsUrl, onSaveCallback);
        }

        function editNews(self) {
            var row = self.parents(selectors.newsParentRow),
                id = row.find(selectors.newsEditButton).data('id'),
                onSaveCallback = function (json) {
                    messages.refreshBox(newsContainer, json);
                    if (json.Success) {
                        setNewsFields(row, json.Data);
                        grid.showHideEmptyRow(newsContainer);
                    }
                };

            openNewsEditForm('edit news', $.format(links.loadEditNewsUrl, id), onSaveCallback);
        }

        function openNewsEditForm(title, url, onSaveCallback) {
            modal.open({
                title: title,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: initializeEditNewsForm,
                        beforePost: function () {
                            htmlEditor.updateEditorContent(selectors.htmlEditorContent);
                            htmlEditor.updateEditorContent(selectors.htmlEditorContent_en);
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

        function NewsViewModel(imageData, newsData) {
            var self = this;
            self.id = newsData.Id;
            self.title = newsData.Title;
            self.title_en = newsData.Title_en;
            self.image = ko.observable(new media.ImageSelectorViewModel(imageData));
            return self;
        }

        function initializeEditNewsForm(dialog, content) {

            var imageData = content.Data.Image,
            newsData = content.Data,
            form = dialog.container.find(selectors.newsForm);

            htmlEditor.initializeHtmlEditor(selectors.htmlEditorContent);
            htmlEditor.initializeHtmlEditor(selectors.htmlEditorContent_en);
            dialog.container.find(selectors.datePickers).initializeDatepicker();

            newsViewModel = new NewsViewModel(imageData, newsData);
            ko.applyBindings(newsViewModel, form.get(0));
        }

        /**
        * Set values, returned from server to row fields
        */
        function setNewsFields(row, json) {
            row.find(selectors.newsEditButton).data('id', json.Id);
            row.find(selectors.newsRowDeleteButton).data('id', json.Id);
            row.find(selectors.newsRowDeleteButton).data('version', json.Version);
            row.find(selectors.newsTitleCell).html(json.Title);
            row.find(selectors.newsTitle_enCell).html(json.Title_en);
            row.find(selectors.newsImageCell).attr('src', json.Image.ThumbnailUrl);
        }

        function deleteNews(self) {
            var row = self.parents(selectors.newsParentRow),
                id = self.data('id'),
                version = self.data('version'),
                title = row.find(selectors.newsTitleCell).html(),
                url = $.format(links.deleteNewsUrl, id, version),
                message = $.format("Are you sure delete '{0}'?", title),
                onDeleteCompleted = function (json) {
                    try {
                        messages.refreshBox(newsContainer, json);
                        if (json.Success) {
                            row.remove();
                            grid.showHideEmptyRow(newsContainer);
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

        news.init = function () {
            bcms.logger.debug('Initializing bcms.news module.');
        };

        bcms.registerInit(news.init);

        return news;
    });