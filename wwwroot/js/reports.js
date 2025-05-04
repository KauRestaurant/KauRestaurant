// Maintain references to each chart, allowing easy update/destroy
let purchasesChart = null;
let redeemedChart = null;
let topRatedChart = null;
let lowestRatedChart = null;

// Generates a vertical gradient for chart fills
function createGradient(ctx, colorStart, colorEnd) {
    const gradient = ctx.createLinearGradient(0, 0, 0, 400);
    gradient.addColorStop(0, colorStart);
    gradient.addColorStop(1, colorEnd);
    return gradient;
}

// Utility function: checks if any non-zero values exist in dataset
function hasData(dataArray) {
    return dataArray && dataArray.some(value => value > 0);
}

// Updates the "ticket purchases" chart data
function updatePurchasesChart(data) {
    const purchasesCanvas = document.getElementById('ticketPurchasesChart');
    const noPurchaseData = document.getElementById('noPurchaseData');

    // Check if we have ticket purchase data
    if (data.purchasedTickets && data.purchasedTickets.length > 0) {
        purchasesCanvas.classList.remove('d-none');
        noPurchaseData.classList.add('d-none');

        // Create or destroy old chart instance
        const purchasesCtx = purchasesCanvas.getContext('2d');
        const purchasesGradient = createGradient(purchasesCtx, 'rgba(76, 175, 80, 0.7)', 'rgba(76, 175, 80, 0.1)');
        if (purchasesChart) {
            purchasesChart.destroy();
        }

        // Instantiate purchases line chart with chart.js
        purchasesChart = new Chart(purchasesCtx, {
            type: 'line',
            data: {
                labels: data.labels,
                datasets: [{
                    label: 'التذاكر المشتراة',
                    data: data.purchasedTickets,
                    fill: true,
                    backgroundColor: purchasesGradient,
                    borderColor: 'rgb(76, 175, 80)',
                    tension: 0.3,
                    // Store the total sales data for use in tooltip
                    totalSales: data.totalSales || Array(data.purchasedTickets.length).fill(0)
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
                    },
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                const labels = [];
                                // Add ticket count
                                labels.push(`التذاكر المشتراة: ${context.raw}`);

                                // Add total sales if available
                                if (context.dataset.totalSales && context.dataset.totalSales[context.dataIndex] !== undefined) {
                                    const totalSale = context.dataset.totalSales[context.dataIndex];
                                    labels.push(`إجمالي المبيعات: ${totalSale.toFixed(2)} ر.س`);
                                }

                                return labels;
                            }
                        }
                    }
                }
            }
        });
    } else {
        // Hide the chart if no data is present
        purchasesCanvas.classList.add('d-none');
        noPurchaseData.classList.remove('d-none');
    }
}

// Updates the "ticket redeemed" chart data
function updateRedeemedChart(data) {
    const redeemedCanvas = document.getElementById('ticketRedeemedChart');
    const noRedeemedData = document.getElementById('noRedeemedData');

    // Check if we have redeemed ticket data
    if (data.redeemedTickets && data.redeemedTickets.length > 0) {
        redeemedCanvas.classList.remove('d-none');
        noRedeemedData.classList.add('d-none');

        const redeemedCtx = redeemedCanvas.getContext('2d');
        const redeemedGradient = createGradient(redeemedCtx, 'rgba(255, 152, 0, 0.7)', 'rgba(255, 152, 0, 0.1)');
        if (redeemedChart) {
            redeemedChart.destroy();
        }

        // Create new line chart to portray redeemed tickets
        redeemedChart = new Chart(redeemedCtx, {
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
        // Hide the chart and show "no data" area
        redeemedCanvas.classList.add('d-none');
        noRedeemedData.classList.remove('d-none');
    }
}

// Shows highest rated meals in a horizontal bar chart
function updateTopRatedChart(data) {
    const topRatedCanvas = document.getElementById('topRatedChart');
    const noTopRatedData = document.getElementById('noTopRatedData');

    // Check if we have data for top-rated meals
    if (data.topRated && data.topRated.length > 0) {
        topRatedCanvas.classList.remove('d-none');
        noTopRatedData.classList.add('d-none');

        // Prepare meal names and ratings
        const mealNames = data.topRated.map(item => item.name);
        const ratings = data.topRated.map(item => item.rating);

        // If there's an existing instance, remove it first
        if (topRatedChart) {
            topRatedChart.destroy();
        }

        // Construct the horizontal bar chart
        topRatedChart = new Chart(topRatedCanvas, {
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
        // Show no data placeholder
        topRatedCanvas.classList.add('d-none');
        noTopRatedData.classList.remove('d-none');
    }
}

// Shows lowest rated meals in a horizontal bar chart
function updateLowestRatedChart(data) {
    const lowestRatedCanvas = document.getElementById('lowestRatedChart');
    const noLowestRatedData = document.getElementById('noLowestRatedData');

    // Check if we have data for lowest-rated meals
    if (data.lowestRated && data.lowestRated.length > 0) {
        lowestRatedCanvas.classList.remove('d-none');
        noLowestRatedData.classList.add('d-none');

        // Prepare meal names and ratings
        const mealNames = data.lowestRated.map(item => item.name);
        const ratings = data.lowestRated.map(item => item.rating);

        // Destroy old chart instance if found
        if (lowestRatedChart) {
            lowestRatedChart.destroy();
        }

        // Build the bar chart
        lowestRatedChart = new Chart(lowestRatedCanvas, {
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
}

// Retrieves data from the server for ticket charts
async function loadTicketCharts() {
    try {
        // Attempt to use a configured URL if provided
        const ticketStatsUrl = window.reportsConfig?.urls?.ticketStats || '/Admin/Reports/GetTicketStatistics';

        // Fetch JSON data
        const response = await fetch(ticketStatsUrl);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("Ticket data received:", data);

        // Populate ticket purchases and redeemed charts
        updatePurchasesChart(data);
        updateRedeemedChart(data);
    } catch (error) {
        // If any error happens, log it and show "no data" placeholders
        console.error('Error loading ticket chart data:', error);
        document.getElementById('ticketPurchasesChart').classList.add('d-none');
        document.getElementById('ticketRedeemedChart').classList.add('d-none');
        document.getElementById('noPurchaseData').classList.remove('d-none');
        document.getElementById('noRedeemedData').classList.remove('d-none');
    }
}

// Retrieves data from the server for meal rating charts
async function loadMealRatingCharts() {
    try {
        // Attempt to use a configured URL if available
        const mealRatingsUrl = window.reportsConfig?.urls?.mealRatings || '/Admin/Reports/GetMealRatings';

        // Fetch JSON data
        const response = await fetch(mealRatingsUrl);
        if (!response.ok) {
            throw new Error(`HTTP error! Status: ${response.status}`);
        }

        const data = await response.json();
        console.log("Meal ratings data received:", data);

        // Populate top-rated and lowest-rated charts
        updateTopRatedChart(data);
        updateLowestRatedChart(data);
    } catch (error) {
        console.error('Error loading meal rating chart data:', error);
        document.getElementById('topRatedChart').classList.add('d-none');
        document.getElementById('lowestRatedChart').classList.add('d-none');
        document.getElementById('noTopRatedData').classList.remove('d-none');
        document.getElementById('noLowestRatedData').classList.remove('d-none');
    }
}

// Provide filtered data requests for ticket charts
function fetchTicketStats(ticketType, chartType) {
    const url = new URL(window.reportsConfig.urls.ticketStats, window.location.origin);
    if (ticketType) {
        url.searchParams.append('ticketType', ticketType);
    }

    // Make network request and parse JSON
    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            // Update either purchases, redeemed, or both
            if (chartType === 'purchases' || chartType === 'both') {
                updatePurchasesChart(data);
            }
            if (chartType === 'redeemed' || chartType === 'both') {
                updateRedeemedChart(data);
            }
        })
        .catch(error => {
            console.error('Error fetching ticket stats:', error);
        });
}

// Provide filtered data requests for meal ratings
function fetchMealRatings(mealType, ratingType) {
    const url = new URL(window.reportsConfig.urls.mealRatings, window.location.origin);
    if (mealType) {
        url.searchParams.append('mealType', mealType);
    }

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            return response.json();
        })
        .then(data => {
            // Update top, lowest, or both charts
            if (ratingType === 'top' || ratingType === 'both') {
                updateTopRatedChart(data);
            }
            if (ratingType === 'lowest' || ratingType === 'both') {
                updateLowestRatedChart(data);
            }
        })
        .catch(error => {
            console.error('Error fetching meal ratings:', error);
        });
}

// Initialize everything once the DOM is fully loaded
document.addEventListener('DOMContentLoaded', function () {
    // Warn if no config is provided in code
    if (typeof window.reportsConfig === 'undefined') {
        console.warn("Reports configuration not found. Using default URLs.");
    }

    // Load the default data for ticket usage and ratings
    loadTicketCharts();
    loadMealRatingCharts();

    // Listen for dropdown changes that call filter-based fetch
    document.getElementById('ticketTypeFilterPurchases')?.addEventListener('change', function () {
        const ticketType = this.value;
        fetchTicketStats(ticketType, 'purchases');
    });

    document.getElementById('ticketTypeFilterRedeemed')?.addEventListener('change', function () {
        const ticketType = this.value;
        fetchTicketStats(ticketType, 'redeemed');
    });

    document.getElementById('mealTypeFilterTopRated')?.addEventListener('change', function () {
        const mealType = this.value;
        fetchMealRatings(mealType, 'top');
    });

    document.getElementById('mealTypeFilterLowestRated')?.addEventListener('change', function () {
        const mealType = this.value;
        fetchMealRatings(mealType, 'lowest');
    });
});
