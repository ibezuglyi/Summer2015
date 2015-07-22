$(document).ready(function () {
    LeanModal();
    BindEventToHandlerOnClick();
});

function LeanModal() {
    $('.modal-trigger').leanModal({
    });
}

function BindEventToHandlerOnClick() {
    $('.modal-trigger').click(function () {
        $('#offerId').val($(this).attr('data-offerid'));
    });
};
