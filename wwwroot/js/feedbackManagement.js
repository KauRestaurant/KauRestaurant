document.addEventListener('DOMContentLoaded', function () {
    // Select all rows that should respond to hover
    const rows = document.querySelectorAll('.clickable');

    // For each clickable row, toggle a highlight class on hover
    rows.forEach(row => {
        // When the mouse enters, add highlight
        row.addEventListener('mouseenter', () => row.classList.add('table-active'));
        // When the mouse leaves, remove highlight
        row.addEventListener('mouseleave', () => row.classList.remove('table-active'));
    });

    // Setup delete modal for feedback deletion
    function setupDeleteModal() {
        const deleteButtons = document.querySelectorAll('.btn-delete-feedback');
        if (!deleteButtons.length) return;
        deleteButtons.forEach(button => {
            button.addEventListener('click', () => {
                // Set the chosen feedback's ID into the hidden field in modal
                document.getElementById('deleteFeedbackId').value = button.dataset.feedbackId;
            });
        });
    }

    setupDeleteModal();
});
