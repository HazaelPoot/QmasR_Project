const MODELO_BASE_CATEG = {
    idCategoria: 0,
    descripcion: ""
}

//DATA TABLE: GET
let tablaData;
$(document).ready(function(){

    tablaData = $('#tbdata').DataTable({
        responsive: true,
        "ajax": {
            "url": '/Categoria/Lista',
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { "data": "idCategoria", "visible":true, "searchable": true },
            { "data": "descripcion" },
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
                    columns: [0,1]
                }
            },'pageLength'
        ],
        language:{
            url:"https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
      });
})

//MODAL FORM CATEGORIA
function mostarModal(modelo = MODELO_BASE_CATEG){
    $("#txtId").val(modelo.idCategoria);
    $("#txtDescripcion").val(modelo.descripcion);
    $("#modalData").modal("show")
}

//BOTON NUEVO CATEGORIA
$("#btnNuevo").click(function () {
    mostarModal();
});

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
    
    const modelo = structuredClone(MODELO_BASE_CATEG);
    modelo["idCategoria"] = parseInt($("#txtId").val())
    modelo["descripcion"] = $("#txtDescripcion").val()

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if(modelo.idCategoria == 0){
        fetch("/Categoria/Crear",{
            method: "POST",
            headers: {"Content-Type":"application/json; charset=utf-8"},
            body: JSON.stringify(modelo)
        })
        .then(response => {
            $("#modalData").find("div.modal-content").LoadingOverlay("hide");
            return response.ok ? response.json(): Promise.reject(response);
        })
        .then(responseJson => {
            if (responseJson.status) {
                tablaData.row.add(responseJson.obejct).draw(false)
                $("#modalData").modal("hide")
                swal("Listo!", "La Categoria fue creada", "success")
            }else{
                swal("Lo sentimos", responseJson.mesaje, "error")
                console.log(responseJson.mesaje)
            }
        })
    }
    else
    {
        fetch("/Categoria/Editar",{
            method: "PUT",
            headers: {"Content-Type": "application/json; charset=utf-8"},
            body: JSON.stringify(modelo)
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
                swal("Listo!", "La Categoria fue modificada", "success")
            }else{
                swal("Lo sentimos", responseJson.Mesaje, "error")
            }
        })
    }
})

//SELECCIONAR UN CATEGORIA
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

//ELIMINAR CATEGORIA: DELETE
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
        text: `Eliminar Categoria "${data.descripcion}"`,
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

                fetch(`/Categoria/Eliminar?idCategoria=${data.idCategoria}`,{
                    method: "DELETE"
                })
                .then(response => {
                    $(".showSweetAlert").LoadingOverlay("hide");
                    return response.ok ? response.json(): Promise.reject(response);
                })
                    .then(responseJson => {

                        if (responseJson.status) {
                        
                        tablaData.row(fila).remove().draw()
                        
                        swal("Listo!", "La Categoria fue Eliminada", "success")
                    }else{
                        swal("Lo sentimos", responseJson.Mesaje, "error")
                    }
                })
            }
        }
    )

})