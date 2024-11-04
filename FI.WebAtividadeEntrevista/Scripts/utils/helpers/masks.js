Inputmask.extendDefaults({
    showMaskOnFocus: false,
    showMaskOnHover: false,
    jitMasking: true,
    removeMaskOnSubmit: true
});

$('[data-mask="CPF"]').inputmask('999.999.999-99');

$('[data-mask="CEP"]').inputmask('99999-999');

$('[data-mask="Telefone"]').inputmask(['(99) 9999-9999', '(99) 99999-9999']);
