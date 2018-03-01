﻿// Write your JavaScript code.
$(document).ready(function () {
    var toggleAll = true;
    $('#album-selectall').click(function() {
        $('.album-checkbox').prop("checked", toggleAll);
        toggleAll = !toggleAll;
    });

    $('input[type="checkbox"]').change(function() {
        if ($('input[type="checkbox"]:checked').length === 0) {
            $('#warningSelect').css('visibility', 'visible');
        } else {
            $('#warningSelect').css('visibility', 'collapse');
        }

    });

    $('.album-check').click(function() {
        if ($(this).hasClass('active')) {
            $(this).find('input[type="checkbox"]').prop('checked', false);
        } else {
            $(this).find('input[type="checkbox"]').prop('checked', true);
        }
    })
});