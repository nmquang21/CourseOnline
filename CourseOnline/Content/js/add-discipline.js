jQuery(document).ready(function () {
    $(document).ready(function () {
        $('.select-name').select2({
            placeholder: 'Search a Student',
            delay: 250,
            ajax: {
                url: '/Students/SearchStudent',
                type: 'GET',
                dataType: 'json',
                processResults: function (data) {
                    return {
                        results: data
                    };
                },
                cache: true
            }
        });
        $('.select-discipline').select2({
            placeholder: 'Select an item',
            minimumResultsForSearch: -1,
            ajax: {
                url: '/Discipline',
                type: 'GET',
                dataType: 'json',
                processResults: function (data) {
                    return {
                        results: data
                    };
                },
                cache: true
            }
        });
        jQuery('.btn-check-discipline').click(function () {
            var studentID = jQuery('.select-name').val();
            var disciplineID = jQuery('.select-discipline').val();
            jQuery.ajax({
                url: '/DisciplineStudents/Create',
                type: 'POST',
                dataType: 'json',
                data: {
                    studentID: studentID,
                    disciplineID: disciplineID,
                }
            }).done(function (data) {
                jQuery('.message-discipline').text(data.message)
            })
        })
    });
})