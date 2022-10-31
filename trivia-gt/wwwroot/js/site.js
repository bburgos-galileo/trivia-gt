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
                ShowMensajeError(result.message, result.direccion);
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

function ShowMensajeError(mensaje, url) {

    Notiflix.Notify.failure(
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

function ShowMensajeInformacion() {

    var mensaje = document.getElementById("iInfo").value;

    if (mensaje !== '') {
        Notiflix.Notify.info(mensaje, { timeout: 3000 });
        document.getElementById("iInfo").value = null;
    }
}




