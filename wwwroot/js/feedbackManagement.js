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
});
