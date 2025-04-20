function adjustQuantity(id, change) {
    const input = document.getElementById(id);
    input.value = Math.max(0, parseInt(input.value || 0) + change);
    calculateTotals();
}

function calculateTotals() {
    const breakfastQty = parseInt(document.getElementById('breakfastQty').value) || 0;
    const lunchQty = parseInt(document.getElementById('lunchQty').value) || 0;

    const breakfastPrice = parseFloat(document.getElementById('breakfastPrice').value);
    const lunchPrice = parseFloat(document.getElementById('lunchPrice').value);

    const total = (breakfastQty * breakfastPrice) + (lunchQty * lunchPrice);
    document.getElementById('grandTotal').textContent = `${total.toFixed(2)} ريال`;
}

// Function to show inline messages
function showInlineMessage(message, type) {
    const alertContainer = document.getElementById('alertContainer');
    if (!alertContainer) return;

    // Clear existing alerts
    alertContainer.innerHTML = '';

    // Create the alert
    const alertHTML = `
        <div class="alert alert-${type} alert-dismissible fade show mb-4" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;

    // Add the alert to the container
    alertContainer.innerHTML = alertHTML;

    // Scroll to top to make the message visible
    window.scrollTo(0, 0);
}

function purchaseTickets() {
    const breakfastQty = parseInt(document.getElementById('breakfastQty').value) || 0;
    const lunchQty = parseInt(document.getElementById('lunchQty').value) || 0;

    // Check if any tickets are selected
    if (breakfastQty === 0 && lunchQty === 0) {
        showInlineMessage('الرجاء اختيار عدد التذاكر أولا', 'danger');
        return;
    }

    // Disable button to prevent multiple submissions
    const purchaseButton = document.querySelector('button.btn-success.btn-lg');
    purchaseButton.disabled = true;
    purchaseButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> جاري المعالجة...';

    // Prepare data
    const data = {
        breakfastQty: breakfastQty,
        lunchQty: lunchQty
    };

    console.log('Sending order data:', data);

    // Send AJAX request to create order
    fetch('/Purchase/CreateOrder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            if (!response.ok) {
                console.error('Server returned:', response.status, response.statusText);
                throw new Error('Network response was not ok: ' + response.status);
            }
            return response.json();
        })
        .then(data => {
            console.log('Order response:', data);
            if (data.success) {
                showInlineMessage('تم شراء التذاكر بنجاح!', 'success');
                setTimeout(() => {
                    window.location.href = 'tickets'; // Redirect to tickets page after a short delay
                }, 1500);
            } else {
                showInlineMessage('حدث خطأ أثناء معالجة الطلب', 'danger');
                purchaseButton.disabled = false;
                purchaseButton.innerHTML = 'شراء';
            }
        })
        .catch(error => {
            console.error('Error:', error);
            showInlineMessage('حدث خطأ في الاتصال. يرجى المحاولة مرة أخرى.', 'danger');
            purchaseButton.disabled = false;
            purchaseButton.innerHTML = 'شراء';
        });
}

// Initialize totals
calculateTotals();
