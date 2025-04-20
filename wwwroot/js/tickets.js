// Fix the URL generation - can't use Razor helpers in external JS files
function showQrCode(ticketId, mealType) {
    if (!ticketId) {
        alert("No ticket available.");
        return;
    }

    // Update the ticket ID display
    document.getElementById('ticketIdDisplay').textContent = 'تذكرة #' + ticketId;

    // Set the QR code image source with ticket data
    const qrCodeImage = document.getElementById('qrCodeImage');

    // Generate the QR code URL - hardcode the URL since we can't use Razor helpers
    const qrUrl = '/QrCode/GenerateQrCode' +
        '?ticketId=' + ticketId +
        '&mealType=' + encodeURIComponent(mealType);

    // Set the image source to load the QR code
    qrCodeImage.src = qrUrl;

    // Display meal type in the modal
    const mealTypeElement = document.getElementById('mealTypeDisplay');
    if (mealTypeElement) {
        mealTypeElement.textContent = mealType;
    }
}

// Wait for the DOM to be ready before setting up event handlers
document.addEventListener('DOMContentLoaded', function () {
    // Add a close handler to the modal to clear the QR code when closed
    const qrModal = document.getElementById('qrModal');
    if (qrModal) {
        qrModal.addEventListener('hidden.bs.modal', function () {
            document.getElementById('qrCodeImage').src = '';
        });
    }
});
