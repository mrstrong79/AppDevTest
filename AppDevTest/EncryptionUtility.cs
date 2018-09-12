using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace AppDevTest
{
    public class EncryptionUtility
    {
        public static void encrypt()
        {
            string inFileName = @"C:\temp\SerializedString.Data";
            string outFileName = @"C:\temp\SerializedString.Data.enc";

            // Step 1: Create the Stream objects 
            FileStream inFile = new FileStream(inFileName, FileMode.Open, FileAccess.Read);
            FileStream outFile = new FileStream(outFileName, FileMode.OpenOrCreate, FileAccess.Write);

            // Step 2: Create the SymmetricAlgorithm object 
            RijndaelManaged myAlg = new RijndaelManaged();

            // Step 3: Specify a key (optional) 
            myAlg.GenerateKey();

            // Read the unencrypted file into fileData 
            byte[] fileData = new byte[inFile.Length];
            inFile.Read(fileData, 0, (int)inFile.Length);

            // Step 4: Create the ICryptoTransform object 
            ICryptoTransform encryptor = myAlg.CreateEncryptor();

            // Step 5: Create the CryptoStream object 
            CryptoStream encryptStream = new CryptoStream(outFile, encryptor, CryptoStreamMode.Write);

            // Step 6: Write the contents to the CryptoStream 
            encryptStream.Write(fileData, 0, fileData.Length);

            // Close the file handles 
            encryptStream.Close();
            inFile.Close();
            outFile.Close();
        }

        public static void createRSAKey()
        {
            // Create an instane of the RSA Algorithm object
            RSACryptoServiceProvider myRSA = new RSACryptoServiceProvider();

            // Create a new RSAParameters object with only the public key
            RSAParameters publicKey = myRSA.ExportParameters(false);

            Console.WriteLine(publicKey.Exponent.ToString());
            Console.WriteLine(publicKey.Modulus.ToString());
            Console.WriteLine(publicKey.GetHashCode().ToString());

        }

        public static void CryptoStreamExample()
        {
            string cardInfo = "Hall, Don; 4455-5566-6677-8899; 09/2008";
            //RC2CryptoServiceProvider encrypter = new RC2CryptoServiceProvider();
            RijndaelManaged myAlg = new RijndaelManaged();
            FileStream fileStream = File.Open(@"c:\temp\ProtectedCardInfo2.txt", FileMode.OpenOrCreate);
            ICryptoTransform encryptor = myAlg.CreateDecryptor();
            CryptoStream cryptoStream = new CryptoStream(fileStream, encryptor, CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            try
            {
                writer.WriteLine(cardInfo);
                Console.WriteLine("Card information successfully written to file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred when writing to the file.");
            }
            finally
            {
                cryptoStream.Close();
                cryptoStream.Dispose();
                myAlg.Clear();
                fileStream.Close();
                fileStream.Dispose();
            }

        }


        /// <summary>
        /// Encrypts the string and creates a Rijndael Cipher
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string EncryptString(string input, string key)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            // First we need to turn the input strings into a byte array.
            byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(input);

            // We are using salt to make it harder to guess our key
            // using a dictionary attack.
            byte[] Salt = Encoding.ASCII.GetBytes(key.Length.ToString());

            // The (Secret Key) will be generated from the specified
            // password and salt.
            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);

            // Create a encryptor from the existing SecretKey bytes.
            // We use 32 bytes for the secret key
            // (the default Rijndael key length is 256 bit = 32 bytes) and
            // then 16 bytes for the IV (initialization vector),
            // (the default Rijndael IV length is 128 bit = 16 bytes)
            ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));


            // Create a MemoryStream that is going to hold the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Create a CryptoStream through which we are going to be processing our data.
            // CryptoStreamMode.Write means that we are going to be writing data
            // to the stream and the output will be written in the MemoryStream
            // we have provided. (always use write mode for encryption)
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);

            // Start the encryption process.
            cryptoStream.Write(PlainText, 0, PlainText.Length);

            // Finish encrypting.
            cryptoStream.FlushFinalBlock();

            // Convert our encrypted data from a memoryStream into a byte array.
            byte[] CipherBytes = memoryStream.ToArray();

            // Close both streams.
            memoryStream.Close();
            cryptoStream.Close();

            // Convert encrypted data into a base64-encoded string.
            // A common mistake would be to use an Encoding class for that.
            // It does not work, because not all byte values can be
            // represented by characters. We are going to be using Base64 encoding
            // That is designed exactly for what we are trying to do.

            string EncryptedData = Convert.ToBase64String(CipherBytes);

            // Return encrypted string.
            return EncryptedData;
        }


        /// <summary>
        /// Decrypts a Rijndael Cipher and returns a string
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string DecryptString(string input, string key)
        {
            RijndaelManaged RijndaelCipher = new RijndaelManaged();

            byte[] EncryptedData = Convert.FromBase64String(input);
            byte[] Salt = Encoding.ASCII.GetBytes(key.Length.ToString());

            PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(key, Salt);

            // Create a decryptor from the existing SecretKey bytes.
            ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
            MemoryStream memoryStream = new MemoryStream(EncryptedData);

            // Create a CryptoStream. (always use Read mode for decryption).
            CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

            // Since at this point we don't know what the size of decrypted data
            // will be, allocate the buffer long enough to hold EncryptedData;
            // DecryptedData is never longer than EncryptedData.
            byte[] PlainText = new byte[EncryptedData.Length];

            // Start decrypting.
            int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);

            memoryStream.Close();
            cryptoStream.Close();

            // Convert decrypted data into a string.
            string DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);

            // Return decrypted string.  
            return DecryptedData;
        }

        public static string GetKey(string input)
        {
            return input.GetHashCode().ToString();
        }

        public static string GenerateRandomCode()
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
            {
                s = String.Concat(s, random.Next(10).ToString());
            }
            return s;
        }

        public static string GetHashValue(string input, HttpRequest request)
        {
            string baseUrl = request.Url.GetLeftPart(UriPartial.Authority);
            string ip = request.UserHostAddress; // Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string key = GetKey(baseUrl + ip);
            return EncryptString(input, key);
        }

        public static void GetNonKeyedHashValue()
        {
            // Create the non keyed hash algorithm
            //MD5 hashAlg = new MD5CryptoServiceProvider();
            SHA512 hashAlg = new SHA512CryptoServiceProvider();

            // store the data to be hashed into a byte array
            FileStream fs = new FileStream(@"c:\temp\test.txt", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            hashAlg.ComputeHash(br.ReadBytes((int) fs.Length));

            Console.WriteLine(Convert.ToBase64String(hashAlg.Hash));
        }

        public static void GeKeyedHashValue()
        {
            byte[] saltValueBytes = Encoding.ASCII.GetBytes("This is my salt");
            Rfc2898DeriveBytes passwordKey = new Rfc2898DeriveBytes("This is my password", saltValueBytes);
            byte[] secretKey = passwordKey.GetBytes(32);

            // Create the keyed hash algorithm object
            HMACSHA1 hashAlg = new HMACSHA1(secretKey);

            // store the data to be hashed into a byte array
            FileStream fs = new FileStream(@"c:\temp\test.txt", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            hashAlg.ComputeHash(br.ReadBytes((int)fs.Length));

            Console.WriteLine(Convert.ToBase64String(hashAlg.Hash));
        }

        public static void CreateDigitalSignature()
        {
            // Create the digital signature algorithm object
            DSACryptoServiceProvider signer = new DSACryptoServiceProvider();

            // store the data to be signed into a byte array
            FileStream fs = new FileStream(@"c:\temp\test.txt", FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            byte[] data = br.ReadBytes((int) fs.Length);

            // call the SignData method and store the signature
            byte[] signature = signer.SignData(data);

            // Export the public key
            string publicKey = signer.ToXmlString(false);

            Console.WriteLine(Convert.ToBase64String(signature));
            fs.Close();
            br.Close();

            DSACryptoServiceProvider verifier = new DSACryptoServiceProvider();

            // store the data to be signed into a byte array
            FileStream fs2 = new FileStream(@"c:\temp\test.txt", FileMode.Open, FileAccess.Read);
            BinaryReader br2 = new BinaryReader(fs2);
            byte[] data2 = br2.ReadBytes((int)fs2.Length);

            // Call the VerifyData method
            if (verifier.VerifyData(data2, signature))
            {
                Console.WriteLine("Signature Verified");
            }
            else
            {
                Console.WriteLine("Signature NOT Verified");
            }
            fs2.Close();
            br2.Close();
        }

        public static void SavePublicKey()
        {
            CspParameters csp = new CspParameters();
            csp.KeyContainerName = "AssymetricKey";
            RSACryptoServiceProvider rsaCryptoServiceProvider = new RSACryptoServiceProvider(csp);
            rsaCryptoServiceProvider.PersistKeyInCsp = true;

            //writes out the current key pair used in the rsa instance
            Console.WriteLine("Key is : \n" + rsaCryptoServiceProvider.ToXmlString(true));
        }
    }
}
