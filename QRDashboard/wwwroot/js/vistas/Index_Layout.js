
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