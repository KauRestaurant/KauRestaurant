function prepareDeleteReview(reviewId) {
    // Place the chosen review's ID into a hidden field
    document.getElementById('deleteReviewId').value = reviewId;

    // Open the Bootstrap modal for review deletion
    var modal = new bootstrap.Modal(document.getElementById('deleteReviewModal'));
    modal.show();
}

document.addEventListener('DOMContentLoaded', function () {
    // Calculates total calories from protein, carbs, and fat
    function calculateCalories(protein, carbs, fat) {
        return Math.round((protein * 4) + (carbs * 4) + (fat * 9));
    }

    // Sets up automatic calorie calculation for Add/Edit forms
    function setupCalorieCalculation(formType) {
        const prefix = formType === 'add' ? 'meal' : 'editMeal';
        const proteinInput = document.getElementById(`${prefix}Protein`);

        // No matching form found
        if (!proteinInput) return;

        const carbsInput = document.getElementById(`${prefix}Carbs`);
        const fatInput = document.getElementById(`${prefix}Fat`);
        const caloriesInput = document.getElementById(`${prefix}Calories`);

        // Track if the user manually edits calorie field
        let caloriesManuallyEdited = false;

        // When the user changes calories manually, don't override
        caloriesInput.addEventListener('input', () => {
            caloriesManuallyEdited = true;
        });

        // Synchronize macro changes with auto-calculated calories
        const updateCalories = () => {
            if (!caloriesManuallyEdited) {
                const protein = parseFloat(proteinInput.value) || 0;
                const carbs = parseFloat(carbsInput.value) || 0;
                const fat = parseFloat(fatInput.value) || 0;
                caloriesInput.value = calculateCalories(protein, carbs, fat);
            }
        };

        // Attach calculation to protein, carbs, and fat inputs
        [proteinInput, carbsInput, fatInput].forEach(input => {
            input.addEventListener('input', updateCalories);
        });

        // If calories field is empty, initialize auto-calc
        if (!caloriesInput.value) {
            updateCalories();
        }
    }

    // Enables meal filtering by category, type, or name
    function setupMealFiltering() {
        const categoryFilter = document.getElementById('categoryFilter');
        if (!categoryFilter) return; // Not on index page
        const typeFilter = document.getElementById('typeFilter');
        const searchFilter = document.getElementById('searchFilter');
        const mealItems = document.querySelectorAll('.meal-item');

        // Filters based on selected/typed criteria
        const filterMeals = () => {
            const category = categoryFilter.value.toLowerCase();
            const type = typeFilter.value.toLowerCase();
            const search = searchFilter.value.toLowerCase();

            mealItems.forEach(item => {
                const matchCategory = category === "" || item.dataset.category.toLowerCase() === category;
                const matchType = type === "" || item.dataset.type.toLowerCase() === type;
                const matchSearch = search === "" || item.dataset.name.toLowerCase().includes(search);

                item.style.display = (matchCategory && matchType && matchSearch) ? '' : 'none';
            });
        };

        // Respond to filter changes and search input
        categoryFilter.addEventListener('change', filterMeals);
        typeFilter.addEventListener('change', filterMeals);
        searchFilter.addEventListener('keyup', filterMeals);
    }

    // Sets up the modal confirmation for deleting a meal
    function setupDeleteModal() {
        const deleteButtons = document.querySelectorAll('.btn-delete-meal');
        if (!deleteButtons.length) return;

        // When delete button clicked, update hidden fields and labels
        deleteButtons.forEach(button => {
            button.addEventListener('click', () => {
                document.getElementById('deleteMealId').value = button.dataset.mealId;
                document.getElementById('deleteMealName').textContent = button.dataset.mealName;
            });
        });
    }

    // Prevents adding duplicate meal names by validating against existing ones
    function setupDuplicateMealNameCheck() {
        // Find the meal name field on add or edit forms
        const mealNameField = document.getElementById('mealName') || document.getElementById('editMealName');
        if (!mealNameField) return; // Not on add/edit meal page

        const formSubmitButton = mealNameField.closest('form').querySelector('button[type="submit"]');

        let debounceTimeout;
        let lastCheckedName = '';

        // Checks the meal name against existing ones as user types
        mealNameField.addEventListener('input', function () {
            const mealName = this.value.trim();

            // Remove any existing error message
            const errorElement = document.getElementById('mealNameError');
            if (errorElement) {
                errorElement.remove();
            }

            // Reset button state when field is modified
            if (formSubmitButton) {
                formSubmitButton.disabled = false;
            }

            // Skip validation for very short names
            if (mealName.length < 3) {
                return;
            }

            // Don't recheck the same name to reduce API calls
            if (mealName === lastCheckedName) {
                return;
            }

            // Debounce the API call to avoid excessive requests
            clearTimeout(debounceTimeout);
            debounceTimeout = setTimeout(function () {
                lastCheckedName = mealName;

                // Get the current meal ID for edit mode (to exclude current meal)
                const currentMealId = document.querySelector('input[name="MealID"]')?.value || '0';

                // Make API call to check for duplicate name
                fetch(`/MealManagement/CheckMealNameExists?mealName=${encodeURIComponent(mealName)}&currentMealId=${currentMealId}`)
                    .then(response => response.json())
                    .then(exists => {
                        if (exists) {
                            // Create and show Arabic error message
                            const errorMsg = document.createElement('div');
                            errorMsg.id = 'mealNameError';
                            errorMsg.className = 'text-danger mt-1';
                            errorMsg.textContent = 'توجد وجبة بنفس الاسم بالفعل. الرجاء اختيار اسم آخر.';
                            mealNameField.parentNode.appendChild(errorMsg);

                            // Disable submit to prevent adding duplicate
                            if (formSubmitButton) {
                                formSubmitButton.disabled = true;
                            }
                        }
                    })
                    .catch(error => {
                        console.error('Error checking meal name:', error);
                    });
            }, 500); // Wait half second after typing stops
        });
    }

    // Start everything once the page is loaded
    setupCalorieCalculation('add');
    setupCalorieCalculation('edit');
    setupMealFiltering();
    setupDeleteModal();
});
