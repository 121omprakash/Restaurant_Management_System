document.addEventListener("DOMContentLoaded", function () {
    // 1. LIVE TIME-CLOCK SYSTEM
    function updateLiveClock() {
        const clockElement = document.getElementById('liveClockDisplay');
        if (!clockElement) return;

        const now = new Date();
        const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
        const dateString = now.toLocaleDateString('en-US', options);
        const timeString = now.toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', second: '2-digit' });

        clockElement.innerHTML = `<i class="fa-regular fa-calendar-days me-2 text-warning"></i>${dateString} | Current Time: ${timeString}`;
    }

    if (document.getElementById('liveClockDisplay')) {
        setInterval(updateLiveClock, 1000);
        updateLiveClock();
    }

    // 2. ADMIN SYSTEM LOAD LINE CHART
    const loadCanvas = document.getElementById('systemLoadChart');
    if (loadCanvas) {
        const ctxLine = loadCanvas.getContext('2d');
        new Chart(ctxLine, {
            type: 'line',
            data: {
                labels: ['08 AM', '10 AM', '12 PM', '02 PM', '04 PM', '06 PM', '08 PM', '10 PM'],
                datasets: [{
                    label: 'Database Server Requests',
                    data: [150, 310, 920, 540, 420, 1100, 1420, 500],
                    borderColor: '#3b82f6',
                    backgroundColor: 'rgba(59, 130, 246, 0.05)',
                    borderWidth: 3,
                    fill: true,
                    tension: 0.3,
                    pointRadius: 4,
                    pointBackgroundColor: '#3b82f6'
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { display: false } },
                animation: {
                    duration: 800,
                    easing: 'easeOutQuart'
                },
                scales: {
                    y: { beginAtZero: true }
                }
            }
        });
    }

    // 3. ADMIN SYSTEM HEALTH DONUT CHART
    const healthCanvas = document.getElementById('systemHealthChart');
    if (healthCanvas) {
        const ctxPie = healthCanvas.getContext('2d');
        new Chart(ctxPie, {
            type: 'doughnut',
            data: {
                labels: ['Healthy Operations', 'System Warnings', 'Critical Exceptions'],
                datasets: [{
                    data: [94, 5, 1],
                    backgroundColor: ['#10b981', '#f59e0b', '#ef4444'],
                    borderWidth: 0
                }]
            },
            options: {
                responsive: true,
                maintainAspectRatio: false,
                plugins: { legend: { position: 'bottom' } },
                animation: {
                    animateScale: true,
                    animateRotate: true,
                    duration: 800,
                    easing: 'easeOutQuart'
                }
            }
        });
    }
});