function GrabarPregunta() {

    var form = $("#frmPreguntas");

    $.ajax({
        cache: false,
        url: '/Preguntas/Grabar',
        type: 'POST',
        dataType: 'json',
        data: form.serialize(),
        success: (result) => {

            if (result.success == true) {
                ShowMensaje(result.message, result.direccion);
            } else {
                ShowMensajeError(result.message);
            }
        }
    });
}

function ShowMensaje(mensaje, url) {


    Notiflix.Notify.success(
        mensaje,
        function () {
            window.location = url;
        },
        {
            plainText: false,
            width: 400,
            messageMaxLength: 1000,
            timeout: 4000,
        },
    );


}

function ShowMensajeError(mensaje) {
    Notiflix.Notify.failure(mensaje, {
        closeButton: false,
        plainText: false,
        messageMaxLength: 1000,
        width: 400,
        timeout: 3000
    });

}