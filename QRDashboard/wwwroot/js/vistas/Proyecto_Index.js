const MODELO_BASE = {
  idProj: 0,
  titulo: "",
  descripcion: "",
  ubicacion: "",
  presupuesto: 0,
  idCategoria: 0,
  categoriaName: "",
  status: 0,
  urlImagen: "",
};

let gridData;
$(document).ready(function () {
  fetch("/Proyecto/ListaCategorias")
    .then((response) => {
      return response.ok ? response.json() : Promise.reject(response);
    })
    .then((responseJson) => {
      if (responseJson.length > 0) {
        responseJson.forEach((item) => {
          $("#cboCategoria").append(
            $("<option>").val(item.idCategoria).text(item.descripcion)
          );
        });
      }
    });
});

function mostarModal(modelo = MODELO_BASE) {
  $("#txtId").val(modelo.idProj);
  $("#txtTitulo").val(modelo.titulo);
  $("#txtDescripcion").val(modelo.descripcion);
  $("#txtPresupuesto").val(modelo.presupuesto);
  $("#txtUbicacion").val(modelo.ubicacion);
  $("#cboCategoria").val(
    modelo.idCategoria == 0
      ? $("#cboCategoria option:first").val()
      : modelo.idCategoria
  );
  $("#cboEstado").val(modelo.status);
  $("#txtFoto").val("");
  $("#imgPost").attr("src", modelo.urlImagen);
  $("#modalData").modal("show");
}

$("#btnNuevo").click(function () {
  mostarModal();
  // swal("Listo!", "El Usuario fue modificado", "success");
});

$("#btnDelete").click(function () {
  swal("Listo!", "El Usuario fue modificado", "success");
});

function openModalEdit(id) {
  $.ajax({
    url: "/Proyecto/GetById/" + id,
    type: "GET",
    dataType: "json",
    success: function (datos) {
      mostarModal(datos);
      console.log(id);
    },
    error: function (xhr, status, error) {},
  });
}

$("#btnGuardar").click(function () {
  const inputs = $("input.input-validar").serializeArray();
  const inputsSnValor = inputs.filter((item) => item.value.trim() == "");

  if (inputsSnValor.length > 0) {
    const mensaje = `Debe completar el campo "${inputsSnValor[0].name}"`;
    toastr.warning("", mensaje);
    $(`input[name="${inputsSnValor[0].name}"]`).focus();
    return;
  }

  const modelo = structuredClone(MODELO_BASE);
  modelo["idProj"] = parseInt($("#txtId").val());
  modelo["titulo"] = $("#txtTitulo").val();
  modelo["descripcion"] = $("#txtDescripcion").val();
  modelo["ubicacion"] = $("#txtUbicacion").val();
  modelo["presupuesto"] = $("#txtPresupuesto").val();
  modelo["idCategoria"] = $("#cboCategoria").val();
  modelo["status"] = $("#cboEstado").val();

  const inputFoto = document.getElementById("txtFoto");
  const formData = new FormData();

  formData.append("foto", inputFoto.files[0]);
  formData.append("modelo", JSON.stringify(modelo));

  $("#modalData").find("div.modal-content").LoadingOverlay("show");

  if (modelo.idProj == 0) {
    fetch("/Proyecto/Crear", {
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
              text: "El Proyecto fue creado",
              type: "success",
            },
            function () {
              location.reload();
            }
          );
        } else {
          swal("Lo sentimos", responseJson.Mesaje, "error");
        }
      });
  } else {
    fetch("/Proyecto/Editar", {
      method: "PUT",
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
              text: "El Proyecto fue Modificado",
              type: "success",
            },
            function () {
              location.reload();
            }
          );
        } else {
          swal("Lo sentimos", responseJson.Mesaje, "error");
        }
      });
  }
});

function openModalDelete(id) {
  $.ajax({
    url: "/Proyecto/GetById/" + id,
    type: "GET",
    dataType: "json",
    success: function (datos) {
      swal(
        {
          title: "¿Está seguro?",
          text: `Eliminar Proyecto "${datos.titulo}"`,
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

            fetch(`/Proyecto/Eliminar?idProj=${id}`, {
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
                      text: "El Proyecto fue Eliminado",
                      type: "success",
                    },
                    function () {
                      location.reload();
                    }
                  );
                } else {
                  swal("Lo sentimos", responseJson.Mesaje, "error");
                }
              });
          }
        }
      );
    },
  });
}
