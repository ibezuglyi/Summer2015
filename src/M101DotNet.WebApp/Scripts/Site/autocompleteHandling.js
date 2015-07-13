

$('.autocomplete').autocomplete({
    serviceUrl: 'Candidate/GetHints',
    onSelect: function (suggestion) {
        $(this).attr('val', suggestion);
    }
});