using System.Text;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace VnBusInfoW10.Utilities
{
    public class EncryptionService
    {
        public static byte[] Encrypt(string plainText, string pw, string sult)
        {
            IBuffer pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
            IBuffer sultBuffer = CryptographicBuffer.ConvertStringToBinary(sult, BinaryStringEncoding.Utf16LE);
            IBuffer plainBuffer = CryptographicBuffer.ConvertStringToBinary(plainText, BinaryStringEncoding.Utf16LE);
            KeyDerivationAlgorithmProvider key = KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
            KeyDerivationParameters parm = KeyDerivationParameters.BuildForPbkdf2(sultBuffer, 1000);
            CryptographicKey ckey = key.CreateKey(pwBuffer);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(ckey, parm, 32);
            CryptographicKey dkey = key.CreateKey(pwBuffer);
            IBuffer sulltMaterial = CryptographicEngine.DeriveKeyMaterial(dkey, parm, 16);
            SymmetricKeyAlgorithmProvider sp = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            CryptographicKey symKey = sp.CreateSymmetricKey(keyMaterial);
            IBuffer resultBuffer = CryptographicEngine.Encrypt(symKey, plainBuffer, sulltMaterial);
            byte[] result;
            CryptographicBuffer.CopyToByteArray(resultBuffer, out result);
            return result;
        }

        public static string EncryptToString(string data, string password, string salt)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] encrypt = Encrypt(data, password, salt);
            return encoding.GetString(encrypt);
        }

        public static string Decrypt(byte[] encryptval, string pw, string salt)
        {
            IBuffer pwBuffer = CryptographicBuffer.ConvertStringToBinary(pw, BinaryStringEncoding.Utf8);
            IBuffer saltBuffer = CryptographicBuffer.ConvertStringToBinary(salt, BinaryStringEncoding.Utf16LE);
            IBuffer cipherBuffer = CryptographicBuffer.CreateFromByteArray(encryptval);
            KeyDerivationAlgorithmProvider key = KeyDerivationAlgorithmProvider.OpenAlgorithm("PBKDF2_SHA1");
            KeyDerivationParameters parm = KeyDerivationParameters.BuildForPbkdf2(saltBuffer, 1000);
            CryptographicKey ckey = key.CreateKey(pwBuffer);
            IBuffer keyMaterial = CryptographicEngine.DeriveKeyMaterial(ckey, parm, 32);
            CryptographicKey dkey = key.CreateKey(pwBuffer);
            IBuffer saltMaterial = CryptographicEngine.DeriveKeyMaterial(ckey, parm, 16);
            SymmetricKeyAlgorithmProvider sp = SymmetricKeyAlgorithmProvider.OpenAlgorithm("AES_CBC_PKCS7");
            CryptographicKey symmKey = sp.CreateSymmetricKey(keyMaterial);
            IBuffer resultBuffer = CryptographicEngine.Decrypt(symmKey, cipherBuffer, saltMaterial);
            string result = CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf16LE, resultBuffer);
            return result;
        }

        public static string DecryptFromString(string data, string password, string salt)
        {
            byte[] toBytes = Encoding.UTF8.GetBytes(data);
            return Decrypt(toBytes, password, salt);
        }
    }
}