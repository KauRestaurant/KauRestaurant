// Enable/disable time inputs based on checkbox status
document.addEventListener('DOMContentLoaded', function () {
    function toggleTimeFields(checkboxId, timeFieldClass) {
        const checkbox = document.getElementById(checkboxId);
        if (!checkbox) return; // Guard against null references

        const timeFields = document.querySelectorAll(`.${timeFieldClass}`);

        checkbox.addEventListener('change', function () {
            for (let field of timeFields) {
                field.disabled = !this.checked;
                // Clear the value when unchecked to ensure proper null handling
                if (!this.checked) {
                    field.value = '';
                }

                // Hide validation error messages when checkbox is unchecked
                const errorElement = field.parentElement.querySelector('.meal-time-error');
                if (errorElement) {
                    errorElement.style.display = 'none';
                }
            }

            // Update required indicators when checkboxes change
            updateRequiredIndicators();
        });

        // Initialize on page load
        for (let field of timeFields) {
            field.disabled = !checkbox.checked;
            // Ensure disabled fields have empty values
            if (!checkbox.checked) {
                field.value = '';
            }
        }
    }

    // Setup for each meal type
    toggleTimeFields('servesBreakfastTop', 'breakfast-time-top');
    toggleTimeFields('servesLunchTop', 'lunch-time-top');
    toggleTimeFields('servesDinnerTop', 'dinner-time-top');

    // Add required indicators to meal time labels
    function updateRequiredIndicators() {
        // Update breakfast time required indicators
        updateMealTypeIndicators('servesBreakfastTop', '.card-body:has(.breakfast-time-top) .form-label');

        // Update lunch time required indicators
        updateMealTypeIndicators('servesLunchTop', '.card-body:has(.lunch-time-top) .form-label');

        // Update dinner time required indicators
        updateMealTypeIndicators('servesDinnerTop', '.card-body:has(.dinner-time-top) .form-label');
    }

    function updateMealTypeIndicators(checkboxId, labelsSelector) {
        const checkbox = document.getElementById(checkboxId);
        if (!checkbox) return;

        const labels = document.querySelectorAll(labelsSelector);
        labels.forEach(label => {
            let requiredSpan = label.querySelector('.required-indicator');
            if (!requiredSpan) {
                requiredSpan = document.createElement('span');
                requiredSpan.className = 'text-danger ms-1 required-indicator';
                requiredSpan.textContent = '*';
                label.appendChild(requiredSpan);
            }
            requiredSpan.style.display = checkbox.checked ? 'inline' : 'none';
        });
    }

    // Run initially to set up required indicators
    updateRequiredIndicators();

    // Handle form submission - validate and enable disabled fields before submitting
    const restaurantForm = document.getElementById('restaurantForm');
    if (restaurantForm) {
        restaurantForm.addEventListener('submit', function (e) {
            let isValid = true;

            // Hide all error messages first
            document.querySelectorAll('.meal-time-error').forEach(el => {
                el.style.display = 'none';
            });

            // Validate breakfast fields if breakfast is enabled
            if (document.getElementById('servesBreakfastTop').checked) {
                const breakfastOpenTime = document.querySelector('input[name="BreakfastOpenTime"]').value;
                const breakfastCloseTime = document.querySelector('input[name="BreakfastCloseTime"]').value;

                if (!breakfastOpenTime || breakfastOpenTime === '') {
                    document.querySelectorAll('.breakfast-error')[0].style.display = 'block';
                    isValid = false;
                }

                if (!breakfastCloseTime || breakfastCloseTime === '') {
                    document.querySelectorAll('.breakfast-error')[1].style.display = 'block';
                    isValid = false;
                }
            }

            // Validate lunch fields if lunch is enabled
            if (document.getElementById('servesLunchTop').checked) {
                const lunchOpenTime = document.querySelector('input[name="LunchOpenTime"]').value;
                const lunchCloseTime = document.querySelector('input[name="LunchCloseTime"]').value;

                if (!lunchOpenTime || lunchOpenTime === '') {
                    document.querySelectorAll('.lunch-error')[0].style.display = 'block';
                    isValid = false;
                }

                if (!lunchCloseTime || lunchCloseTime === '') {
                    document.querySelectorAll('.lunch-error')[1].style.display = 'block';
                    isValid = false;
                }
            }

            // Validate dinner fields if dinner is enabled
            if (document.getElementById('servesDinnerTop').checked) {
                const dinnerOpenTime = document.querySelector('input[name="DinnerOpenTime"]').value;
                const dinnerCloseTime = document.querySelector('input[name="DinnerCloseTime"]').value;

                if (!dinnerOpenTime || dinnerOpenTime === '') {
                    document.querySelectorAll('.dinner-error')[0].style.display = 'block';
                    isValid = false;
                }

                if (!dinnerCloseTime || dinnerCloseTime === '') {
                    document.querySelectorAll('.dinner-error')[1].style.display = 'block';
                    isValid = false;
                }
            }

            // If validation fails, prevent form submission
            if (!isValid) {
                e.preventDefault();
                return;
            }

            // Enable all disabled fields before submitting if validation passes
            const disabledFields = document.querySelectorAll('input[disabled]');
            disabledFields.forEach(field => {
                field.disabled = false;
            });
        });
    }
});
