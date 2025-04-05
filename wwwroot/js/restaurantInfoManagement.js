// Enable/disable time inputs based on checkbox status
document.addEventListener('DOMContentLoaded', function() {
    function toggleTimeFields(checkboxId, timeFieldClass) {
        const checkbox = document.getElementById(checkboxId);
        if (!checkbox) return; // Guard against null references

        const timeFields = document.querySelectorAll(`.${timeFieldClass}`);

        checkbox.addEventListener('change', function() {
            for (let field of timeFields) {
                field.disabled = !this.checked;
            }
        });

        // Initialize on page load
        for (let field of timeFields) {
            field.disabled = !checkbox.checked;
        }
    }

    // Setup for each meal type
    toggleTimeFields('servesBreakfastTop', 'breakfast-time-top');
    toggleTimeFields('servesLunchTop', 'lunch-time-top');
    toggleTimeFields('servesDinnerTop', 'dinner-time-top');

    // Handle form submission - enable disabled fields before submitting
    const restaurantForm = document.querySelector('form[asp-controller="RestaurantInfoManagement"]');
    if (restaurantForm) {
        restaurantForm.addEventListener('submit', function(e) {
            // Enable all disabled fields before submitting
            const disabledFields = document.querySelectorAll('input[disabled]');
            disabledFields.forEach(field => {
                field.disabled = false;
            });
        });
    }
});
