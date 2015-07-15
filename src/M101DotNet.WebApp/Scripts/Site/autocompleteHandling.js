function addAutocomplete(selector, options) {
    $(selector).autocomplete(options);
}

function addAutocompleteToInputField(parent, options) {
    var autocompletedInputField = parent.find(".autocomplete")
    addAutocomplete(autocompletedInputField, options);
}