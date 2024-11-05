
$(document).ready(function () {
    $('#formCadastro').submit(function(e) {
        e.preventDefault();

        clienteBeneficiarioModel.Cliente.Nome = $(this).find("#Nome").val();
        clienteBeneficiarioModel.Cliente.CEP = $(this).find("#CEP").val();
        clienteBeneficiarioModel.Cliente.CPF = $(this).find("#CPF").val();
        clienteBeneficiarioModel.Cliente.Email = $(this).find("#Email").val();
        clienteBeneficiarioModel.Cliente.Sobrenome = $(this).find("#Sobrenome").val();
        clienteBeneficiarioModel.Cliente.Nacionalidade = $(this).find("#Nacionalidade").val()
        clienteBeneficiarioModel.Cliente.Estado = $(this).find("#Estado").val();
        clienteBeneficiarioModel.Cliente.Cidade = $(this).find("#Cidade").val();
        clienteBeneficiarioModel.Cliente.Logradouro = $(this).find("#Logradouro").val();
        clienteBeneficiarioModel.Cliente.Telefone = $(this).find("#Telefone").val();

        $.ajax({
            url: urlPost,
            method: "POST",
            data: clienteBeneficiarioModel,
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
