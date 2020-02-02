jQuery(document).ready(function ($) {
    console.log("Validate");
    console.log($("#add-course-form"));
    $("#add-course-form").validate({
        onfocusout: false,
        onkeyup: false,
        onclick: false,
        rules: {
            "courseName": {
                required: true,
                maxlength: 255
            },
            "price": {
                required: true
            }
        }
    });
});