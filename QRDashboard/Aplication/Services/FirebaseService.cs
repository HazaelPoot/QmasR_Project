using QRDashboard.Domain.Interfaces;
using Firebase.Auth;
using Firebase.Storage;

namespace QRDashboard.Aplication.Services
{
    public class FirebaseService : IFirebaseService
    {
        public async Task<string> UploadStorage(Stream streamArchivo, string carpetaDestino, string nomArchivo)
        {
            string urlImagen = "";

            try
            {
                //INGRESA AQUÍ LAS CREDENCIALES DE LA EMPRESA
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
                    .Child(carpetaDestino)
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

        public async Task<bool> DeleteStorage(string carpetaDestino, string nomArchivo)
        {
            try
            {
                //INGRESA AQUÍ LAS CREDENCIALES DE LA EMPRESA
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
                    .Child(carpetaDestino)
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