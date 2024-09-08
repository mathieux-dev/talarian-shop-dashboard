let graficoInadimplentes = null;
let graficoRendimentos = null;

document.addEventListener('DOMContentLoaded', function () {
    carregarMetricas();
    carregarGraficos();
});

function carregarMetricas() {
    const ano = document.querySelector('#filtroAno').value;
    const trimestre = document.querySelector('#filtroTrimestre').value;
    const mes = document.querySelector('#filtroMes').value;

    fetch(`/api/dashboard/metricas?ano=${ano}&trimestre=${trimestre}&mes=${mes}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Erro ao carregar métricas');
            }
            return response.json();
        })
        .then(dados => {
            const totalNotasEmitidas = dados.totalNotasEmitidas || 0;
            const totalSemCobranca = dados.totalSemCobranca || 0;
            const totalInadimplente = dados.totalInadimplente || 0;
            const totalParaVencer = dados.totalParaVencer || 0;
            const totalPago = dados.totalPago || 0;

            const totalNotasEmitidasElem = document.querySelector('#totalNotasEmitidas');
            const totalSemCobrancaElem = document.querySelector('#totalSemCobranca');
            const totalInadimplenteElem = document.querySelector('#totalInadimplente');
            const totalParaVencerElem = document.querySelector('#totalParaVencer');
            const totalPagoElem = document.querySelector('#totalPago');

            if (totalNotasEmitidasElem) totalNotasEmitidasElem.textContent = `R$ ${totalNotasEmitidas.toFixed(2)}`;
            if (totalSemCobrancaElem) totalSemCobrancaElem.textContent = `R$ ${totalSemCobranca.toFixed(2)}`;
            if (totalInadimplenteElem) totalInadimplenteElem.textContent = `R$ ${totalInadimplente.toFixed(2)}`;
            if (totalParaVencerElem) totalParaVencerElem.textContent = `R$ ${totalParaVencer.toFixed(2)}`;
            if (totalPagoElem) totalPagoElem.textContent = `R$ ${totalPago.toFixed(2)}`;
        })
        .catch(error => console.error('Erro ao carregar métricas:', error));
}


function carregarGraficos() {
    const ano = document.querySelector('#filtroAno').value;
    const trimestre = document.querySelector('#filtroTrimestre').value;
    const mes = document.querySelector('#filtroMes').value;

    fetch(`/api/dashboard/grafico?ano=${ano}&trimestre=${trimestre}&mes=${mes}`)
        .then(response => {
            if (!response.ok) {
                throw new Error('Erro ao carregar gráficos');
            }
            return response.json();
        })
        .then(dados => {
            const inadimplenciaMensal = dados.inadimplenciaMensal || [];
            const receitaMensal = dados.receitaMensal || [];

            const ctxInadimplentes = document.getElementById('graficoInadimplentes').getContext('2d');
            const ctxRendimentos = document.getElementById('graficoRendimentos').getContext('2d');

            if (graficoInadimplentes) {
                graficoInadimplentes.destroy();
            }
            if (graficoRendimentos) {
                graficoRendimentos.destroy();
            }

            graficoInadimplentes = new Chart(ctxInadimplentes, {
                type: 'bar',
                data: {
                    labels: inadimplenciaMensal.map(g => g.mes),
                    datasets: [{
                        label: 'Inadimplentes',
                        data: inadimplenciaMensal.map(g => g.valor),
                        backgroundColor: 'rgba(255, 99, 132, 1)',
                        borderColor: 'rgba(255, 99, 132, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });

            graficoRendimentos = new Chart(ctxRendimentos, {
                type: 'bar',
                data: {
                    labels: receitaMensal.map(g => g.mes),
                    datasets: [{
                        label: 'Rendimentos Recebidos',
                        data: receitaMensal.map(g => g.valor),
                        backgroundColor: 'rgba(13, 69, 35, 1)',
                        borderColor: 'rgba(13, 69, 35, 1)',
                        borderWidth: 1
                    }]
                },
                options: {
                    responsive: true,
                    scales: {
                        y: {
                            beginAtZero: true
                        }
                    }
                }
            });
        })
        .catch(error => console.error('Erro ao carregar gráficos:', error));
}

window.addEventListener('unload', function () {
    if (graficoInadimplentes) {
        graficoInadimplentes.destroy();
    }
    if (graficoRendimentos) {
        graficoRendimentos.destroy();
    }
});

document.querySelector('#filtroAno').addEventListener('change', function () {
    carregarMetricas();
    carregarGraficos();
});
document.querySelector('#filtroTrimestre').addEventListener('change', function () {
    carregarMetricas();
    carregarGraficos();
});
document.querySelector('#filtroMes').addEventListener('change', function () {
    carregarMetricas();
    carregarGraficos();
});
