function getParameterFromUrl(parameter) {
    var url_string = window.location.href;
    var url = new URL(url_string);
    return url.searchParams.get(parameter);
}
    


function drawLineChart(contronllerName) {
    var id = $('#datefilter').data('id');

    var startDate = new Date();
    startDate.setFullYear(startDate.getFullYear() - 1);
    var endDate = new Date();

    var startPara = getParameterFromUrl('start');
    if (startPara != null) {
        startDate = new Date(startPara);
    }
    var endPara = getParameterFromUrl('end');
    if (endPara != null) {
        endDate = new Date(endPara);
    }
    $('input[name="datefilter"]').daterangepicker({
        autoUpdateInput: true,
        startDate: startDate,
        endDate: endDate,
        locale: {
            cancelLabel: 'Clear'
        },
        ranges: {
            'Today': [moment(), moment()],
            'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
            'Last 7 Days': [moment().subtract(6, 'days'), moment()],
            'Last 30 Days': [moment().subtract(29, 'days'), moment()],
            'This Month': [moment().startOf('month'), moment().endOf('month')],
            'Last Month': [
                moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')
            ]
        }
    });

    $('input[name="datefilter"]').on('apply.daterangepicker',
        function (ev, picker) {
            var start = picker.startDate.format('YYYY-MM-DD');
            var end = picker.endDate.format('YYYY-MM-DD');
            window.location.href = window.location.href.split('?')[0] + '?start=' + start + '&end=' + end;
        });

    $.ajax({
        url: `/${contronllerName}/GetChartData?id=${id}&start=${startPara}&end=${endPara}`,
        type: 'GET',
        success: function (responseData) {
            google.charts.load('current', { 'packages': ['line', 'corechart'] });
            google.charts.setOnLoadCallback(drawChart);

            function drawChart() {
                var chartDiv = document.getElementById('chart_div');

                var data = new google.visualization.DataTable();
                data.addColumn('date', 'Month');
                data.addColumn('number', "Money");
                data.addColumn('number', "Push-up");
                var total = 0, totalMoney = 0, totalPushUp = 0;
                for (var i = 0; i < responseData.length; i++) {
                    data.addRow([new Date(responseData[i].Date), responseData[i].Money, responseData[i].PushUp]);
                    total += responseData[i].Count;
                    totalMoney += responseData[i].Money;
                    totalPushUp += responseData[i].PushUp;
                }
                $('#late-counter').html(total);
                totalMoney = totalMoney.toLocaleString('it-IT', { style: 'currency', currency: 'VND' });
                $('#total-money').html(totalMoney);
                $('#total-pushup').html(totalPushUp);

                var classicOptions = {
                    //title: 'Thống kê số tiền (push-up) đi học muộn của sinh viên',
                    series: {
                        0: { targetAxisIndex: 0 },
                        1: { targetAxisIndex: 1 }
                    },
                    vAxes: {
                        0: { title: 'Money (VND)' },
                        1: { title: 'Push-up (Times)' }
                    },
                    hAxis: {
                    },
                    vAxis: {
                    }
                };
                function drawClassicChart() {
                    var classicChart = new google.visualization.LineChart(chartDiv);
                    classicChart.draw(data, classicOptions);
                }
                drawClassicChart();
            }
        }
    });

}



