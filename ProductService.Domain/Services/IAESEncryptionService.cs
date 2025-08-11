namespace ProductService.Domain.Services
{
    public interface IAESEncryptionService
    {
        string Encrypt(string plainText);
        string Decrypt(string encryptedText);
    }
}
