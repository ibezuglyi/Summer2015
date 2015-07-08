$(document).ready(function () {
    $(".delete-skill").click(function () {
        $(this).parents(".skill-row").remove();
    })
});

