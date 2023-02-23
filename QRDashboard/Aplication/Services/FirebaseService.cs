using QRDashboard.Domain.Interfaces;
using Firebase.Auth;
using Firebase.Storage;

namespace QRDashboard.Aplication.Services
{
    public class FirebaseService : IFirebaseService
    {
        //private readonly IGenericRepository _repository;

        public async Task<string> UploadStorage(Stream streamArchivo, string nomArchivo)
        {
            string urlImagen = "";

            try
            {
                //INGRESA AQUÍ TUS PROPIAS CREDENCIALES
                string email = "prueba001@gmail.com";
                string clave = "1234567";
                string ruta = "prueba-sotorage.appspot.com";
                string api_key = "AIzaSyAaAGkMZ3__Mdll1xNLYwUFKaaBx0QRKc4";


                var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
                var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    ruta,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child("Fotos_Perfil")
                    .Child(nomArchivo)
                    .PutAsync(streamArchivo, cancellation.Token);

                var downloadURL = await task;

                urlImagen = await task;

            }
            catch
            {
                urlImagen = "";
            }

            return urlImagen;
        }

        public async Task<bool> DeleteStorage(string nomArchivo)
        {
            try
            {
                //INGRESA AQUÍ TUS PROPIAS CREDENCIALES
                string email = "prueba001@gmail.com";
                string clave = "1234567";
                string ruta = "prueba-sotorage.appspot.com";
                string api_key = "AIzaSyAaAGkMZ3__Mdll1xNLYwUFKaaBx0QRKc4";


                var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
                var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

                var cancellation = new CancellationTokenSource();

                var task = new FirebaseStorage(
                    ruta,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                        ThrowOnCancel = true
                    })
                    .Child("Fotos_Perfil")
                    .Child(nomArchivo)
                    .DeleteAsync();

                 await task;
                 return true;

            }
            catch
            {
                return false;
            }
        }
    }
}