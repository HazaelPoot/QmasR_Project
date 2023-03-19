//MODELO DE LA RESPUESTA HTTP
const MODELO_BASE_FOTO = {
    idImg: 0,
    nombreFoto: "",
    urlImage: "",
    idProj: 0,
    nombreProyecto: "",
}

//ANIMACION DE CARGA DE IMAGEN (PRELOADER)
$(document).ready(function () {
  $(".wrapper").LoadingOverlay("show");
  $(".wrapper").LoadingOverlay("hide");
});

//MOSTRAR EN EL MODAL LA IMAGEN SELECCIONADA
function previewImage(event, querySelector){
	const input = event.target;
	$imgPreview = document.querySelector(querySelector);
	if(!input.files.length) return
	file = input.files[0];
	objectURL = URL.createObjectURL(file);
	$imgPreview.src = objectURL;
}

//MODAL FORM FOTO
function mostarModalImg(modelo = MODELO_BASE_FOTO){
    $("#txtId").val(modelo.idImg);
    $("#txtFoto").val("");
    if(modelo.urlImage == "")
    {
        modelo.urlImage = "https://static.vecteezy.com/system/resources/thumbnails/005/720/408/small_2x/crossed-image-icon-picture-not-available-delete-picture-symbol-free-vector.jpg";
    }
    $("#imgFoto").attr("src", modelo.urlImage)
    $("#modalData").modal("show");
}

//MODAL PARA VISUALIZAR LA FOTO
function mostarModalFoto(modelo = MODELO_BASE_FOTO){
  $("#txtId").val(modelo.idImg);
  $("#txtNombreFoto").val(modelo.nombreFoto);
  $("#imgPost").attr("src", modelo.urlImage);
  $("#modalImage").modal("show");
}

//OBTENER ID DEL ELEMENTO SELECCIONADO
function openModalFoto(id) {
  $.ajax({
    url: "/Foto/GetById/" + id,
    type: "GET",
    dataType: "json",
    success: function (datos) {
       mostarModalFoto(datos);
      console.log(datos);
    }
  });
}

//BOTON NUEVA FOTO
$("#btnNuevo").click(function(){
    mostarModalImg()
})

//BOTON GUARDA FOTO: POST
$("#btnGuardar").click(function () {
  const inputs = $("input.input-validar").serializeArray();
  const inputsSnValor = inputs.filter((item) => item.value.trim() == "");

  if (inputsSnValor.length > 0) {
    const mensaje = `Debe completar el campo "${inputsSnValor[0].name}"`;
    toastr.warning("", mensaje);
    $(`input[name="${inputsSnValor[0].name}"]`).focus();
    return;
  }

  const modelo = structuredClone(MODELO_BASE_FOTO);
  modelo["idImg"] = parseInt($("#txtId").val());
  //modelo["urlImage"] = $("#txtFoto").val();

  const inputFoto = document.getElementById("txtFoto");
  const formData = new FormData();

  formData.append("foto", inputFoto.files[0]);
  formData.append("modelo", JSON.stringify(modelo));

  $("#modalData").find("div.modal-content").LoadingOverlay("show");

  if (modelo.idImg == 0) {
    fetch("/Foto/Crear", {
      method: "POST",
      body: formData,
    })
      .then((response) => {
        $("#modalData").find("div.modal-content").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
      })
      .then((responseJson) => {
        if (responseJson.status) {
          $("#modalData").modal("hide");
          swal(
            {
              title: "Listo!",
              text: "La Foto se Agregó",
              type: "success",
            },
            function () {
              location.reload();
            }
          );
        } else {
          swal("Lo sentimos", responseJson.message, "error");
        }
      });
  }
});

//ELIMINAR FOTO: DELETE
function openModalDelete(id) {
  $.ajax({
    url: "/Foto/GetById/" + id,
    type: "GET",
    dataType: "json",
    success: function (datos) {
      swal(
        {
          title: "¿Está seguro?",
          text: `Eliminar la Imagen "${datos.nombreFoto}"`,
          type: "warning",
          showCancelButton: true,
          confirmButtonClass: "btn-danger",
          confirmButtonText: "Si, Eliminar",
          cancelButtonText: "No, Cancelar",
          closeOnConfirm: false,
          closeOnCancel: true,
        },
        function (response) {
          if (response) {
            $(".showSweetAlert").LoadingOverlay("show");

            fetch(`/Foto/Eliminar?idImage=${id}`, {
              method: "DELETE",
            })
              .then((response) => {
                $(".showSweetAlert").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
              })
              .then((responseJson) => {
                if (responseJson.status) {
                  swal(
                    {
                      title: "Listo!",
                      text: "La imagen fue Eliminada",
                      type: "success",
                    },
                    function () {
                      location.reload();
                    }
                  );
                } else {
                  swal("Lo sentimos", responseJson.message, "error");
                }
              });
          }
        }
      );
    },
  });
}