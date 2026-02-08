using System.Security.Cryptography;
using System.Text;

namespace Smake.Helper
{
    public class CryptoHelper
    {
        const string Passwort = "djsghiowrhurt9iwezriwehgfokweh9tfhwoirthweoihtoeriwh";
        const int SaltLength = 16;
        const int NonceLength = 12;
        const int TagLength = 16;
        const int Iterations = 100_000;

        public static byte[] Encrypt(string plainText)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltLength);
            byte[] nonce = RandomNumberGenerator.GetBytes(NonceLength);

            byte[] key = Rfc2898DeriveBytes.Pbkdf2(Passwort, salt, Iterations, HashAlgorithmName.SHA256, 32);

            byte[] plaintextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] ciphertext = new byte[plaintextBytes.Length];
            byte[] tag = new byte[TagLength];

            using var aesGcm = new AesGcm(key, TagLength);

            aesGcm.Encrypt(nonce, plaintextBytes, ciphertext, tag, null);

            using var ms = new MemoryStream();
            ms.Write(salt, 0, salt.Length);
            ms.Write(nonce, 0, nonce.Length);
            ms.Write(ciphertext, 0, ciphertext.Length);
            ms.Write(tag, 0, tag.Length);

            return ms.ToArray();
        }

        public static string Decrypt(byte[] payload)
        {
            byte[] salt = new byte[SaltLength];
            Array.Copy(payload, 0, salt, 0, SaltLength);

            byte[] nonce = new byte[NonceLength];
            Array.Copy(payload, SaltLength, nonce, 0, NonceLength);

            int tagStart = payload.Length - TagLength;
            int cipherStart = SaltLength + NonceLength;
            int cipherLength = tagStart - cipherStart;

            byte[] ciphertext = new byte[cipherLength];
            Array.Copy(payload, cipherStart, ciphertext, 0, cipherLength);

            byte[] tag = new byte[TagLength];
            Array.Copy(payload, tagStart, tag, 0, TagLength);

            byte[] key = Rfc2898DeriveBytes.Pbkdf2(Passwort, salt, Iterations, HashAlgorithmName.SHA256, 32);

            byte[] plaintextBytes = new byte[ciphertext.Length];

            using var aesGcm = new AesGcm(key, TagLength);

            aesGcm.Decrypt(nonce, ciphertext, tag, plaintextBytes, null);

            return Encoding.UTF8.GetString(plaintextBytes);
        }
    }
}
