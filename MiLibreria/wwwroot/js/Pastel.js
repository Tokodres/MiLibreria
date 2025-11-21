$(document).ready(function () {
    const urlBase = window.location.origin;

    $.ajax({
        type: "GET",
        contentType: "application/json",
        url: `${urlBase}/Graficas/DataPastel`,
        error: function () {
            alert("Ocurrió un error al consultar los datos.");
        },
        success: function (data) {
            GraficaPastel(data);
        }
    });
});

function GraficaPastel(data) {
    Highcharts.chart('pastel', {
        chart: {
            plotBackgroundColor: null,
            plotShadow: false,
            type: 'pie'
        },
        title: {
            text: 'Top de Libros Más Vendidos',
            align: 'center'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.1f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.y} unidades'
                },
                showInLegend: true
            }
        },
        series: [{
            name: 'Libros',
            colorByPoint: true,
            data: data
        }]
    });
}
