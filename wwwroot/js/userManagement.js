// Attach a click event to any 'delete user' button
document.addEventListener('click', e => {
    // Find the closest element with the 'btn-delete-user' class
    const button = e.target.closest('.btn-delete-user');
    // If no such button was clicked, do nothing
    if (!button) return;

    // Populate the hidden form fields with the relevant user data
    document.getElementById('deleteUserId').value = button.dataset.userId;
    document.getElementById('deleteUserName').textContent = button.dataset.userName;
});
