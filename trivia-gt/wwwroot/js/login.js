﻿$(function () {

    $("#frmLogin").hide();
    $("#frmLogin").show(1000).fadeIn('slow');

    $("#btnIngreso").on("click", function () {

        var form = $("#frmLogin");

        $.ajax({
            cache: false,
            url: $(this).data("url"),
            type: 'POST',
            dataType: 'json',
            data: form.serialize(),
            success: (result) => {
                
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