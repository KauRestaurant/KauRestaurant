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

    // Add password strength indicator for change password modals
    $('input[name="newPassword"]').on('input', function () {
        updatePasswordStrength(this);
    });

    // Password confirmation validation
    $('#changePasswordForm').on('submit', function (e) {
        var newPassword = $(this).find('input[name="newPassword"]').val();
        var confirmPassword = $(this).find('input[name="confirmPassword"]').val();

        if (newPassword !== confirmPassword) {
            e.preventDefault();
            alert('كلمات المرور غير متطابقة');
        }
    });

    // Edit user button click handler
    $('.btn-edit-user').on('click', function () {
        var userId = $(this).data('user-id');
        var firstName = $(this).data('user-firstname');
        var lastName = $(this).data('user-lastname');
        var email = $(this).data('user-email');
        var role = $(this).data('user-role');

        // Set form values
        $('#editUserId').val(userId);
        $('#editUserFirstName').val(firstName);
        $('#editUserLastName').val(lastName);
        $('#editUserEmail').val(email);
        $('#editUserRole').val(role);
    });

    // Change password button click handler
    $('[data-bs-target="#changePasswordModal"]').on('click', function () {
        var userId = $(this).data('user-id');
        var userName = $(this).data('user-name');

        // Set user ID in the password change form
        $('#passwordUserId').val(userId);
        $('#passwordUserName').text('تغيير كلمة المرور للمستخدم: ' + userName);
    });

    // Setup the delete user modal
    $(document).on('click', '.btn-delete-user', function () {
        var userId = $(this).data('user-id');
        var userName = $(this).data('user-name');

        $('#deleteUserId').val(userId);
        $('#deleteUserName').text(userName);
    });
});
