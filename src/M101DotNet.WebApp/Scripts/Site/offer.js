$(document).ready(function () {
    LeanModal();
    GetOfferToRemoveOnClick();
});

function LeanModal() {
    $('.modal-trigger').leanModal({
    });
}

function GetOfferToRemoveOnClick() {
    $('.modal-trigger').click(function () {
        $('#offerId').val($(this).attr('data-offerid'));
    });
};
