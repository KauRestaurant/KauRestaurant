$(document).ready(function () {
    // Initialize Bootstrap tooltips
    try {
        var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
        tooltipTriggerList.forEach(function (tooltipTriggerEl) {
            new bootstrap.Tooltip(tooltipTriggerEl);
        });
    } catch (e) {
        console.error("Error initializing tooltips:", e);
    }

    // Password strength indicator function
    function updatePasswordStrength(input) {
        var password = $(input).val();
        var strength = 0;

        if (password.length >= 8) strength++;
        if (password.match(/[a-z]/)) strength++;
        if (password.match(/[A-Z]/)) strength++;
        if (password.match(/[0-9]/)) strength++;
        if (password.match(/[^a-zA-Z0-9]/)) strength++;

        var strengthClass = ['danger', 'warning', 'warning', 'success', 'success'][strength - 1] || 'danger';
        var strengthText = ['ضعيف جداً', 'ضعيف', 'متوسط', 'قوي', 'قوي جداً'][strength - 1] || 'ضعيف جداً';

        // Update password strength indicator
        var indicator = $(input).closest('.input-group').siblings('.password-strength');

        if (password.length > 0) {
            indicator.html(`<div class="progress" style="height: 5px;">
                <div class="progress-bar bg-${strengthClass}" style="width: ${strength * 20}%"></div>
            </div>
            <small class="text-${strengthClass} mt-1 d-block">قوة كلمة المرور: ${strengthText}</small>`);
        } else {
            indicator.empty();
        }
    }

    // Add password strength indicator for create user form
    $('input[name="Password"]').on('input', function () {
        updatePasswordStrength(this);
    });

    // Role dropdown change handler - visual feedback
    $('.role-dropdown').on('change', function () {
        var originalValue = $(this).attr('data-original-value');
        var currentValue = $(this).val();

        if (originalValue !== currentValue) {
            $(this).addClass('border-success');
        } else {
            $(this).removeClass('border-success');
        }
    });

    // Store original values for dropdowns
    $('.role-dropdown').each(function () {
        $(this).attr('data-original-value', $(this).val());
    });

    // Setup the delete user modal
    $(document).on('click', '.btn-delete-user', function () {
        var userId = $(this).data('user-id');
        var userName = $(this).data('user-name');

        $('#deleteUserId').val(userId);
        $('#deleteUserName').text(userName);
    });
});
