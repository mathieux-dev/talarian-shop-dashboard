const mapaStatus = {
    1: 'Emitida',
    2: 'Cobrança Realizada',
    3: 'Pagamento Atrasado',
    4: 'Pagamento Realizado'
};

function mostrarSwal(mensagem, tipo) {
    Swal.fire({
        icon: tipo,
        title: mensagem,
        confirmButtonText: 'OK'
    });
}

document.querySelector('#editarNotaModal .close').addEventListener('click', function () {
    $('#editarNotaModal').modal('hide');
});

document.querySelector('#adicionarNotaModal .close').addEventListener('click', function () {
    $('#adicionarNotaModal').modal('hide');
});

let paginaAtual = 1;
const tamanhoPagina = 10;

function carregarNotas(pagina = 1) {
    const mesEmissao = document.querySelector('#filtroMesEmissao').value;
    const anoEmissao = document.querySelector('#filtroAnoEmissao').value;
    const status = document.querySelector('#filtroStatus').value;

    const parametros = new URLSearchParams();
    parametros.append('pagina', pagina);
    parametros.append('tamanhoPagina', tamanhoPagina);
    if (mesEmissao) parametros.append('mesEmissao', mesEmissao);
    if (anoEmissao) parametros.append('anoEmissao', anoEmissao);
    if (status) parametros.append('status', status);

    fetch(`/api/notasfiscais/filtradas?${parametros.toString()}`, {
        method: 'GET'
    })
        .then(response => response.json())
        .then(dados => {
            if (!dados.itens) {
                console.error('Erro: Propriedade "itens" indefinida na resposta.');
                return;
            }

            const corpoTabela = document.querySelector('#tabelaNotas tbody');
            const paginacao = document.querySelector('#paginacao');
            corpoTabela.innerHTML = '';
            dados.itens.forEach(nota => {
                const linha = document.createElement('tr');
                linha.innerHTML = `
                <td>${nota.nomePagador}</td>
                <td>${nota.numeroNotaFiscal}</td>
                <td>${nota.dataEmissao}</td>
                <td>${nota.dataCobrança || '-'}</td>
                <td>${nota.dataPagamento || '-'}</td>
                <td>${nota.valor.toFixed(2)}</td>
                <td>${mapaStatus[nota.status]}</td>
                <td>
                    <button class="btn btn-primary-tolarianshop" onclick="editarNota(${nota.id})"><i class="fas fa-pencil-alt"></i></button>
                    <button class="btn btn-danger" onclick="deletarNota(${nota.id})"><i class="fa fa-trash"></i></button>
                </td>
            `;
                corpoTabela.appendChild(linha);
            });

            const totalPaginas = Math.ceil(dados.totalCount / tamanhoPagina);
            paginacao.innerHTML = '';
            for (let i = 1; i <= totalPaginas; i++) {
                const linkPagina = document.createElement('li');
                linkPagina.className = 'page-item';
                linkPagina.innerHTML = `<a class="page-link" href="#" style="color: #132b51" onclick="carregarNotas(${i})">${i}</a>`;
                paginacao.appendChild(linkPagina);
            }
        })
        .catch(error => console.error('Erro ao carregar as notas:', error));
}

function aplicarFiltros() {
    carregarNotas(1);
}

function editarNota(id) {
    fetch(`/api/notasfiscais/${id}`)
        .then(response => response.json())
        .then(nota => {
            document.querySelector('#editarNotaId').value = nota.id;
            document.querySelector('#editarNomePagador').value = nota.nomePagador;
            document.querySelector('#editarNumeroNotaFiscal').value = nota.numeroNotaFiscal;
            document.querySelector('#editarDataEmissao').value = nota.dataEmissao.split('T')[0];
            document.querySelector('#editarDataCobrança').value = nota.dataCobrança ? nota.dataCobrança.split('T')[0] : '';
            document.querySelector('#editarDataPagamento').value = nota.dataPagamento ? nota.dataPagamento.split('T')[0] : '';
            document.querySelector('#editarValor').value = nota.valor;
            document.querySelector('#editarStatus').value = nota.status;
            $('#editarNotaModal').modal('show');
        })
        .catch(error => console.error('Erro ao buscar nota:', error));
}

function deletarNota(id) {
    Swal.fire({
        title: 'Você tem certeza?',
        text: 'Você não poderá reverter esta ação!',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Sim, deletar!',
        cancelButtonText: 'Cancelar'
    }).then((resultado) => {
        if (resultado.isConfirmed) {
            fetch(`/api/notasfiscais/${id}`, {
                method: 'DELETE'
            })
                .then(response => {
                    if (response.ok) {
                        carregarNotas(paginaAtual);
                        Swal.fire(
                            'Deletado!',
                            'A nota foi deletada.',
                            'success'
                        );
                    } else {
                        Swal.fire(
                            'Erro!',
                            'Não foi possível deletar a nota.',
                            'error'
                        );
                    }
                })
                .catch(() => Swal.fire(
                    'Erro!',
                    'Não foi possível deletar a nota.',
                    'error'
                ));
        }
    });
}

document.addEventListener('DOMContentLoaded', () => {
    carregarNotas(paginaAtual);
});

document.querySelector('#formAdicionarNota').addEventListener('submit', function (event) {
    event.preventDefault();
    const nota = {
        nomePagador: document.querySelector('#nomePagador').value,
        numeroNotaFiscal: document.querySelector('#numeroNotaFiscal').value,
        dataEmissao: new Date(document.querySelector('#dataEmissao').value).toISOString(),
        dataCobrança: document.querySelector('#dataCobrança').value ? new Date(document.querySelector('#dataCobrança').value).toISOString() : null,
        dataPagamento: document.querySelector('#dataPagamento').value ? new Date(document.querySelector('#dataPagamento').value).toISOString() : null,
        valor: parseFloat(document.querySelector('#valor').value),
        status: parseInt(document.querySelector('#status').value, 10),
        documentoNota: 'nota.pdf',
        documentoBoleto: 'boleto.pdf'
    };

    fetch('/api/notasfiscais', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(nota)
    })
        .then(response => {
            if (response.ok) {
                $('#adicionarNotaModal').modal('hide');
                carregarNotas(paginaAtual);
                mostrarSwal('Nota adicionada com sucesso!', 'success');
            } else {
                response.json().then(error => mostrarSwal('Erro ao adicionar nota.', 'error'));
            }
        })
        .catch(() => mostrarSwal('Erro ao adicionar nota.', 'error'));
});

document.querySelector('#formEditarNota').addEventListener('submit', function (event) {
    event.preventDefault();
    const nota = {
        id: document.querySelector('#editarNotaId').value,
        nomePagador: document.querySelector('#editarNomePagador').value,
        numeroNotaFiscal: document.querySelector('#editarNumeroNotaFiscal').value,
        dataEmissao: new Date(document.querySelector('#editarDataEmissao').value).toISOString(),
        dataCobrança: document.querySelector('#editarDataCobrança').value ? new Date(document.querySelector('#editarDataCobrança').value).toISOString() : null,
        dataPagamento: document.querySelector('#editarDataPagamento').value ? new Date(document.querySelector('#editarDataPagamento').value).toISOString() : null,
        valor: parseFloat(document.querySelector('#editarValor').value),
        status: parseInt(document.querySelector('#editarStatus').value, 10),
        documentoNota: 'nota.pdf',
        documentoBoleto: 'boleto.pdf'
    };

    fetch(`/api/notasfiscais/${nota.id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(nota)
    })
        .then(response => {
            if (response.ok) {
                $('#editarNotaModal').modal('hide');
                carregarNotas(paginaAtual);
                mostrarSwal('Nota editada com sucesso!', 'success');
            } else {
                response.json().then(error => mostrarSwal('Erro ao editar nota.', 'error'));
            }
        })
        .catch(() => mostrarSwal('Erro ao editar nota.', 'error'));
});
