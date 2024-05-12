using HashidsNet;
using System.Security.Cryptography;
using System.Text;

namespace BasicBoilerplate.Utilities
{
    public class HashCode
    {
        public static string EncodeId(int id)
        {
            var hashids = new Hashids("yourSalt", 8);
            var returnId = hashids.Encode(id);

            return returnId;
        }


        public static int DecodeId(string id)
        {
            try
            {
                var hashids = new Hashids("yourSalt", 8);
                var decodedId = hashids.Decode(id);
                return decodedId.First();
            }
            catch (Exception)
            {
                throw new Exception("Public Id can not found");
            }
        }

        public static string RandomString(int length)
        {
            var r = new Random();
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[r.Next(s.Length)]).ToArray());
        }

        public static string EncryptMd5(string password)
        {
            var data = Encoding.UTF8.GetBytes(password);
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var keys = md5.ComputeHash(Encoding.UTF8.GetBytes("yourSalt"));
                using (var tripDes = new TripleDESCryptoServiceProvider
                { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    var transform = tripDes.CreateEncryptor();
                    var results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        public static string DecryptMd5(string decryptPassword)
        {
            var data = Convert.FromBase64String(decryptPassword);
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var keys = md5.ComputeHash(Encoding.UTF8.GetBytes("yourSalt"));
                using (var tripDes = new TripleDESCryptoServiceProvider
                { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    var transform = tripDes.CreateDecryptor();
                    var results = transform.TransformFinalBlock(data, 0, data.Length);
                    return Encoding.UTF8.GetString(results);
                }
            }
        }
    }
}
