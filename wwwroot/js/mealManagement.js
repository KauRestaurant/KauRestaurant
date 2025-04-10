$(document).ready(function () {
    // Function to calculate calories based on macronutrients
    function calculateCalories(protein, carbs, fat) {
        // Standard caloric values: protein and carbs = 4 calories/gram, fat = 9 calories/gram
        return Math.round((protein * 4) + (carbs * 4) + (fat * 9));
    }

    // Add meal form - Calculate calories when protein, carbs, or fat change
    $('#mealProtein, #mealCarbs, #mealFat').on('input', function () {
        const protein = parseFloat($('#mealProtein').val()) || 0;
        const carbs = parseFloat($('#mealCarbs').val()) || 0;
        const fat = parseFloat($('#mealFat').val()) || 0;

        $('#mealCalories').val(calculateCalories(protein, carbs, fat));
    });

    // Edit meal form - Calculate calories when protein, carbs, or fat change
    $('#editMealProtein, #editMealCarbs, #editMealFat').on('input', function () {
        const protein = parseFloat($('#editMealProtein').val()) || 0;
        const carbs = parseFloat($('#editMealCarbs').val()) || 0;
        const fat = parseFloat($('#editMealFat').val()) || 0;

        $('#editMealCalories').val(calculateCalories(protein, carbs, fat));
    });

    // Calculate initial calories on page load
    if ($('#mealProtein').val() || $('#mealCarbs').val() || $('#mealFat').val()) {
        const protein = parseFloat($('#mealProtein').val()) || 0;
        const carbs = parseFloat($('#mealCarbs').val()) || 0;
        const fat = parseFloat($('#mealFat').val()) || 0;

        $('#mealCalories').val(calculateCalories(protein, carbs, fat));
    }

    // Meal filtering functionality
    $("#categoryFilter, #typeFilter, #searchFilter").on("change keyup", function () {
        var categoryFilter = $("#categoryFilter").val().toLowerCase();
        var typeFilter = $("#typeFilter").val().toLowerCase();
        var searchFilter = $("#searchFilter").val().toLowerCase();

        $(".meal-item").each(function () {
            var category = $(this).data("category").toLowerCase();
            var type = $(this).data("type").toLowerCase();
            var name = $(this).data("name").toLowerCase();

            // Show/hide based on filters
            var matchCategory = categoryFilter === "" || category === categoryFilter;
            var matchType = typeFilter === "" || type === typeFilter;
            var matchSearch = searchFilter === "" || name.includes(searchFilter);

            if (matchCategory && matchType && matchSearch) {
                $(this).show();
            } else {
                $(this).hide();
            }
        });
    });

    // Edit meal button click
    $(".btn-edit-meal").click(function () {
        var mealId = $(this).data("meal-id");
        var mealName = $(this).data("meal-name");
        var mealDescription = $(this).data("meal-description");
        var mealCalories = $(this).data("meal-calories");
        var mealProtein = $(this).data("meal-protein");
        var mealCarbs = $(this).data("meal-carbs");
        var mealFat = $(this).data("meal-fat");
        var mealCategory = $(this).data("meal-category");
        var mealType = $(this).data("meal-type");
        var mealPicture = $(this).data("meal-picture");

        // Set values in the edit form
        $("#editMealId").val(mealId);
        $("#editMealName").val(mealName);
        $("#editMealDescription").val(mealDescription);
        $("#editMealCalories").val(mealCalories);
        $("#editMealProtein").val(mealProtein);
        $("#editMealCarbs").val(mealCarbs);
        $("#editMealFat").val(mealFat);
        $("#editMealCategory").val(mealCategory);
        $("#editMealType").val(mealType);
        $("#currentPicturePath").val(mealPicture);

        // Show the current meal image if available
        if (mealPicture && mealPicture !== "") {
            $("#currentMealImage").attr("src", mealPicture).show();
        } else {
            $("#currentMealImage").hide();
        }

        // Recalculate calories based on protein, carbs, and fat
        setTimeout(function () {
            const protein = parseFloat($('#editMealProtein').val()) || 0;
            const carbs = parseFloat($('#editMealCarbs').val()) || 0;
            const fat = parseFloat($('#editMealFat').val()) || 0;

            $('#editMealCalories').val(calculateCalories(protein, carbs, fat));
        }, 100);
    });

    // Delete meal button click
    $(".btn-delete-meal").click(function () {
        var mealId = $(this).data("meal-id");
        var mealName = $(this).data("meal-name");

        // Set the meal ID and name in the delete confirmation modal
        $("#deleteMealId").val(mealId);
        $("#deleteMealName").text(mealName);
    });
});
