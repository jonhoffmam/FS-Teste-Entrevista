
$(document).ready(function () {
    $('#formCadastro').submit(function(e) {
        e.preventDefault();
        $.ajax({
            url: urlPost,
            method: "POST",
            data: {
                "Nome": $(this).find("#Nome").val(),
                "CEP": $(this).find("#CEP").val(),
                "CPF": $(this).find("#CPF").val(),
                "Email": $(this).find("#Email").val(),
                "Sobrenome": $(this).find("#Sobrenome").val(),
                "Nacionalidade": $(this).find("#Nacionalidade").val(),
                "Estado": $(this).find("#Estado").val(),
                "Cidade": $(this).find("#Cidade").val(),
                "Logradouro": $(this).find("#Logradouro").val(),
                "Telefone": $(this).find("#Telefone").val()
            },
            error:
                function(r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function(r) {
                    ModalDialog("Sucesso!", r)
                    $("#formCadastro")[0].reset();
                }
        });
    });

    $('#formBeneficiario').submit(function (e) {
        e.preventDefault();

        if (!$(this).valid()) {
            e.preventDefault();
            return false;
        }

        adicionaBeneficiarios();
    });
});

function adicionaBeneficiarios() {
    var cpf = $('#formBeneficiario').find("#CPF").val();
    var nome = $('#formBeneficiario').find("#Nome").val();

    if (!cpf || !nome) {
        return;
    }

    var index = clienteBeneficiarioModel.Beneficiarios.findIndex(b => b.CPF === cpf);

    if (index !== -1) {
        ModalDialog("Ocorreu um erro", "O Beneficiário já existe.");
    } else {
        clienteBeneficiarioModel.Beneficiarios.push({ "CPF": cpf, "Nome": nome });

        $.ajax({
            url: urlListarBeneficiarios,
            method: 'POST',
            data: clienteBeneficiarioModel,
            error:
                function (r) {
                    if (r.status == 400)
                        ModalDialog("Ocorreu um erro", r.responseJSON);
                    else if (r.status == 500)
                        ModalDialog("Ocorreu um erro", "Ocorreu um erro interno no servidor.");
                },
            success:
                function (data) {
                    $('#listaBeneficiarios').html(data);
                    $('#listaBeneficiarios [data-mask="CPF"]').inputmask("999.999.999-99");
                }
        });
    }
}
