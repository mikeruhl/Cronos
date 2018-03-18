$(document).ready(function() {
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
        .autocomplete("instance")._renderItem = function(ul, item) {
            return $("<li class=\"list-group-item autocomplete-item\">")
                .append("<div>" + item.label + "</div>")
                .appendTo(ul);
        };
});