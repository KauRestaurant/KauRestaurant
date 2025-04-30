// Displays a QR code in a modal for a specific ticket
function showQrCode(ticketId, mealType) {
    // If no valid ticket ID is passed, alert the user
    if (!ticketId) {
        alert("No ticket available.");
        return;
    }

    // Update ticket ID text in the modal
    document.getElementById('ticketIdDisplay').textContent = 'تذكرة #' + ticketId;

    // Identify the IMG element for the QR code
    const qrCodeImage = document.getElementById('qrCodeImage');

    // Build the URL that returns the generated QR code image for this ticket
    const qrUrl = '/QrCode/GenerateQrCode' +
        '?ticketId=' + ticketId +
        '&mealType=' + encodeURIComponent(mealType);

    // Assign it to the image's source to trigger loading
    qrCodeImage.src = qrUrl;

    // Display meal type in the modal for clarity
    const mealTypeElement = document.getElementById('mealTypeDisplay');
    if (mealTypeElement) {
        mealTypeElement.textContent = mealType;
    }
}

// Wait for the DOM to be ready before hooking up events
document.addEventListener('DOMContentLoaded', function () {
    // Clean up the QR code image when the modal is closed
    const qrModal = document.getElementById('qrModal');
    if (qrModal) {
        // Use bootstrap's 'hidden.bs.modal' event to reset QR source
        qrModal.addEventListener('hidden.bs.modal', function () {
            document.getElementById('qrCodeImage').src = '';
        });
    }
});
