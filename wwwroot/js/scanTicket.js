document.addEventListener('DOMContentLoaded', function () {
    const html5QrCode = new Html5Qrcode("reader");
    const startButton = document.getElementById("startButton");
    const stopButton = document.getElementById("stopButton");
    const scanResult = document.getElementById("scanResult");
    const resultSuccess = document.getElementById("resultSuccess");
    const resultError = document.getElementById("resultError");
    const errorMessage = document.getElementById("errorMessage");
    const scannerTab = document.getElementById("scanner-tab");
    const resultsTab = document.getElementById("results-tab");
    const scanAnotherBtn = document.getElementById("scanAnotherBtn");
    const tryAgainBtn = document.getElementById("tryAgainBtn");

    // Setup additional event listeners
    scanAnotherBtn?.addEventListener('click', () => {
        scannerTab.click();
        startScanning();
    });

    tryAgainBtn?.addEventListener('click', () => {
        scannerTab.click();
        startScanning();
    });

    // Success callback when QR code is scanned
    function onScanSuccess(decodedText, decodedResult) {
        // Play a beep sound to notify user
        beep();

        // Stop the scanner
        html5QrCode.stop();
        updateButtonState(false);

        // Switch to results tab
        resultsTab.click();

        // Show scanning result
        scanResult.classList.remove('d-none');
        resultSuccess.classList.add('d-none');
        resultError.classList.add('d-none');

        // Validate the ticket
        validateTicket(decodedText);
    }

    // Error callback
    function onScanFailure(error) {
        // We'll handle the UI in the scan success callback
    }

    // Update button state
    function updateButtonState(isScanning) {
        if (isScanning) {
            startButton.disabled = true;
            stopButton.disabled = false;
        } else {
            startButton.disabled = false;
            stopButton.disabled = true;
        }
    }

    // Play a beep sound when QR code is successfully scanned
    function beep() {
        const audioCtx = new (window.AudioContext || window.webkitAudioContext)();
        const oscillator = audioCtx.createOscillator();
        const gainNode = audioCtx.createGain();

        oscillator.connect(gainNode);
        gainNode.connect(audioCtx.destination);

        oscillator.type = 'sine';
        oscillator.frequency.value = 800;
        gainNode.gain.value = 0.5;

        oscillator.start();
        setTimeout(() => {
            oscillator.stop();
        }, 200);
    }

    // Function to start scanning
    function startScanning() {
        // Reset UI
        scanResult.classList.add('d-none');
        resultSuccess.classList.add('d-none');
        resultError.classList.add('d-none');

        // Request camera permissions and start scanning
        Html5Qrcode.getCameras()
            .then(devices => {
                if (devices && devices.length) {
                    // Start with back camera if available, otherwise use the first camera
                    const cameraId = devices.length > 1 ? devices[1].id : devices[0].id;

                    html5QrCode.start(
                        { deviceId: cameraId }, // Use specific camera if multiple are available
                        {
                            fps: 10,
                            qrbox: { width: 250, height: 250 }
                        },
                        onScanSuccess,
                        onScanFailure
                    ).then(() => {
                        updateButtonState(true);
                    }).catch(err => {
                        // If specific camera fails, try with environment mode
                        html5QrCode.start(
                            { facingMode: "environment" },
                            {
                                fps: 10,
                                qrbox: { width: 250, height: 250 }
                            },
                            onScanSuccess,
                            onScanFailure
                        ).then(() => {
                            updateButtonState(true);
                        }).catch(err => {
                            alert(`خطأ في تشغيل الكاميرا: ${err}`);
                        });
                    });
                } else {
                    alert("لم يتم العثور على كاميرات متاحة");
                }
            })
            .catch(err => {
                alert("حدث خطأ في الوصول إلى الكاميرات");
                console.error("Error accessing cameras:", err);
            });
    }

    // Start scanning button event listener
    startButton.addEventListener('click', startScanning);

    // Stop scanning
    stopButton.addEventListener('click', () => {
        html5QrCode.stop().then(() => {
            updateButtonState(false);
        });
    });

    // Validate ticket with the server
    function validateTicket(qrData) {
        console.log("Validating QR data:", qrData);

        // Get the anti-forgery token
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;

        if (!token) {
            console.error("CSRF token not found");
        }

        // Get the URL from the global configuration object
        const validateUrl = window.scanTicketConfig?.validateUrl || '/Admin/ScanTicket/ValidateTicket';
        
        fetch(validateUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token,
                'RequestVerificationToken': token
            },
            body: JSON.stringify({ qrData: qrData })
        })
        .then(response => {
            console.log("Server response status:", response.status);
            if (!response.ok) {
                return response.text().then(text => {
                    console.error("Error response body:", text);
                    throw new Error(`Server responded with ${response.status}`);
                });
            }
            return response.json();
        })
        .then(data => {
            console.log("Server response data:", data);
            scanResult.classList.add('d-none');

            if (data.success) {
                // Show success result
                resultSuccess.classList.remove('d-none');
                document.getElementById('ticketId').textContent = data.ticketDetails.ticketId;
                document.getElementById('mealType').textContent = data.ticketDetails.mealType;
                document.getElementById('userName').textContent = data.ticketDetails.userName;
            } else {
                // Show error result
                resultError.classList.remove('d-none');
                errorMessage.textContent = data.message;
            }
        })
        .catch(error => {
            console.error('Error validating ticket:', error);
            scanResult.classList.add('d-none');
            resultError.classList.remove('d-none');
            errorMessage.textContent = "حدث خطأ في الاتصال بالخادم";
        });
    }
});

