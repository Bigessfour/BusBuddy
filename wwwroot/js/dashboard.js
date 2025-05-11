// BusBuddy Dashboard JavaScript functions

// Renders a pie chart with the provided data
function renderPieChart(canvasId, labels, data, colors) {
    // Check if the canvas element exists
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error(`Canvas element with id '${canvasId}' not found`);
        return;
    }

    const ctx = canvas.getContext('2d');
    
    // Default colors if none provided
    const chartColors = colors || [
        '#36A2EB',  // Blue
        '#FF6384',  // Red
        '#FFCE56',  // Yellow
        '#4BC0C0',  // Teal
        '#9966FF',  // Purple
        '#FF9F40'   // Orange
    ];
    
    // Create the chart
    new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: chartColors,
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right'
                },
                tooltip: {
                    callbacks: {
                        label: function(context) {
                            const label = context.label || '';
                            const value = context.raw || 0;
                            const total = context.chart.data.datasets[0].data.reduce((a, b) => a + b, 0);
                            const percentage = Math.round((value / total) * 100);
                            return `${label}: ${value} (${percentage}%)`;
                        }
                    }
                }
            }
        }
    });
}

// Function to refresh dashboard data
function refreshDashboardData() {
    if (typeof DotNet !== 'undefined') {
        DotNet.invokeMethodAsync('BusBuddy', 'RefreshDashboardDataAsync');
    }
}
