// Function to create gradient
function createGradient(ctx, colorStart, colorEnd) {
    const gradient = ctx.createLinearGradient(0, 0, 0, 400);
    gradient.addColorStop(0, colorStart);
    gradient.addColorStop(1, colorEnd);
    return gradient;
}

// Function to check if data array has any non-zero values
function hasData(dataArray) {
    return dataArray && dataArray.some(value => value > 0);
}

// Fetch and render ticket charts
async function loadTicketCharts() {
    try {
        // Get the URL from the global configuration
        const ticketStatsUrl = window.reportsConfig?.urls?.ticketStats || '/Admin/Reports/GetTicketStatistics';
        
        const response = await fetch(ticketStatsUrl);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("Ticket data received:", data);

        // Handle Purchases Chart
        const purchasesCanvas = document.getElementById('ticketPurchasesChart');
        const noPurchaseData = document.getElementById('noPurchaseData');

        if (data.purchasedTickets && data.purchasedTickets.length > 0) {
            purchasesCanvas.classList.remove('d-none');
            noPurchaseData.classList.add('d-none');

            const purchasesCtx = purchasesCanvas.getContext('2d');
            const purchasesGradient = createGradient(purchasesCtx, 'rgba(76, 175, 80, 0.7)', 'rgba(76, 175, 80, 0.1)');

            new Chart(purchasesCtx, {
                type: 'line',
                data: {
                    labels: data.labels,
                    datasets: [{
                        label: 'التذاكر المشتراة',
                        data: data.purchasedTickets,
                        fill: true,
                        backgroundColor: purchasesGradient,
                        borderColor: 'rgb(76, 175, 80)',
                        tension: 0.3
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'عدد التذاكر'
                            }
                        },
                        x: {
                            title: {
                                display: true,
                                text: 'الشهر'
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            position: 'top'
                        }
                    }
                }
            });
        } else {
            purchasesCanvas.classList.add('d-none');
            noPurchaseData.classList.remove('d-none');
        }

        // Handle Redeemed Chart
        const redeemedCanvas = document.getElementById('ticketRedeemedChart');
        const noRedeemedData = document.getElementById('noRedeemedData');

        if (data.redeemedTickets && data.redeemedTickets.length > 0) {
            redeemedCanvas.classList.remove('d-none');
            noRedeemedData.classList.add('d-none');

            const redeemedCtx = redeemedCanvas.getContext('2d');
            const redeemedGradient = createGradient(redeemedCtx, 'rgba(255, 152, 0, 0.7)', 'rgba(255, 152, 0, 0.1)');

            new Chart(redeemedCtx, {
                type: 'line',
                data: {
                    labels: data.labels,
                    datasets: [{
                        label: 'التذاكر المستخدمة',
                        data: data.redeemedTickets,
                        fill: true,
                        backgroundColor: redeemedGradient,
                        borderColor: 'rgb(255, 152, 0)',
                        tension: 0.3
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: 'عدد التذاكر'
                            }
                        },
                        x: {
                            title: {
                                display: true,
                                text: 'الشهر'
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            position: 'top'
                        }
                    }
                }
            });
        } else {
            redeemedCanvas.classList.add('d-none');
            noRedeemedData.classList.remove('d-none');
        }
    } catch (error) {
        console.error('Error loading ticket chart data:', error);
        // Show error message for both charts
        document.getElementById('ticketPurchasesChart').classList.add('d-none');
        document.getElementById('ticketRedeemedChart').classList.add('d-none');
        document.getElementById('noPurchaseData').classList.remove('d-none');
        document.getElementById('noRedeemedData').classList.remove('d-none');
    }
}

// Fetch and render meal rating charts
async function loadMealRatingCharts() {
    try {
        // Get the URL from the global configuration
        const mealRatingsUrl = window.reportsConfig?.urls?.mealRatings || '/Admin/Reports/GetMealRatings';
        
        const response = await fetch(mealRatingsUrl);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("Meal ratings data received:", data);

        // Handle Top Rated Meals Chart
        const topRatedCanvas = document.getElementById('topRatedChart');
        const noTopRatedData = document.getElementById('noTopRatedData');

        if (data.topRated && data.topRated.length > 0) {
            topRatedCanvas.classList.remove('d-none');
            noTopRatedData.classList.add('d-none');

            const mealNames = data.topRated.map(item => item.name);
            const ratings = data.topRated.map(item => item.rating);

            new Chart(topRatedCanvas, {
                type: 'bar',
                data: {
                    labels: mealNames,
                    datasets: [{
                        label: 'التقييم',
                        data: ratings,
                        backgroundColor: [
                            'rgba(76, 175, 80, 0.7)',
                            'rgba(76, 175, 80, 0.65)',
                            'rgba(76, 175, 80, 0.6)',
                            'rgba(76, 175, 80, 0.55)',
                            'rgba(76, 175, 80, 0.5)'
                        ],
                        borderColor: 'rgba(76, 175, 80, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    indexAxis: 'y',
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        x: {
                            beginAtZero: true,
                            max: 5,
                            title: {
                                display: true,
                                text: 'التقييم (من 5)'
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            display: false
                        }
                    }
                }
            });
        } else {
            topRatedCanvas.classList.add('d-none');
            noTopRatedData.classList.remove('d-none');
        }

        // Handle Lowest Rated Meals Chart
        const lowestRatedCanvas = document.getElementById('lowestRatedChart');
        const noLowestRatedData = document.getElementById('noLowestRatedData');

        if (data.lowestRated && data.lowestRated.length > 0) {
            lowestRatedCanvas.classList.remove('d-none');
            noLowestRatedData.classList.add('d-none');

            const mealNames = data.lowestRated.map(item => item.name);
            const ratings = data.lowestRated.map(item => item.rating);

            new Chart(lowestRatedCanvas, {
                type: 'bar',
                data: {
                    labels: mealNames,
                    datasets: [{
                        label: 'التقييم',
                        data: ratings,
                        backgroundColor: [
                            'rgba(244, 67, 54, 0.7)',
                            'rgba(244, 67, 54, 0.65)',
                            'rgba(244, 67, 54, 0.6)',
                            'rgba(244, 67, 54, 0.55)',
                            'rgba(244, 67, 54, 0.5)'
                        ],
                        borderColor: 'rgba(244, 67, 54, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    indexAxis: 'y',
                    responsive: true,
                    maintainAspectRatio: false,
                    scales: {
                        x: {
                            beginAtZero: true,
                            max: 5,
                            title: {
                                display: true,
                                text: 'التقييم (من 5)'
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            display: false
                        }
                    }
                }
            });
        } else {
            lowestRatedCanvas.classList.add('d-none');
            noLowestRatedData.classList.remove('d-none');
        }
    } catch (error) {
        console.error('Error loading meal rating chart data:', error);
        // Show error message for both charts
        document.getElementById('topRatedChart').classList.add('d-none');
        document.getElementById('lowestRatedChart').classList.add('d-none');
        document.getElementById('noTopRatedData').classList.remove('d-none');
        document.getElementById('noLowestRatedData').classList.remove('d-none');
    }
}

// Load all charts when page loads
document.addEventListener('DOMContentLoaded', function() {
    // Check if the configuration exists first
    if (typeof window.reportsConfig === 'undefined') {
        console.warn("Reports configuration not found. Using default URLs.");
    }
    
    loadTicketCharts();
    loadMealRatingCharts();
});
