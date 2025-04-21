document.addEventListener("DOMContentLoaded", function () {
    // Find all number input fields
    const numberInputs = document.querySelectorAll('input[type="number"]');

    // Add a change event to each input
    numberInputs.forEach(function (input) {
        input.addEventListener("change", function () {
            // Get the value and make sure it's a number
            let value = parseFloat(input.value);
            if (!isNaN(value)) {
                // Format the value to 2 decimal places
                input.value = value.toFixed(2);
            }
        });
    });
});
