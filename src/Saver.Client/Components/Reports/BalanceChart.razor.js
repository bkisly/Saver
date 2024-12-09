function createChart(datesList, valuesList) {
    const ctx = document.getElementById('balance-chart');

    new Chart(ctx, {
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
