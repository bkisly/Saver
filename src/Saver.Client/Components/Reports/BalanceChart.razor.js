function createChart(datesList, valuesList) {
    const ctx = document.getElementById('balance-chart');

    ctx.chartInstance = new Chart(ctx, {
        type: 'line',
        data: {
            labels: datesList,
            datasets: [{
                data: valuesList,
                borderWidth: 3
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: false
                }
            }
        }
    });
}

function updateChart(datesList, valuesList) {
    console.log("updagin chart...");
    const chart = document.getElementById('balance-chart').chartInstance;
    console.log(chart);
    chart.data.labels = datesList;
    chart.data.datasets[0].data = valuesList;
    chart.update();
    console.log("updated");
}
