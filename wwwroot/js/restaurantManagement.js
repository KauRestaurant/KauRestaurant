document.addEventListener("DOMContentLoaded", function () {
    // Select all number input fields for potential formatting
    const numberInputs = document.querySelectorAll('input[type="number"]');

    // Attach a change event to each number input
    numberInputs.forEach(function (input) {
        input.addEventListener("change", function () {
            // Convert the input's current value to a float
            let value = parseFloat(input.value);

            // If it's a valid number, fix it to two decimal places
            if (!isNaN(value)) {
                input.value = value.toFixed(2);
            }
        });
    });
});
