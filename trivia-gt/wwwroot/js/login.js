$(function () {

    $("#frmLogin").hide();
    $("#frmLogin").show(1000).fadeIn('slow');

    $("#btnIngreso").on("click", function () {

        var form = $("#frmLogin");

        Notiflix.Loading.arrows('Verificando...', {
            backgroundColor: 'rgba(0,0,0,0.8)',
        });

        $.ajax({
            cache: false,
            url: $(this).data("url"),
            type: 'POST',
            dataType: 'json',
            data: form.serialize(),
            success: (result) => {

                Notiflix.Loading.remove();

                if (result.success == true) {
                    window.location = result.direccion;
                } else {
                    ShowLoginError(result.message);
                }
            }
        });

    });

});

function ShowLoginError(mensaje) {
    Notiflix.Notify.failure(mensaje, {
        closeButton: true,
        plainText: false,
        messageMaxLength: 1000,
        width: 400,
        timeout: 3000
    });

}

function ShowMensajeInformacion() {

    var mensaje = document.getElementById("iInfo").value;

    if (mensaje !== '') {
        Notiflix.Notify.info(mensaje, { timeout: 3000 });
        document.getElementById("iInfo").value = null;
    }
}