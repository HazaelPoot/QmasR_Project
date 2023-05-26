using QRDashboard.Domain.Interfaces;
using Firebase.Auth;
using Firebase.Storage;

namespace QRDashboard.Aplication.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IConfiguration _config;
        public FirebaseService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> UploadStorage(Stream streamArchivo, string carpetaDestino, string nomArchivo)
        {
            string urlImagen = "";

            try
            {
                string email = _config.GetSection("FireBase:Email").Value;
                string clave = _config.GetSection("FireBase:Clave").Value;
                string ruta = _config.GetSection("FireBase:Ruta").Value;
                string api_key = _config.GetSection("FireBase:Api_Key").Value;

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
                string email = _config.GetSection("FireBase:Email").Value;
                string clave = _config.GetSection("FireBase:Clave").Value;
                string ruta = _config.GetSection("FireBase:Ruta").Value;
                string api_key = _config.GetSection("FireBase:Api_Key").Value;

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