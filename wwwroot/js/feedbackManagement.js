document.addEventListener('DOMContentLoaded', function() {
    const rows = document.querySelectorAll('.clickable');
    rows.forEach(row => {
        row.addEventListener('mouseenter', () => row.classList.add('table-active'));
        row.addEventListener('mouseleave', () => row.classList.remove('table-active'));
    });
});