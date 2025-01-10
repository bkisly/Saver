using System.Security.Cryptography;

namespace Saver.AccountIntegrationService.Extensions;

public static class EncryptionExtensions
{
    public static string Encrypt(this string s, string key)
    {
        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);
        aes.GenerateIV();
        var iv = aes.IV;

        var encryptor = aes.CreateEncryptor(aes.Key, iv);

        using var memoryStream = new MemoryStream();
        memoryStream.Write(iv, 0, iv.Length);

        using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
        var streamWriter = new StreamWriter(memoryStream);
        streamWriter.Write(s);
        streamWriter.Flush();
        return Convert.ToBase64String(memoryStream.ToArray());
    }

    public static string Decrypt(this string s, string key)
    {
        var cipherBytes = Convert.FromBase64String(s);

        using var aes = Aes.Create();
        aes.Key = Convert.FromBase64String(key);

        var iv = new byte[aes.BlockSize / 8];
        Array.Copy(cipherBytes, 0, iv, 0, iv.Length);
        aes.IV = iv;

        var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

        using var memoryStream = new MemoryStream(cipherBytes, iv.Length, cipherBytes.Length - iv.Length);
        using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
        var streamReader = new StreamReader(memoryStream);
        
        return streamReader.ReadToEnd();
    }
}