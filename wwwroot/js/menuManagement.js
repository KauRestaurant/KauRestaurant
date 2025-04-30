document.addEventListener('DOMContentLoaded', function () {
    // Stores the selected meals indexed by day/menuId
    const mealsByDay = {};

    // Identify all meal dropdowns
    const dropdowns = document.querySelectorAll('.meal-dropdown');
    dropdowns.forEach(dropdown => {
        // Assign an ID if none exists (for reference during changes)
        if (!dropdown.id) {
            dropdown.id = `meal-${Math.random().toString(36).slice(2, 8)}`;
        }

        // Save the original value in a dataset
        dropdown.dataset.originalValue = dropdown.value;
    });

    // Gather initial selections from the existing dropdown values
    initializeTracking();

    // Attach listeners to each dropdown
    dropdowns.forEach(dropdown => {
        // Track the dropdown's value before changes
        dropdown.addEventListener('focus', function () {
            this.dataset.beforeChange = this.value;
        });

        // When user changes selection, handle the logic below
        dropdown.addEventListener('change', handleDropdownChange);
    });

    // Validate form before submit
    const form = document.querySelector('form[asp-action="SaveMenu"]');
    if (form) {
        form.addEventListener('submit', function (e) {
            // If there's any dropdown in error state, block saving
            if (document.querySelectorAll('.meal-dropdown[data-has-error="true"]').length > 0) {
                e.preventDefault();
                alert('يرجى تصحيح الأخطاء قبل حفظ القائمة');
            }
        });
    }

    // Creates a map of currently chosen meals to prevent duplicates
    function initializeTracking() {
        dropdowns.forEach(dropdown => {
            const menuId = dropdown.dataset.menuId;
            const mealId = parseInt(dropdown.value);

            // If a valid meal is selected, track it
            if (mealId && mealId !== 0) {
                if (!mealsByDay[menuId]) {
                    mealsByDay[menuId] = new Set();
                }
                mealsByDay[menuId].add(mealId);
            }
        });
    }

    // Called whenever a dropdown's value changes
    function handleDropdownChange() {
        // If this dropdown is in an error state, revert to old or original
        if (this.dataset.hasError === 'true') {
            this.value = this.dataset.beforeChange || this.dataset.originalValue || '0';
            return;
        }

        // Extract important info from data attributes
        const menuId = this.dataset.menuId;
        const mealId = parseInt(this.value);
        const prevMealId = parseInt(this.dataset.beforeChange || '0');

        // Ensure there's a set for this day's menu
        if (!mealsByDay[menuId]) {
            mealsByDay[menuId] = new Set();
        }

        // User chose the "empty" option (value = 0)
        if (mealId === 0) {
            if (prevMealId !== 0) {
                mealsByDay[menuId].delete(prevMealId);
            }
            updateCaloriesDisplay(this, '0');
            return;
        }

        // If new meal already chosen in another dropdown for the same day, raise error
        if (isMealAlreadySelected(menuId, mealId, this.id)) {
            const mealName = this.options[this.selectedIndex].text;
            showError(this, `الوجبة "${mealName}" مختارة بالفعل في هذا اليوم. يرجى اختيار وجبة أخرى.`);
            this.value = prevMealId;
            return;
        }

        // Remove the old meal ID and add the new meal ID to the tracking set
        if (prevMealId !== 0) {
            mealsByDay[menuId].delete(prevMealId);
        }
        mealsByDay[menuId].add(mealId);

        // Update the reference stored before changes
        this.dataset.beforeChange = mealId;

        // Refresh displayed calories for the newly selected meal
        const calories = this.options[this.selectedIndex].dataset.calories || '0';
        updateCaloriesDisplay(this, calories);

        // Clear any existing error from this dropdown
        clearError(this);
    }

    // Checks all dropdowns for a duplicate meal selection on the same day
    function isMealAlreadySelected(menuId, mealId, currentDropdownId) {
        let isSelected = false;
        document.querySelectorAll(`.meal-dropdown[data-menu-id="${menuId}"]`).forEach(dropdown => {
            if (dropdown.id !== currentDropdownId && parseInt(dropdown.value) === mealId) {
                isSelected = true;
            }
        });
        return isSelected;
    }

    // Updates the text about calories near the dropdown
    function updateCaloriesDisplay(dropdown, calories) {
        const menuId = dropdown.dataset.menuId;
        const category = dropdown.dataset.category;
        const type = dropdown.dataset.type;
        const index = dropdown.dataset.index;

        // Find our info element and set the displayed calories
        const infoElement = document.getElementById(`info-${menuId}-${category}-${type}-${index}`);
        if (infoElement) {
            infoElement.innerHTML = `<div class="small fw-bold text-end text-dark">${calories} سعرة حرارية</div>`;
        }
    }

    // Displays an error message near the dropdown
    function showError(dropdown, message) {
        clearError(dropdown);

        // Build an error div for user notification
        const errorDiv = document.createElement('div');
        errorDiv.className = 'error-message invalid-feedback d-block';
        errorDiv.style.color = '#dc3545';
        errorDiv.style.fontSize = '0.875rem';
        errorDiv.style.marginTop = '0.25rem';
        errorDiv.textContent = message;

        dropdown.classList.add('is-invalid');
        dropdown.style.borderColor = '#dc3545';
        dropdown.dataset.hasError = 'true';
        dropdown.parentNode.appendChild(errorDiv);

        // Automatically clear the error after a short delay
        setTimeout(() => {
            if (dropdown.dataset.hasError === 'true') {
                clearError(dropdown);
            }
        }, 3000);
    }

    // Removes error styles and messages
    function clearError(dropdown) {
        dropdown.classList.remove('is-invalid');
        dropdown.style.borderColor = '';
        dropdown.removeAttribute('data-has-error');

        const errorMessages = dropdown.parentNode.querySelectorAll('.error-message');
        errorMessages.forEach(element => element.remove());
    }
});
