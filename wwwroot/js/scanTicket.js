document.addEventListener('DOMContentLoaded', function () {
    // Initial setup of the Html5Qrcode object for scanning
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

    // Set up event listeners on buttons that restart scanning
    scanAnotherBtn?.addEventListener('click', () => {
        // Switch UI back to scanner tab and reinitiate scanning
        scannerTab.click();
        startScanning();
    });

    tryAgainBtn?.addEventListener('click', () => {
        // Same logic for the "Try Again" button
        scannerTab.click();
        startScanning();
    });

    // Called when QR code is successfully decoded
    function onScanSuccess(decodedText, decodedResult) {
        // Play a sound effect to confirm scan
        beep();

        // Stop reading from the camera
        html5QrCode.stop();
        updateButtonState(false);

        // Switch the UI to show results
        resultsTab.click();

        // Hide existing statuses before showing new results
        scanResult.classList.remove('d-none');
        resultSuccess.classList.add('d-none');
        resultError.classList.add('d-none');

        // Use the scanned data to validate the ticket
        validateTicket(decodedText);
    }

    // Called on each scan failure (we handle success updates, so do nothing here)
    function onScanFailure(error) {
        // We skip UI changes on each failure to avoid spam
    }

    // Controls the start/stop button states
    function updateButtonState(isScanning) {
        if (isScanning) {
            startButton.disabled = true;
            stopButton.disabled = false;
        } else {
            startButton.disabled = false;
            stopButton.disabled = true;
        }
    }

    // Plays a short beep when a valid QR code is found
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

    // Function to start scanning from the selected camera
    function startScanning() {
        // Reset the results section before scanning again
        scanResult.classList.add('d-none');
        resultSuccess.classList.add('d-none');
        resultError.classList.add('d-none');

        // Attempt to list available cameras
        Html5Qrcode.getCameras()
            .then(devices => {
                if (devices && devices.length) {
                    // Prefer the back camera if multiple exist, else default to first
                    const cameraId = devices.length > 1 ? devices[1].id : devices[0].id;

                    // Start scanning with chosen camera
                    html5QrCode.start(
                        { deviceId: cameraId },
                        {
                            fps: 10,
                            qrbox: { width: 250, height: 250 }
                        },
                        onScanSuccess,
                        onScanFailure
                    ).then(() => {
                        updateButtonState(true);
                    }).catch(err => {
                        // If the specific camera fails, try the environment facing mode
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
                            // If that also fails, notify the user
                            alert(`خطأ في تشغيل الكاميرا: ${err}`);
                        });
                    });
                } else {
                    alert("لم يتم العثور على كاميرات متاحة");
                }
            })
            .catch(err => {
                // If unable to access cameras at all, display error
                alert("حدث خطأ في الوصول إلى الكاميرات");
                console.error("Error accessing cameras:", err);
            });
    }

    // Start scanning when the user clicks "Start"
    startButton.addEventListener('click', startScanning);

    // Stop scanning when the user clicks "Stop"
    stopButton.addEventListener('click', () => {
        html5QrCode.stop().then(() => {
            updateButtonState(false);
        });
    });

    // Sends the scanned data to the server for validation
    function validateTicket(qrData) {
        console.log("Validating QR data:", qrData);

        // Attempt to retrieve the anti-forgery token
        const token = document.querySelector('input[name="__RequestVerificationToken"]')?.value;
        if (!token) {
            console.error("CSRF token not found");
        }

        // Use a globally configured URL, or default if not found
        const validateUrl = window.scanTicketConfig?.validateUrl || '/Admin/ScanTicket/ValidateTicket';

        // Make the POST request with scanned data
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
                    // If server responded with an error, read the body for more info
                    return response.text().then(text => {
                        console.error("Error response body:", text);
                        throw new Error(`Server responded with ${response.status}`);
                    });
                }
                return response.json();
            })
            .then(data => {
                console.log("Server response data:", data);
                // Hide the initial result placeholder
                scanResult.classList.add('d-none');

                if (data.success) {
                    // On success, show user data about the ticket
                    resultSuccess.classList.remove('d-none');
                    document.getElementById('ticketId').textContent = data.ticketDetails.ticketId;
                    document.getElementById('mealType').textContent = data.ticketDetails.mealType;
                    document.getElementById('userName').textContent = data.ticketDetails.userName;
                } else {
                    // On failure, show an error message
                    resultError.classList.remove('d-none');
                    errorMessage.textContent = data.message;
                }
            })
            .catch(error => {
                // If an exception occurred, show an error message
                console.error('Error validating ticket:', error);
                scanResult.classList.add('d-none');
                resultError.classList.remove('d-none');
                errorMessage.textContent = "حدث خطأ في الاتصال بالخادم";
            });
    }
});
