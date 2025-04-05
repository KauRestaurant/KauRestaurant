$(document).ready(function() {
    // Update meal nutritional information when dropdown selection changes
    $('.meal-dropdown').change(function() {
        const menuId = $(this).data('menu-id');
        const category = $(this).data('category');
        const type = $(this).data('type');
        const index = $(this).data('index');
        const selectedOption = $(this).find('option:selected');
        const calories = selectedOption.data('calories');
        const infoElement = $(`#info-${menuId}-${category}-${type}-${index}`);

        if (selectedOption.val() !== '0' && calories) {
            infoElement.html(`<span class="badge bg-light text-dark">${calories} سعرة حرارية</span>`);
        } else {
            infoElement.empty();
        }
    });
});