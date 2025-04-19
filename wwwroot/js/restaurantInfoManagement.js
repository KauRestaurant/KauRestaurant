// Restaurant management with day-specific meal times
document.addEventListener('DOMContentLoaded', function () {
    const days = ['Saturday', 'Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday'];
    const meals = ['Breakfast', 'Lunch', 'Dinner'];

    // Handle day dropdown changes
    const daySelector = document.getElementById('daySelector');
    const toggleContainer = document.getElementById('toggleContainer');

    if (daySelector) {
        // Function to update the toggle button based on selected day
        function updateToggleButton() {
            const selectedDayId = daySelector.value;
            const selectedPanel = document.getElementById(selectedDayId + '-panel');

            // Clear current toggle
            if (toggleContainer) {
                toggleContainer.innerHTML = '';

                if (selectedPanel) {
                    // Get the toggle from the selected day panel and clone it
                    const toggleSource = selectedPanel.querySelector('.toggle-source .form-check');
                    if (toggleSource) {
                        const toggleClone = toggleSource.cloneNode(true);
                        toggleContainer.appendChild(toggleClone);

                        // Ensure the cloned toggle works with the original input
                        const originalToggle = selectedPanel.querySelector('.day-toggle');
                        const clonedToggle = toggleContainer.querySelector('.day-toggle');

                        clonedToggle.addEventListener('change', function () {
                            originalToggle.checked = this.checked;
                            // Trigger the original toggle's change event
                            originalToggle.dispatchEvent(new Event('change'));
                        });
                    }
                }
            }
        }

        // Update toggle and show panel when dropdown changes
        daySelector.addEventListener('change', function () {
            // Hide all day panels first
            document.querySelectorAll('.day-panel').forEach(panel => {
                panel.style.display = 'none';
            });

            // Show the selected day panel
            const selectedDayId = this.value;
            const selectedPanel = document.getElementById(selectedDayId + '-panel');
            if (selectedPanel) {
                selectedPanel.style.display = 'block';
            }

            // Update toggle button
            updateToggleButton();
        });

        // Initial update of toggle button
        updateToggleButton();
    }

    // Function to initialize day toggles
    function initializeDayToggles() {
        // For each day, set up event handlers
        days.forEach(day => {
            const dayToggle = document.getElementById(`is${day}Open`);
            if (!dayToggle) return;

            // Find all meal toggles for this day
            const dayContent = document.querySelector(`.day-content[data-day="${day}"]`);
            if (!dayContent) return;

            // Get all meal toggles and time inputs for this day
            const mealToggles = dayContent.querySelectorAll('.meal-toggle');
            const allTimeInputs = dayContent.querySelectorAll('input[type="time"]');
            const allRequiredIndicators = dayContent.querySelectorAll('.required-indicator');

            // When day toggle changes
            dayToggle.addEventListener('change', function () {
                const isOpen = this.checked;

                // Enable/disable all meal toggles based on day status
                mealToggles.forEach(toggle => {
                    toggle.disabled = !isOpen;
                });

                // Enable/disable all time inputs based on day status
                if (!isOpen) {
                    allTimeInputs.forEach(input => {
                        input.disabled = true;
                    });

                    // Hide all required indicators when day is closed
                    allRequiredIndicators.forEach(indicator => {
                        indicator.classList.add('d-none');
                    });
                } else {
                    // When day is open, handle each meal individually
                    meals.forEach(meal => {
                        updateMealFieldsState(day, meal);
                    });
                }
            });

            // Initialize day toggle
            dayToggle.dispatchEvent(new Event('change'));

            // Set up meal toggles for this day
            meals.forEach(meal => {
                initializeMealToggle(day, meal);
            });
        });
    }

    // Function to initialize meal toggles
    function initializeMealToggle(day, meal) {
        const mealToggle = document.getElementById(`${day}Serves${meal}`);
        if (!mealToggle) return;

        mealToggle.addEventListener('change', function () {
            updateMealFieldsState(day, meal);
        });
    }

    // Function to update meal fields state based on toggles
    function updateMealFieldsState(day, meal) {
        const dayToggle = document.getElementById(`is${day}Open`);
        const mealToggle = document.getElementById(`${day}Serves${meal}`);

        if (!dayToggle || !mealToggle) return;

        // Day must be open AND meal must be served for fields to be enabled
        const isEnabled = dayToggle.checked && mealToggle.checked;

        // Get time fields and required indicators for this specific meal
        const timeFields = document.querySelectorAll(`.${day.toLowerCase()}-${meal.toLowerCase()}-time`);
        const requiredIndicators = document.querySelectorAll(`.day-panel .card-body:has(.${day.toLowerCase()}-${meal.toLowerCase()}-time) .required-indicator`);

        // Update time field states
        timeFields.forEach(field => {
            field.disabled = !isEnabled;

            // Hide validation errors when disabled
            const errorMsg = field.nextElementSibling;
            if (errorMsg && errorMsg.classList.contains('meal-time-error')) {
                errorMsg.style.display = 'none';
            }
        });

        // Update required indicators
        requiredIndicators.forEach(indicator => {
            indicator.classList.toggle('d-none', !isEnabled);
        });
    }

    // Initialize the UI
    initializeDayToggles();

    // Form validation and submission
    const form = document.getElementById('restaurantForm');
    if (form) {
        form.addEventListener('submit', function (event) {
            let isValid = true;

            // Clear all previous errors
            document.querySelectorAll('.meal-time-error').forEach(error => {
                error.style.display = 'none';
            });

            // For each day, validate its meals
            days.forEach(day => {
                const dayToggle = document.getElementById(`is${day}Open`);
                if (!dayToggle || !dayToggle.checked) return; // Skip if day is not open

                // For each meal type, validate if it's enabled
                meals.forEach(meal => {
                    const mealToggle = document.getElementById(`${day}Serves${meal}`);
                    if (!mealToggle || !mealToggle.checked) return; // Skip if meal is not served

                    // Validate open time
                    const openTimeField = document.querySelector(`[name="${day}${meal}OpenTime"]`);
                    if (!openTimeField || !openTimeField.value) {
                        isValid = false;
                        const errorMsg = openTimeField ? openTimeField.nextElementSibling : null;
                        if (errorMsg && errorMsg.classList.contains('meal-time-error')) {
                            errorMsg.style.display = 'block';
                        }
                    }

                    // Validate close time
                    const closeTimeField = document.querySelector(`[name="${day}${meal}CloseTime"]`);
                    if (!closeTimeField || !closeTimeField.value) {
                        isValid = false;
                        const errorMsg = closeTimeField ? closeTimeField.nextElementSibling : null;
                        if (errorMsg && errorMsg.classList.contains('meal-time-error')) {
                            errorMsg.style.display = 'block';
                        }
                    }
                });
            });

            // If validation failed, prevent submission
            if (!isValid) {
                event.preventDefault();
                return;
            }

            // Enable all disabled fields before submitting
            document.querySelectorAll('input[disabled]').forEach(input => {
                input.disabled = false;
            });
        });
    }
});
