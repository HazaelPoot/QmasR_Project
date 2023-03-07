const MODELO_BASE_USER = {
    idUser: 0,
    nombre: "",
    apellidos: "",
    username: "",
    passw: "",
    adminType: 0,
    urlImagen: "",
}

//DATA TABLE: GET
let tablaData;
$(document).ready(function(){

    fetch("/Usuario/ListaRoles")
    .then(response => {
        return response.ok ? response.json(): Promise.reject(response);
    })
    .then(responseJson => {
        if(responseJson.length > 0){
            responseJson.forEach((item) => {
                $("#cboRol").append(
                    $("<option>").val(item.idRol).text(item.tipo)
                )
            })
        }
    })

    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Usuario/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idUser", "visible":false, "searchable": false },
            {
                "data": "urlImagen", render: function(data){
                return `<img style="height:60px" src=${data} class="rounded mx-auto d-block"/>`
            }},
            { "data": "nombre" },
            { "data": "apellidos" },
            { "data": "username" },
            { "data": "passw" },
            {"data": "adminTypeName" },
            {
                "defaultContent": '<button class="btn btn-primary btn-editar btn-sm mr-2"><i class="fas fa-pencil-alt"></i></button>' +
                    '<button class="btn btn-danger btn-eliminar btn-sm"><i class="fas fa-trash-alt"></i></button>',
                "orderable": false,
                "searchable": false,
                "width": "80px"
            }
        ],
        order: [[0, "desc"]],
        dom:"Bfrtip",
        buttons:[
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Q+R Usuarios',
                exportOptions: {
                    columns: [0,2,3,4,5,6]
                }
            },'pageLength'
        ],
        language:{
            url:"https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
      });
})

//MODAL FORM USUARIO
function mostarModal(modelo = MODELO_BASE_USER){
    $("#txtId").val(modelo.idUser)
    $("#txtNombre").val(modelo.nombre)
    $("#txtApellido").val(modelo.apellidos)
    $("#txtUserName").val(modelo.username)
    $("#txtPassword").val(modelo.passw)
    $("#cboRol").val(modelo.adminType == 0 ? $("#cboRol option:first").val(): modelo.adminType)
    $("#txtFoto").val("")
    $("#imgUsuario").attr("src", modelo.urlImagen)
    $("#modalData").modal("show")
}

//BOTON NUEVO USUARIO
$("#btnNuevo").click(function(){
    mostarModal()
})

//BOTON GUARDAR: POST - PUT
$("#btnGuardar").click(function(){

    const inputs = $("input.input-validar").serializeArray();
    const inputsSnValor = inputs.filter((item) => item.value.trim() == "")

    if(inputsSnValor.length > 0){
        const mensaje = `Debe completar el campo "${inputsSnValor[0].name}"`
        toastr.warning("",mensaje)
        $(`input[name="${inputsSnValor[0].name}"]`).focus()
        return;
    }
    
    const modelo = structuredClone(MODELO_BASE_USER);
    modelo["idUser"] = parseInt($("#txtId").val())
    modelo["nombre"] = $("#txtNombre").val()
    modelo["apellidos"] = $("#txtApellido").val()
    modelo["username"] = $("#txtUserName").val()
    modelo["passw"] = $("#txtPassword").val()
    modelo["adminType"] = $("#cboRol").val()
    
    const inputFoto = document.getElementById("txtFoto")
    const formData = new FormData();

    formData.append("foto", inputFoto.files[0])
    formData.append("modelo", JSON.stringify(modelo))

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if(modelo.idUser == 0){
        fetch("/Usuario/Crear",{
            method: "POST",
            body: formData
        })
        .then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json(): Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.status) {
                tablaData.row.add(responseJson.obejct).draw(false)
                $("#modalData").modal("hide")
                swal("Listo!", "El Usuario fue creado", "success")
            }else{
                swal("Lo sentimos", responseJson.Mesaje, "error")
            }
        })
    }
    else
    {
        fetch("/Usuario/Editar",{
            method: "PUT",
            body: formData
        })
        .then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json(): Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.status) {
                
                tablaData.row(filaSeleccionada).data(responseJson.obejct).draw(false)
                filaSeleccionada = null;
                $("#modalData").modal("hide")
                swal("Listo!", "El Usuario fue modificado", "success")
            }else{
                swal("Lo sentimos", responseJson.Mesaje, "error")
            }
        })
    }
})

//SELECCIONAR UN USUARIO
let filaSeleccionada
$("#tbdata tbody").on("click", ".btn-editar", function(){

    if($(this).closest("tr").hasClass("child")){
        filaSeleccionada = $(this).closest("tr").prev();
    }else{
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();
    mostarModal(data);
})

//ELIMINAR USUARIO: DELETE
$("#tbdata tbody").on("click", ".btn-eliminar", function(){

    let fila;
    if($(this).closest("tr").hasClass("child")){
        fila = $(this).closest("tr").prev();
    }else{
        fila = $(this).closest("tr");
    }

    const data = tablaData.row(fila).data();
    
    swal({
        title: "¿Está seguro?",
        text: `Eliminar Usuario "${data.nombre}"`,
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Si, Eliminar",
        cancelButtonText: "No, Cancelar",
        closeOnConfirm: false,
        closeOnCancel: true,
    },
        function(response)
        {
            if(response)
            {
                $(".showSweetAlert").LoadingOverlay("show");

                fetch(`/Usuario/Eliminar?idUsuario=${data.idUser}`,{
                    method: "DELETE"
                })
                .then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide");
                    return response.ok ? response.json(): Promise.reject(response);
                })
                    .then(responseJson => {

                        if (responseJson.status) {
                        
                        tablaData.row(fila).remove().draw()
                        
                        swal("Listo!", "El Usuario fue Eliminado", "success")
                    }else{
                        swal("Lo sentimos", responseJson.Mesaje, "error")
                    }
                })
            }
        }
    )

})


