$.validator.addMethod("cpf", function (value, element) {
    return this.optional(element) || validaCpf(value);
});
$.validator.unobtrusive.adapters.addBool("cpf");

function validaCpf(value) {
    if (value == null || value === '')
        return true;

    var sameNumbers = /^(\d)\1{10}$/;
    var result = true;

    // Remove máscara (ponto e traço) caso possua
    value = value.replace(/\D/g, '');

    // Valida se cpf:
    // Possui mais ou menos que 11 caracteres;
    // Possui todos os números iguais;
    if (value.length !== 11 || sameNumbers.test(value))
        return false;

    // Valida dígitos verificadores
    [9, 10].forEach(digit => {
        var sum = 0;
        var remaining;

        value
            .split(/(?=)/)
            .splice(0, digit)
            .forEach((number, index) => {
                sum += parseInt(number) * (digit + 2 - (index + 1));
            });

        remaining = sum % 11;
        remaining = remaining < 2 ? 0 : 11 - remaining;
        if (remaining !== +value.substring(digit, digit + 1))
            result = false;
    });

    return result;
}