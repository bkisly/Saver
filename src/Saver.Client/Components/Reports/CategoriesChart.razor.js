function createCategoriesChart(chartId, categoriesList, valuesList) {
    const ctx = document.getElementById(chartId);

    ctx.chartInstance = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: categoriesList,
            datasets: [{
                data: valuesList,
                borderWidth: 3
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            },
            plugins: {
                legend: {
                    display: false
                }
            }
        }
    });
}

function updateCategoriesChart(chartId, categoriesList, valuesList) {
    const chart = document.getElementById(chartId).chartInstance;
    chart.data.labels = categoriesList;
    chart.data.datasets[0].data = valuesList;
    chart.update();
}
