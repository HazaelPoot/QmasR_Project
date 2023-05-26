$(document).ready(function(){

  $(".container-fluid").LoadingOverlay("show");

    fetch("/Home/ObtenerPerfil")
      .then(response => {
        $(".container-fluid").LoadingOverlay("hide");
        return response.ok ? response.json() : Promise.reject(response);
      })
      .then(responseJson =>{

        if(responseJson.status){
          const d = responseJson.object

          $("#imgFoto").attr("src", d.urlImagen)
          $("#txtNombre").val(d.nombre)
          $("#txtApellidos").val(d.apellidos)
          $("#txtUsername").val(d.username)
          $("#txtPassword").val(d.passw)
          $("#txtRol").val(d.adminTypeName)
        }
        else{
          swal("Lo sentimos", responseJson.mesaje, "error")
        }
      })
})

$("#btnGuardarCambios").click(function(){

  if($("#txtNombre").val().trim() == ""){
    toastr.warning("", "Debe completar el campo Nombre")
    $("txtNombre").focus()
    return;
  }

  if($("#txtApellidos").val().trim() == ""){
    toastr.warning("", "Debe completar el campo Apellidos")
    $("txtApellidos").focus()
    return;
  }

  if($("#txtUsername").val().trim() == ""){
    toastr.warning("", "Debe completar el campo Username")
    $("txtUsername").focus()
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
          nombre: $("#txtNombre").val().trim(),
          apellidos: $("#txtApellidos").val().trim(),
          username: $("#txtUsername").val().trim(),
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
            swal("Lo sentimos", responseJson.message, "error")
          }
        });
      }
    }
  );
});

$("#btnCambiarClave").click(function(){

  const inputs = $("input.input-validar").serializeArray();
  const input_sin_valor = inputs.filter((item) => item.value.trim() == "");

  if(input_sin_valor.length > 0)
  {
    const mensaje = `Debe completar el campo "${input_sin_valor[0].name}"`;
    toastr.warning("", mensaje);
    $(`input[name="${input_sin_valor[0].name}"]`).focus()
    return;
  }

  if($("#txtClaveNueva").val().trim() != $("#txtConfirmarClave").val().trim()){
    toastr.warning("", "Las contraseñas no coinciden")
    return;
  }

  let modeloPass = {
    claveActual: $("#txtClaveActual").val().trim(),
    claveNueva: $("#txtClaveNueva").val().trim(),
  }

  swal(
    {
      title: "¿Esta seguro de cambiar su contraseña?",
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

        fetch("/Home/CambiarClave", {
          method: "POST",
          headers: {"Content-Type":"application/json; charset=utf-8"},
          body: JSON.stringify(modeloPass)

        })
        .then((response) => {
          $(".showSweetAlert").LoadingOverlay("hide");
          return response.ok ? response.json() : Promise.reject(response);
        })
        .then(responseJson => {
          if(responseJson.status){
            swal("Listo!", "Su contraseña fue cambiada", "success");
            $("input.input-validar").val("");
          }else{
            swal("Lo sentimos", responseJson.message, "error")
          }
        });
      }
    }
  );
});