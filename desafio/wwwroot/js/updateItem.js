document.addEventListener("DOMContentLoaded", function () {
    // Adiciona um ouvinte de evento quando o DOM está pronto
    var cpfInputs = document.querySelectorAll(".cpfProdutorInput");

    cpfInputs.forEach(function (cpfInput) {
        cpfInput.addEventListener("input", function () {
            formatarCPF(this);
        });
    });

    function formatarCPF(cpfInput) {
        var cpf = cpfInput.value.replace(/\D/g, '');
        cpf = cpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4');
        cpfInput.value = cpf;

        console.log(cpf);

        var isValid = validarCPF(cpf);
        cpfInput.setAttribute('data-valid', isValid);

        console.log(isValid)

        var dataValidField = cpfInput.parentElement.querySelector('.dataValid');
        if (dataValidField) {
            dataValidField.value = isValid ? 'true' : 'false';
        }

        console.log(dataValidField)

        if (isValid) {
            console.log('CPF válido');
        } else {
            console.log('CPF inválido');
        }
    }

    function validarCPF(cpf) {
        cpf = cpf.replace(/[^\d]+/g, '');
        if (cpf == '') return false;
        // Elimina CPFs invalidos conhecidos
        if (cpf.length != 11 ||
            cpf == "00000000000" ||
            cpf == "11111111111" ||
            cpf == "22222222222" ||
            cpf == "33333333333" ||
            cpf == "44444444444" ||
            cpf == "55555555555" ||
            cpf == "66666666666" ||
            cpf == "77777777777" ||
            cpf == "88888888888" ||
            cpf == "99999999999")
            return false;
        // Valida 1o digito
        add = 0;
        for (i = 0; i < 9; i++)
            add += parseInt(cpf.charAt(i)) * (10 - i);
        rev = 11 - (add % 11);
        if (rev == 10 || rev == 11)
            rev = 0;
        if (rev != parseInt(cpf.charAt(9)))
            return false;
        // Valida 2o digito
        add = 0;
        for (i = 0; i < 10; i++)
            add += parseInt(cpf.charAt(i)) * (11 - i);
        rev = 11 - (add % 11);
        if (rev == 10 || rev == 11)
            rev = 0;
        if (rev != parseInt(cpf.charAt(10)))
            return false;
        return true;
    }
});