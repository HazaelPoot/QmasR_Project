namespace QRDashboard.Domain.Interfaces
{
    public interface IUtilityService
    {
        string EncryptMD5(string texto);
        string DesencryptMD5(string texto);
    }
}