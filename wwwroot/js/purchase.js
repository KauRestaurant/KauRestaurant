// Adjusts the quantity of either breakfast or lunch tickets
function adjustQuantity(id, change) {
    // Retrieve and parse the current value
    const input = document.getElementById(id);
    // Increase or decrease by 'change', but don't go below zero
    input.value = Math.max(0, parseInt(input.value || 0) + change);
    // Recalculate total cost after each adjustment
    calculateTotals();
}

// Recomputes and displays the total price
function calculateTotals() {
    // Read selected quantities; if blank, assume zero
    const breakfastQty = parseInt(document.getElementById('breakfastQty').value) || 0;
    const lunchQty = parseInt(document.getElementById('lunchQty').value) || 0;

    // Fetch the current ticket prices
    const breakfastPrice = parseFloat(document.getElementById('breakfastPrice').value);
    const lunchPrice = parseFloat(document.getElementById('lunchPrice').value);

    // Sum everything to get the final total
    const total = (breakfastQty * breakfastPrice) + (lunchQty * lunchPrice);
    // Display the formatted total in the designated element
    document.getElementById('grandTotal').textContent = `${total.toFixed(2)} ريال`;
}

// Shows a dynamically generated alert message at the top
function showInlineMessage(message, type) {
    // Target the alert container on the page
    const alertContainer = document.getElementById('alertContainer');
    if (!alertContainer) return;

    // Clear any existing alert content
    alertContainer.innerHTML = '';

    // Build new alert with Bootstrap styling
    const alertHTML = `
        <div class="alert alert-${type} alert-dismissible fade show mb-4" role="alert">
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
        </div>
    `;

    // Inject the alert into the container
    alertContainer.innerHTML = alertHTML;

    // Scroll to top so user definitely sees it
    window.scrollTo(0, 0);
}

// Submits a ticket purchase request
function purchaseTickets() {
    // Collect the user-selected quantities
    const breakfastQty = parseInt(document.getElementById('breakfastQty').value) || 0;
    const lunchQty = parseInt(document.getElementById('lunchQty').value) || 0;

    // Ensure the user has actually selected something
    if (breakfastQty === 0 && lunchQty === 0) {
        showInlineMessage('الرجاء اختيار عدد التذاكر أولا', 'danger');
        return;
    }

    // Temporarily disable the purchase button to prevent double-click
    const purchaseButton = document.querySelector('button.btn-success.btn-lg');
    purchaseButton.disabled = true;
    purchaseButton.innerHTML = '<span class="spinner-border spinner-border-sm" role="status" aria-hidden="true"></span> جاري المعالجة...';

    // Prepare the data to be sent in the request
    const data = {
        breakfastQty: breakfastQty,
        lunchQty: lunchQty
    };

    console.log('Sending order data:', data);

    // Issue a POST request to create the order
    fetch('/Purchase/CreateOrder', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
        },
        body: JSON.stringify(data)
    })
        .then(response => {
            // Check if the server responded successfully
            if (!response.ok) {
                console.error('Server returned:', response.status, response.statusText);
                throw new Error('Network response was not ok: ' + response.status);
            }
            return response.json();
        })
        .then(data => {
            console.log('Order response:', data);
            // If successful, notify the user and redirect
            if (data.success) {
                showInlineMessage('تم شراء التذاكر بنجاح!', 'success');
                setTimeout(() => {
                    window.location.href = 'tickets'; // Go to tickets page
                }, 1500);
            } else {
                // If backend says failure, re-enable purchase
                showInlineMessage('حدث خطأ أثناء معالجة الطلب', 'danger');
                purchaseButton.disabled = false;
                purchaseButton.innerHTML = 'شراء';
            }
        })
        .catch(error => {
            // On any fetch/network error, show an alert
            console.error('Error:', error);
            showInlineMessage('حدث خطأ في الاتصال. يرجى المحاولة مرة أخرى.', 'danger');
            purchaseButton.disabled = false;
            purchaseButton.innerHTML = 'شراء';
        });
}

// Run total calculation immediately on load
calculateTotals();
