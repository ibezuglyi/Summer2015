

$('.autocomplete').autocomplete({
    serviceUrl: 'SkillSuggestion/GetHints',
    onSelect: function (suggestion) {
        $(this).attr('val', suggestion);
    }
});