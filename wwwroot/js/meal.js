// Review form validation
document.addEventListener('DOMContentLoaded', function () {
    const reviewForm = document.getElementById('review-form');
    if (!reviewForm) return;

    const ratingInputs = document.querySelectorAll('input[name="rating"]');
    const commentField = document.querySelector('textarea[name="comment"]');
    const ratingError = document.getElementById('rating-error');
    const commentError = document.getElementById('comment-error');
    const ratingRequired = document.getElementById('rating-required');
    const commentRequired = document.getElementById('comment-required');

    // Form validation
    reviewForm.addEventListener('submit', function (event) {
        let isValid = true;

        // Clear errors when a star is selected
        ratingInputs.forEach(function (input) {
            input.addEventListener('change', function () {
                ratingError.textContent = '';
                ratingRequired.textContent = '';

            });
        });

        // Clear error when comment is entered
        commentField.addEventListener('input', function () {
            if (this.value.trim() !== '') {
                commentError.textContent = '';
                commentRequired.textContent = '';
            }
        });

        // Check if a star rating is selected
        let ratingSelected = false;
        ratingInputs.forEach(function (input) {
            if (input.checked) {
                ratingSelected = true;
            }
        });

        if (!ratingSelected) {
            ratingError.textContent = 'الرجاء تحديد تقييم';
            ratingRequired.textContent = '*';
            isValid = false;
        }

        // Validate comment field
        if (!commentField.value.trim()) {
            commentError.textContent = 'الرجاء كتابة تعليق';
            commentRequired.textContent = '*';
            isValid = false;
        }

        if (!isValid) {
            event.preventDefault();
        }
    });
});
