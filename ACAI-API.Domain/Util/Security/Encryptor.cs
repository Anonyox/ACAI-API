using System.Security.Cryptography;
using System.Text;

namespace ACAI_API.Domain.Util.Security
{
    public static class Encryptor
	{
		#region [Hash]

		public static string Hash(string input, HashType hashType = HashType.MD5)
		{
			HashAlgorithm hashAlgorithm;

			switch (hashType)
			{
				case HashType.MD5:
					hashAlgorithm = MD5.Create();
					break;
				case HashType.SHA1:
					hashAlgorithm = SHA1.Create();
					break;
				case HashType.SHA256:
					hashAlgorithm = SHA256.Create();
					break;
				case HashType.SHA384:
					hashAlgorithm = SHA384.Create();
					break;
				case HashType.SHA512:
					hashAlgorithm = SHA512.Create();
					break;
				case HashType.KeyedHashAlgorithm:
					hashAlgorithm = KeyedHashAlgorithm.Create();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(hashType));
			}

			byte[] inputBytes = Encoding.ASCII.GetBytes(input);

			byte[] hash = hashAlgorithm.ComputeHash(inputBytes);

			var result = new StringBuilder();

			for (int i = 0; i < hash.Length; i++)
				result.Append(hash[i].ToString("x2"));

			return result.ToString();
		}

		#endregion

		#region [Symmetric]

		internal const string Inputkey = "3754f25c-497a-4f2c-901d-cdf3efb492c7";

		private static SymmetricAlgorithm CreateSymmetricAlgorithm(string key, SymmetricType symmetricType, string inputKey = null)
		{
			if (string.IsNullOrWhiteSpace(inputKey))
				inputKey = Inputkey;

			var keyBytes = Encoding.ASCII.GetBytes(key);
			var salt = new Rfc2898DeriveBytes(inputKey, keyBytes);

			SymmetricAlgorithm symmetricAlgorithm;

			switch (symmetricType)
			{
				case SymmetricType.Aes:
					symmetricAlgorithm = Aes.Create();
					break;
				case SymmetricType.DES:
					symmetricAlgorithm = DES.Create();
					break;
				case SymmetricType.RC2:
					symmetricAlgorithm = RC2.Create();
					break;
				case SymmetricType.Rijndael:
					symmetricAlgorithm = Rijndael.Create();
					break;
				case SymmetricType.TripleDES:
					symmetricAlgorithm = TripleDES.Create();
					break;
				default:
					throw new ArgumentOutOfRangeException("s");
			}

			symmetricAlgorithm.Key = salt.GetBytes(symmetricAlgorithm.KeySize / 8);
			symmetricAlgorithm.IV = salt.GetBytes(symmetricAlgorithm.BlockSize / 8);

			return symmetricAlgorithm;
		}

		public static string SymmetricEncrypt(string input, string key = null, SymmetricType symmetricType = SymmetricType.Aes)
		{
			if (string.IsNullOrEmpty(key))
				key = Inputkey;

			var symmetricAlgorithm = CreateSymmetricAlgorithm(key, symmetricType);

			var encryptor = symmetricAlgorithm.CreateEncryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);
			var msEncrypt = new MemoryStream();
			using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
			{
				using (var swEncrypt = new StreamWriter(csEncrypt))
				{
					swEncrypt.Write(input);
				}
			}

			return Convert.ToBase64String(msEncrypt.ToArray());
		}

		public static string SymmetricDecrypt(string input, string key = null, SymmetricType symmetricType = SymmetricType.Aes)
		{
			if (string.IsNullOrEmpty(key))
				key = Inputkey;

			string result;

			var symmetricAlgorithm = CreateSymmetricAlgorithm(key, symmetricType);

			var decryptor = symmetricAlgorithm.CreateDecryptor(symmetricAlgorithm.Key, symmetricAlgorithm.IV);

			var cipher = Convert.FromBase64String(input);

			using (var msDecrypt = new MemoryStream(cipher))
			{
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (var srDecrypt = new StreamReader(csDecrypt))
					{
						result = srDecrypt.ReadToEnd();
					}
				}
			}
			return result;
		}

		#endregion

		#region [MetalQuip Encryptor Methods]

		private const string encryptorTemplate = "m3t!4l{0}Qu;1p";

		public static string HashPassword(string value) => Hash(string.Format(encryptorTemplate, value), HashType.SHA1);

		#endregion
	}
}
