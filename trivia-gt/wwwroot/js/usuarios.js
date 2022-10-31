$(function () {

    $("#tblLista").on('click', '.btnSelect', function () {
        // get the current row
        var currentRow = $(this).closest("tr");

        let usuario = {};

        usuario.Correo = currentRow.find('td:eq(2)').text();
        usuario.IdRol = parseInt(currentRow.find('.cmbRol').val());

        let lista = JSON.stringify(usuario);

        const res = fetch("/Home/Actualiza", {
            method: "POST",
            body: lista,
            headers: {
                'Accept': 'application/json',
                "Content-Type": "application/json"
            }
        }).then(res => res.json()).then(res => ShowMensajeAct(res.direccion));
    });

});

function ActualizarUsuarios() {

    let usersList = [];

    $("#tblLista tbody tr").each(function () {

        let row = $(this);
        let usuario = {};

        usuario.Correo = row.find('td:eq(2)').text();
        usuario.IdRol = parseInt(row.find('.cmbRol').val());
        usersList.push(usuario);


    });

    let lista = JSON.stringify(usersList);

    const res = fetch("/Home/Grabar", {
        method: "POST",
        body: lista,
        headers: {
            'Accept': 'application/json',
            "Content-Type": "application/json"
        }
    }).then(res => res.json()).then(res => ShowMensajeAct(res.direccion));

}


function ShowMensajeAct(url) {


    Notiflix.Notify.info(
        "Registro Actualizado, clic para continuar",
        function () {
            window.location = url;
        },
        {
            plainText: true,
            width: 400,
            messageMaxLength: 1000,
            timeout: 4000,
        },
    );


}