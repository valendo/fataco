/*jslint unparam: true, white: true, browser: true, devel: true */
/*global bettercms */

bettercms.define('bcms.faq', ['bcms.jquery', 'bcms', 'bcms.modal', 'bcms.siteSettings', 'bcms.dynamicContent', 'bcms.media', 'bcms.messages', 'bcms.grid', 'bcms.ko.extenders', 'bcms.redirect', 'bcms.htmlEditor'],
    function ($, bcms, modal, siteSettings, dynamicContent, media, messages, grid, ko, redirect, htmlEditor) {
        'use strict';

        var faq = {},
            selectors = {
                htmlEditorAnswer: 'bcms-answer',
                htmlEditorAnswer_en: 'bcms-answer_en',
                siteSettingsFaqCreateButton: "#bcms-create-faq-button",
                faqListForm: '#bcms-faq-form',
                faqForm: 'form:first',
                faqSearchButton: '#bcms-faq-search-btn',
                faqSearchField: '.bcms-search-block input.bcms-editor-field-box',
                faqCells: 'td',
                faqEditButton: '.bcms-icn-edit',
                faqRowDeleteButton: '.bcms-grid-item-delete-button',
                faqParentRow: 'tr:first',
                faqQuestionCell: '.bcms-faq-Question',
                faqQuestion_enCell: '.bcms-faq-Question_en',
                faqRowTemplate: '#bcms-faq-list-row-template',
                faqTableFirstRow: 'table.bcms-tables > tbody > tr:first',
                faqRowTemplateFirstRow: 'tr:first'
            },
            links = {
                loadSiteSettingsFaqUrl: null,
                loadCreateFaqUrl: null,
                loadEditFaqUrl: null,
                deleteFaqUrl: null
            },
            faqContainer = null,
            faqViewModel = null;

        // Assign objects to module.
        faq.links = links;
        faq.selectors = selectors;

        faq.initializeFaqList = function (container) {
            faqContainer = container;
            initializeSiteSettingsFaqList();
        };

        function searchSiteSettingsFaq(form) {
            grid.submitGridForm(form, function (htmlContent) {
                faqContainer.html(htmlContent);
                initializeSiteSettingsFaqList();
            });
        }

        function initializeSiteSettingsFaqList() {
            var form = faqContainer.find(selectors.faqListForm);

            grid.bindGridForm(form, function (htmlContent) {
                faqContainer.html(htmlContent);
                initializeSiteSettingsFaqList();
            });

            form.on('submit', function (event) {
                bcms.stopEventPropagation(event);
                searchSiteSettingsFaq(form);
                return false;
            });

            bcms.preventInputFromSubmittingForm(form.find(selectors.faqSearchField), {
                preventedEnter: function () {
                    searchSiteSettingsFaq(form);
                },
            });

            form.find(selectors.faqSearchButton).on('click', function () {
                searchSiteSettingsFaq(form);
            });

            faqContainer.find(selectors.siteSettingsFaqCreateButton).on('click', function () {
                createFaq();

            });

            initializeSiteSettingsFaqListItem(faqContainer);

            // Select search (timeout is required to work on IE11)
            grid.focusSearchInput(form.find(selectors.faqSearchField), true);
        }

        function initializeSiteSettingsFaqListItem(container) {
            container.find(selectors.faqCells).on('click', function () {
                editFaq($(this));
                return false;
            });

            container.find(selectors.faqRowDeleteButton).on('click', function () {
                deleteFaq($(this));
                return false;
            });
        }

        function createFaq() {
            var onSaveCallback = function (json) {
                messages.refreshBox(faqContainer, json);
                if (json.Success) {
                    var rowtemplate = $(selectors.faqRowTemplate),
                        newRow = $(rowtemplate.html()).find(selectors.faqRowTemplateFirstRow);

                    setFaqFields(newRow, json.Data);
                    newRow.insertBefore(faqContainer.find(selectors.faqTableFirstRow));
                    initializeSiteSettingsFaqListItem(newRow);
                    grid.showHideEmptyRow(faqContainer);
                }
            };

            openFaqEditForm('add new', links.loadCreateFaqUrl, onSaveCallback);
        }

        function editFaq(self) {
            var row = self.parents(selectors.faqParentRow),
                id = row.find(selectors.faqEditButton).data('id'),
                onSaveCallback = function (json) {
                    messages.refreshBox(faqContainer, json);
                    if (json.Success) {
                        setFaqFields(row, json.Data);
                        grid.showHideEmptyRow(faqContainer);
                    }
                };

            openFaqEditForm('edit faq', $.format(links.loadEditFaqUrl, id), onSaveCallback);
        }

        function openFaqEditForm(title, url, onSaveCallback) {
            modal.open({
                title: title,
                onLoad: function (dialog) {
                    dynamicContent.bindDialog(dialog, url, {
                        contentAvailable: initializeEditFaqForm,
                        beforePost: function () {
                            htmlEditor.updateEditorContent(selectors.htmlEditorAnswer);
                            htmlEditor.updateEditorContent(selectors.htmlEditorAnswer_en);
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

        function FaqViewModel(faqData) {
            var self = this;
            self.id = faqData.Id;
            self.question = faqData.Question;
            self.question_en = faqData.Question_en;
            return self;
        }

        function initializeEditFaqForm(dialog, content) {

            var faqData = content.Data,
            form = dialog.container.find(selectors.faqForm);

            htmlEditor.initializeHtmlEditor(selectors.htmlEditorAnswer);
            htmlEditor.initializeHtmlEditor(selectors.htmlEditorAnswer_en);

            faqViewModel = new FaqViewModel(faqData);
            ko.applyBindings(faqViewModel, form.get(0));
        }

        /**
        * Set values, returned from server to row fields
        */
        function setFaqFields(row, json) {
            row.find(selectors.faqEditButton).data('id', json.Id);
            row.find(selectors.faqRowDeleteButton).data('id', json.Id);
            row.find(selectors.faqRowDeleteButton).data('version', json.Version);
            row.find(selectors.faqQuestionCell).html(json.Question);
            row.find(selectors.faqQuestion_enCell).html(json.Question_en);
        }

        function deleteFaq(self) {
            var row = self.parents(selectors.faqParentRow),
                id = self.data('id'),
                version = self.data('version'),
                title = row.find(selectors.faqQuestionCell).html(),
                url = $.format(links.deleteFaqUrl, id, version),
                message = $.format("Are you sure delete '{0}'?", title),
                onDeleteCompleted = function (json) {
                    try {
                        messages.refreshBox(faqContainer, json);
                        if (json.Success) {
                            row.remove();
                            grid.showHideEmptyRow(faqContainer);
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

        faq.init = function () {
            bcms.logger.debug('Initializing bcms.faq module.');
        };

        bcms.registerInit(faq.init);

        return faq;
    });