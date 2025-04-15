function prepareDeleteReview(reviewId) {
    document.getElementById('deleteReviewId').value = reviewId;
    var modal = new bootstrap.Modal(document.getElementById('deleteReviewModal'));
    modal.show();
}