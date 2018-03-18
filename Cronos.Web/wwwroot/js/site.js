// Write your JavaScript code.
$(document).ready(function () {
    var toggleAll = true;
    $('#album-selectall').click(function () {
        if (toggleAll)
            $('.album-check:not(.active)').click();
        else
            $('.album-check').click();
        //$('.album-check').addClass('active');
        toggleAll = !toggleAll;

        if (toggleAll)
            $(this).text("Select All");
        else
            $(this).text("Select None");
    });

    $('.album-check').click(function () {
        var checked = $('.album-check.active').length;
        if ($(this).hasClass('active'))
            checked--;
        else
            checked++;
        console.log('checked: ' + checked);
        $('.submit-albums').attr('disabled', (checked === 0));
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
    });



});