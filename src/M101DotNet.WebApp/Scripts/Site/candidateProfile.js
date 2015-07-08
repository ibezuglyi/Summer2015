
$(document).ready(function () {
    $("#skills").on('click', ".delete-skill", function (event) {
        $(this).parents(".skill-row").remove();
    });
    $(".add-skill").click(function (event) {
        addRow("#skills");
    });
});

var addRow = (function () {
    var index = $(".blank-row .row input[type='hidden']").val();
    return function (parent) {
        var blankRow = $(".blank-row").clone(false);
        var newSkillNameId = "Skills[" + index + "].Name";
        var newSkillLevelId = "Skills[" + index + "].Level";
        var newSkillNameInput = blankRow.find(".skill-name input[type='text']");
        var newSkillLevelInput = blankRow.find(".skill-level input[type='text']");
        var newSkillIndex = blankRow.find("input[type='hidden']");
        newSkillNameInput.attr("id", newSkillNameId).attr("name", newSkillNameId);
        newSkillLevelInput.attr("id", newSkillLevelId).attr("name", newSkillLevelId);
        newSkillIndex.attr("value", index);
        blankRow.removeClass("blank-row hidden");
        blankRow.appendTo(parent);
        index++;
    };
})();


