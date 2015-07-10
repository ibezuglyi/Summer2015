
$(document).ready(function () {
    deleteRowOnClick();
    addRowOnClick();
    rangeInputHandling();
});

function rangeInputHandling() {
    setRangeInputValue();
    $("body").on('input', "input[type='range']", function () {
        $(this).closest(".range-field").siblings(".value-field").html(this.value);
    });
}

function addRangeInputValue(parent) {
    var valueField = $(parent).find(".value-field")
    var value = valueField.closest('range-field').siblings(".value-field").attr('value');
    valueField.html(value);
};

function setRangeInputValue() {
    $("input[type='range']").each(function (index, input) {
        $(input).closest(".range-field").siblings(".value-field").html($(input).attr('value'));
    });
}

function deleteRowOnClick() {
    $("#skills").on('click', ".delete-skill", function (event) {
        $(this).closest(".skill-row").remove();
    });
}

function addRowOnClick() {
    $(".add-skill").click(function (event) {
        addRow("#skills");
    });
}

var addRow = (function () {
    var index = $(".blank-row .row input[type='hidden']").val();
    return function (parent) {
        var blankRow = $(".blank-row").clone(false);
        prepareSnippet(blankRow, index);
        addRangeInputValue(blankRow);
        blankRow.appendTo(parent);
        index++;
    };
})();

function addIdAndName (element, attrValue) {
    element.attr("id", attrValue).attr("name", attrValue);
}

function prepareSnippet (blankRow, index) {
    addIndexToSkill(blankRow, index);
    addIdAndNameToSkill(blankRow, index);
    blankRow.removeClass("blank-row hidden");
    return blankRow;
}

function addIndexToSkill (blankRow, index){
    var newSkillIndex = blankRow.find("input[type='hidden']");
    newSkillIndex.attr("value", index);
}

function addIdAndNameToSkill (blankRow, index) {
    var newSkillNameId = "Skills[" + index + "].Name";
    var newSkillLevelId = "Skills[" + index + "].Level";
    var newSkillNameInput = blankRow.find(".skill-name input[type='text']");
    var newSkillLevelInput = blankRow.find(".skill-level input[type='range']");
    addIdAndName(newSkillNameInput, newSkillNameId);
    addIdAndName(newSkillLevelInput, newSkillLevelId);
}