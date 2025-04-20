$(document).ready(function () {
    // Format numbers in input fields to 2 decimal places
    $('input[type="number"]').on('change', function () {
        const val = parseFloat($(this).val());
        if (!isNaN(val)) {
            $(this).val(val.toFixed(2));
        }
    });
});
