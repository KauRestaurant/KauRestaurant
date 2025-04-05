$(document).ready(function() {
    // Make sure first tab is active (Bootstrap 5 syntax)
    var firstTab = document.querySelector('#faq-tab');
    if (firstTab) {
        new bootstrap.Tab(firstTab).show();
    }

    // Add simple formatting toolbar to textareas
    $('.content-editor').each(function() {
        var $textarea = $(this);
        var $toolbar = $('<div class="btn-toolbar mb-2" role="toolbar"></div>');

        var $btnGroup = $('<div class="btn-group me-2" role="group"></div>');

        // Add formatting buttons
        $btnGroup.append('<button type="button" class="btn btn-sm btn-outline-secondary" data-format="bold"><i class="bi bi-type-bold"></i></button>');
        $btnGroup.append('<button type="button" class="btn btn-sm btn-outline-secondary" data-format="italic"><i class="bi bi-type-italic"></i></button>');
        $btnGroup.append('<button type="button" class="btn btn-sm btn-outline-secondary" data-format="heading"><i class="bi bi-type-h1"></i></button>');

        var $btnGroup2 = $('<div class="btn-group me-2" role="group"></div>');
        $btnGroup2.append('<button type="button" class="btn btn-sm btn-outline-secondary" data-format="ul"><i class="bi bi-list-ul"></i></button>');
        $btnGroup2.append('<button type="button" class="btn btn-sm btn-outline-secondary" data-format="ol"><i class="bi bi-list-ol"></i></button>');
        $btnGroup2.append('<button type="button" class="btn btn-sm btn-outline-secondary" data-format="link"><i class="bi bi-link"></i></button>');

        $toolbar.append($btnGroup);
        $toolbar.append($btnGroup2);

        // Insert toolbar before textarea
        $textarea.before($toolbar);

        // Add click handlers
        $toolbar.find('button').click(function() {
            var format = $(this).data('format');
            var textarea = $textarea[0];
            var start = textarea.selectionStart;
            var end = textarea.selectionEnd;
            var selectedText = textarea.value.substring(start, end);
            var replacement = '';

            switch(format) {
                case 'bold':
                    replacement = '<strong>' + selectedText + '</strong>';
                    break;
                case 'italic':
                    replacement = '<em>' + selectedText + '</em>';
                    break;
                case 'heading':
                    replacement = '<h3>' + selectedText + '</h3>';
                    break;
                case 'ul':
                    var items = selectedText.split('\n');
                    replacement = '<ul>\n';
                    items.forEach(function(item) {
                        if (item.trim()) {
                            replacement += '  <li>' + item + '</li>\n';
                        }
                    });
                    replacement += '</ul>';
                    break;
                case 'ol':
                    var items = selectedText.split('\n');
                    replacement = '<ol>\n';
                    items.forEach(function(item) {
                        if (item.trim()) {
                            replacement += '  <li>' + item + '</li>\n';
                        }
                    });
                    replacement += '</ol>';
                    break;
                case 'link':
                    var url = prompt('أدخل الرابط:', 'https://');
                    if (url) {
                        replacement = '<a href="' + url + '">' + (selectedText || url) + '</a>';
                    } else {
                        return;
                    }
                    break;
            }

            // Insert the formatted text
            textarea.value = textarea.value.substring(0, start) + replacement + textarea.value.substring(end);

            // Set cursor position after the insertion
            textarea.focus();
            textarea.selectionStart = start + replacement.length;
            textarea.selectionEnd = start + replacement.length;
        });
    });

    // FAQ Edit Button
    $('.edit-faq-btn').click(function() {
        $('#edit-faq-id').val($(this).data('id'));
        $('#edit-faq-question').val($(this).data('question'));
        $('#edit-faq-answer').val($(this).data('answer'));
        $('#edit-faq-order').val($(this).data('order'));
    });

    // FAQ Delete Button
    $('.delete-faq-btn').click(function() {
        $('#delete-faq-id').val($(this).data('id'));
        $('#delete-faq-question').text($(this).data('question'));
    });

    // Terms Edit Button
    $('.edit-terms-btn').click(function() {
        var termId = $(this).data('id');
        $('#edit-terms-id').val(termId);
        $('#edit-terms-title').val($(this).data('title'));
        $('#edit-terms-order').val($(this).data('order'));

        // Get content from hidden div
        var contentElement = $('#term-content-' + termId);
        if (contentElement.length) {
            $('#edit-terms-content').val(contentElement.html());
        }
    });

    // Terms Delete Button
    $('.delete-terms-btn').click(function() {
        $('#delete-terms-id').val($(this).data('id'));
        $('#delete-terms-title').text($(this).data('title'));
    });
});
