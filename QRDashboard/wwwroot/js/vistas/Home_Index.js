$(document).ready(function(){

  $(".container-fluid").LoadingOverlay("show");

    fetch("/Home/ObtenerPerfil")
      .then(response => {
        $(".container-fluid").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
      })
      .then(responseJson =>{

        if(responseJson.status){
          const d = responseJson.obejct

          $("#imgFoto").attr("src", d.urlImagen)
          $("#txtNombre").val(d.nombre)
          $("#txtUsername").val(d.username)
          $("#txPassword").val(d.passw)
          $("#txtRol").val(d.adminTypeName)
        }
        else{
          //ACTIVAR CUANDO SE VALIDEN TODOS LOS ROLES DEL LOGIN
          //swal("Lo sentimos", responseJson.mesaje, "error")
        }
      })
})

$("#btnGuardarCambios").click(function(){

  if($("#txtUsername").val().trim() == ""){
    toastr.warning("", "Debe completar el campo Username")
    $("txtUsername").focus()
    return;
  }

  if($("#txPassword").val().trim() == ""){
    toastr.warning("", "Debe completar el campo Contraseña")
    $("txPassword").focus()
    return;
  }

  swal(
    {
      title: "¿Desea Guardar los cambios?",
      type: "info",
      showCancelButton: true,
      confirmButtonClass: "btn-primary",
      confirmButtonText: "Si",
      cancelButtonText: "No",
      closeOnConfirm: false,
      closeOnCancel: true,
    },
    function (response) {
      if (response) {
        $(".showSweetAlert").LoadingOverlay("show");

        let modelo = {
          username: $("#txtUsername").val().trim(),
          passw: $("#txPassword").val().trim()
        }

        fetch("/Home/GuardarPerfil", {
          method: "POST",
          headers: {"Content-Type":"application/json; charset=utf-8"},
          body: JSON.stringify(modelo)

        })
        .then((response) => {
          $(".showSweetAlert").LoadingOverlay("hide");
          return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
          if(responseJson.status){
            swal("Listo!", "Los Cambios fueron guardados", "success")
          }else{
            swal("Lo sentimos", responseJson.mesaje, "error")
          }
        });
      }
    }
  );

})

function OpenDialogLogout() {
  swal(
    {
      title: "¿Desea cerrar Sesión?",
      type: "info",
      showCancelButton: true,
      confirmButtonClass: "btn-info",
      confirmButtonText: "Si, Salir",
      cancelButtonText: "No",
      closeOnConfirm: false,
      closeOnCancel: true,
    },
    function (response) {
      if (response) {
        $(".showSweetAlert").LoadingOverlay("show");

        fetch(`/Login/Logout`, {
          method: "GET",
        }).then((response) => {
          $(".showSweetAlert").LoadingOverlay("hide");
          // return response.ok ? response.json() : Promise.reject(response);
          if (response) {
            swal(
              {
                title: "Listo!",
                text: "Se cerró la sesión",
                type: "success",
              },
              function () {
                location.reload();
              }
            );
          }
        });
      }
    }
  );
}

