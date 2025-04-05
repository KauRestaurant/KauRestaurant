$(document).ready(function() {
    // Meal filtering functionality
    $("#categoryFilter, #typeFilter, #searchFilter").on("change keyup", function() {
        var categoryFilter = $("#categoryFilter").val().toLowerCase();
        var typeFilter = $("#typeFilter").val().toLowerCase();
        var searchFilter = $("#searchFilter").val().toLowerCase();

        $(".meal-item").each(function() {
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
    $(".btn-edit-meal").click(function() {
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
    });

    // Delete meal button click
    $(".btn-delete-meal").click(function() {
        var mealId = $(this).data("meal-id");
        var mealName = $(this).data("meal-name");

        // Set the meal ID and name in the delete confirmation modal
        $("#deleteMealId").val(mealId);
        $("#deleteMealName").text(mealName);
    });
});
