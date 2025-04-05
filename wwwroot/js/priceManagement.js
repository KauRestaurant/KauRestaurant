$(document).ready(function() {
    // Form validation
    $('form').on('submit', function(e) {
        let isValid = true;
        $('input[type="number"]').each(function() {
            const val = parseFloat($(this).val());
            if (isNaN(val) || val <= 0) {
                alert('يرجى إدخال أسعار صحيحة أكبر من صفر');
                isValid = false;
                return false;
            }
        });
        
        // Only update timestamps if form is valid and about to submit
        if (isValid) {
            updateTimestamps();
        }
        
        return isValid;
    });

    // Format numbers in input fields to 2 decimal places
    $('input[type="number"]').on('change', function() {
        const val = parseFloat($(this).val());
        if (!isNaN(val)) {
            $(this).val(val.toFixed(2));
        }
    });
    
    // Function to update timestamps
    function updateTimestamps() {
        const now = new Date();
        const formattedDate = now.toLocaleString('ar-SA', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
        
        // Safely update elements if they exist
        const timestampElements = ['breakfastLastUpdate', 'lunchLastUpdate', 'dinnerLastUpdate'];
        timestampElements.forEach(id => {
            const element = document.getElementById(id);
            if (element) {
                element.textContent = formattedDate;
            }
        });
    }
});
