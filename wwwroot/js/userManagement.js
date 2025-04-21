// Setup the delete user modal
document.addEventListener('click', e => {
    const button = e.target.closest('.btn-delete-user');
    if (!button) return;

    document.getElementById('deleteUserId').value = button.dataset.userId;
    document.getElementById('deleteUserName').textContent = button.dataset.userName;
});
