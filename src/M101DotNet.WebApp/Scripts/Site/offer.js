$(document).ready(function () {
    $('.modal-trigger').leanModal({
    });
});

$(document).ready(function () {
    $('.modal-trigger').click(function () {
        $('#offerId').val($(this).attr('data-offerid'));
    })
});
