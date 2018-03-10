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


    $('.artist-suggestion').autocomplete({
        minLength: 3,
        source: function(request, response) {
            var endpoint = $('.artist-suggestion').attr('artist-suggestion-url');
            console.log("hitting url: " + endpoint);
            if (endpoint === undefined)
                return;
            $.ajax({
                url: endpoint,
                type: 'GET',
                dataType: 'json',
                data: request,
                success: function(data) {
                    response($.map(data,
                        function(item) {
                            return {
                                label: item.name,
                                id: item.id,
                                url: item.imgUrl
                            }
                        }));
                }
            });
        },
        select: function(event, ui) {
            $(".artist-id").val(ui.item.id);
        }
    })
        .autocomplete("instance")._renderItem = function (ul, item) {
            return $("<li class=\"list-group-item autocomplete-item\">")
                .append("<div>" + item.label + "</div>")
                .appendTo(ul);
        };;
    //.autocomplete("instance")._renderItem = function(ul, item) {
    //    return $("<li>")
    //        .append("<div><img class=\"img img-responsive\" src=\"" + item.url + "\"></img><br>" + item.label + "</div>")
    //        .appendTo(ul);
    //};

});