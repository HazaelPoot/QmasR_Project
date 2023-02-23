namespace QRDashboard.Domain.Interfaces
{
    public interface IFirebaseService
    {
        Task<string> UploadStorage(Stream streamArchivo, string nomArchivo);
        Task<bool> DeleteStorage(string nomArchivo);
    }
}