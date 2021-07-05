var cvas = document.getElementById('myChart').getContext('2d');
var myChart = new Chart(cvas, {
    type: 'radar',
    data: {
        labels: ['早餐', '午餐', '點心', '晚餐', '宵夜'],
        datasets: [{
            label: '目前攝取 (單位 %): ',
            data: [50, 60, 50, 80,60],
            backgroundColor: [
                'rgba(255, 99, 132, 0.2)'
            ],
            borderColor: [
                'rgba(255, 99, 132, 1)'
            ],
            borderWidth: 1
        },
        {
            label: '即將攝取 (單位 %): ',
            data: [50, 60, 50, 80, 60],
            backgroundColor: [
                'rgba(0, 99, 132, 0)'
            ],
            borderColor: [
                'rgba(0, 99, 132, 1)'
            ],
            borderWidth: 1
        }]
    },
    options: {
        responsive: true,

        scale: {
            min: 0,
            max: 60,

        },
    },



});
