function prepareDeleteReview(reviewId) {
    document.getElementById('deleteReviewId').value = reviewId;
    var modal = new bootstrap.Modal(document.getElementById('deleteReviewModal'));
    modal.show();
}

document.addEventListener('DOMContentLoaded', function () {
    // Calculate calories based on macros
    function calculateCalories(protein, carbs, fat) {
        return Math.round((protein * 4) + (carbs * 4) + (fat * 9));
    }

    // Setup auto-calculation of calories on macro input
    function setupCalorieCalculation(formType) {
        const prefix = formType === 'add' ? 'meal' : 'editMeal';
        const proteinInput = document.getElementById(`${prefix}Protein`);

        if (!proteinInput) return; // Not on a page with this form

        const carbsInput = document.getElementById(`${prefix}Carbs`);
        const fatInput = document.getElementById(`${prefix}Fat`);
        const caloriesInput = document.getElementById(`${prefix}Calories`);

        // Keep track of whether the user has manually edited calories
        let caloriesManuallyEdited = false;

        caloriesInput.addEventListener('input', () => {
            caloriesManuallyEdited = true;
        });

        const updateCalories = () => {
            // Only update calories automatically if user hasn't manually edited them
            if (!caloriesManuallyEdited) {
                const protein = parseFloat(proteinInput.value) || 0;
                const carbs = parseFloat(carbsInput.value) || 0;
                const fat = parseFloat(fatInput.value) || 0;
                caloriesInput.value = calculateCalories(protein, carbs, fat);
            }
        };

        // Calculate on macro input changes
        [proteinInput, carbsInput, fatInput].forEach(input =>
            input.addEventListener('input', updateCalories));

        // Initialize with current values if calories field is empty
        if (!caloriesInput.value) {
            updateCalories();
        }
    }

    // Setup meal filtering on the index page
    function setupMealFiltering() {
        const categoryFilter = document.getElementById('categoryFilter');
        if (!categoryFilter) return; // Not on the listing page

        const typeFilter = document.getElementById('typeFilter');
        const searchFilter = document.getElementById('searchFilter');
        const mealItems = document.querySelectorAll('.meal-item');

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

        // Listen for filter changes
        categoryFilter.addEventListener('change', filterMeals);
        typeFilter.addEventListener('change', filterMeals);
        searchFilter.addEventListener('keyup', filterMeals);
    }

    // Setup delete confirmation modal
    function setupDeleteModal() {
        const deleteButtons = document.querySelectorAll('.btn-delete-meal');
        if (!deleteButtons.length) return; // No delete buttons on page

        deleteButtons.forEach(button => {
            button.addEventListener('click', () => {
                document.getElementById('deleteMealId').value = button.dataset.mealId;
                document.getElementById('deleteMealName').textContent = button.dataset.mealName;
            });
        });
    }

    // Initialize all functionality based on what's on the page
    setupCalorieCalculation('add');
    setupCalorieCalculation('edit');
    setupMealFiltering();
    setupDeleteModal();
});
