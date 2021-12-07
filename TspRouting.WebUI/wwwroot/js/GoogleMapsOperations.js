$('.trips').each(function (i, e) {
    var _this = $(this);
    var element = $(e).children('td');
    element.click(function () {
        //console.log("Clicked! " + _this.data('url') + " " + _this.data('id'));
        $("#partialMap").load(_this.data('url'), { id: _this.data('id') }, function () {
            $("#partialMap").slideDown('200');
        });
    });

});