using System.Security.Cryptography;

namespace Smake.Helper
{
    public class CryptoHelper
    {
        private static readonly string Passwort = "djsghiowrhurt9iwezriwehgfokweh9tfhwoirthweoihtoeriwh";

        // Länge des Salts in Bytes
        private const int SaltLength = 16;

        public static byte[] Encrypt(string plainText)
        {
            // Zufälligen Salt erzeugen
            byte[] salt = RandomNumberGenerator.GetBytes(SaltLength);

            using var aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(Passwort, salt, 100_000, HashAlgorithmName.SHA256);

            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
            }

            byte[] encryptedData = ms.ToArray();

            // Salt vorne anfügen, damit es beim Entschlüsseln verfügbar ist
            byte[] result = new byte[SaltLength + encryptedData.Length];
            Array.Copy(salt, 0, result, 0, SaltLength);
            Array.Copy(encryptedData, 0, result, SaltLength, encryptedData.Length);

            return result;
        }

        public static string Decrypt(byte[] cipherWithSalt)
        {
            // Salt aus den ersten Bytes auslesen
            byte[] salt = new byte[SaltLength];
            Array.Copy(cipherWithSalt, 0, salt, 0, SaltLength);

            // Restliche Daten = verschlüsselter Text
            byte[] cipherText = new byte[cipherWithSalt.Length - SaltLength];
            Array.Copy(cipherWithSalt, SaltLength, cipherText, 0, cipherText.Length);

            using var aes = Aes.Create();
            using var key = new Rfc2898DeriveBytes(Passwort, salt, 100_000, HashAlgorithmName.SHA256);

            aes.Key = key.GetBytes(32);
            aes.IV = key.GetBytes(16);

            using var ms = new MemoryStream(cipherText);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
        }
    }
}
