// BusBuddy Dashboard JavaScript functions

// Cache for Chart instances
const chartInstances = {};

// Renders a pie chart with the provided data
function renderPieChart(canvasId, labels, data, colors) {
    // Check if the canvas element exists
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error(`Canvas element with id '${canvasId}' not found`);
        return;
    }

    // Destroy existing chart if it exists
    if (chartInstances[canvasId]) {
        chartInstances[canvasId].destroy();
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
    chartInstances[canvasId] = new Chart(ctx, {
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
    
    return chartInstances[canvasId];
}

// Function to refresh dashboard data
function refreshDashboardData() {
    if (typeof DotNet !== 'undefined') {
        DotNet.invokeMethodAsync('BusBuddy', 'RefreshDashboardDataAsync');
    }
}

// Renders a gauge chart for performance metrics
function renderGaugeChart(canvasId, value, options = {}) {
    // Check if the canvas element exists
    const canvas = document.getElementById(canvasId);
    if (!canvas) {
        console.error(`Canvas element with id '${canvasId}' not found`);
        return;
    }
    
    // Destroy existing chart if it exists
    if (chartInstances[canvasId]) {
        chartInstances[canvasId].destroy();
    }

    const ctx = canvas.getContext('2d');
    
    // Default options
    const defaultOptions = {
        min: 0,
        max: 100,
        lowThreshold: 60,
        highThreshold: 90,
        title: 'Performance',
        colors: {
            low: '#FF6384',    // Red
            medium: '#FFCE56', // Yellow
            high: '#4BC0C0',   // Teal
        }
    };
    
    // Merge provided options with defaults
    const chartOptions = { ...defaultOptions, ...options };
    
    // Determine color based on value and thresholds
    const getColor = (value) => {
        if (value < chartOptions.lowThreshold) return chartOptions.colors.low;
        if (value < chartOptions.highThreshold) return chartOptions.colors.medium;
        return chartOptions.colors.high;
    };
    
    // Create the gauge chart (using doughnut type)
    chartInstances[canvasId] = new Chart(ctx, {
        type: 'doughnut',
        data: {
            datasets: [{
                data: [value, chartOptions.max - value],
                backgroundColor: [
                    getColor(value),
                    '#EEEEEE' // Light gray for remaining portion
                ],
                borderWidth: 0,
                circumference: 180,
                rotation: 270
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            cutout: '75%',
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    enabled: false
                }
            }
        },
        plugins: [{
            // Add value display in center of gauge
            id: 'gaugeText',
            afterDraw: (chart) => {
                const { width, height } = chart;
                const ctx = chart.ctx;
                
                ctx.save();
                ctx.textAlign = 'center';
                
                // Draw value
                ctx.font = 'bold 20px Arial';
                ctx.fillStyle = getColor(value);
                ctx.fillText(
                    `${value}%`,
                    width / 2,
                    height - height / 4
                );
                
                // Draw title
                ctx.font = '14px Arial';
                ctx.fillStyle = '#666666';
                ctx.fillText(
                    chartOptions.title,
                    width / 2,
                    height - height / 8
                );
                
                ctx.restore();
            }
        }]
    });
    
    return chartInstances[canvasId];
}
