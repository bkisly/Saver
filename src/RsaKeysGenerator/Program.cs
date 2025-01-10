using System.Security.Cryptography;

using var rsa = RSA.Create();
var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());

Console.WriteLine($"Public key: {publicKey}");
Console.WriteLine();
Console.WriteLine($"Private key: {privateKey}");
