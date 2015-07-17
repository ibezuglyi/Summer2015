$(document).ready(function () {
    $('.modal-trigger').leanModal();
});

//I don't know why it's working like this, and it isn't working
$(document).ready(function () {
    $('input[type=checkbox]').each(function (index, elem) {
        //console.log("Val elem: "+$(elem).val()); 
        //console.log("Prop elem before: " + $(elem).prop("checked"));
        $(elem).prop("checked", $(elem).val());
        //console.log("Prop elem after: " + $(elem).prop("checked"));
    });
});

$(document).ready(function () {
    $('input[type=checkbox]').click(function () {
        if ($(this).prop("checked") === true)
        {
            $(this).val("true");
        }
        else {
            $(this).val("false");
        }
    });
});