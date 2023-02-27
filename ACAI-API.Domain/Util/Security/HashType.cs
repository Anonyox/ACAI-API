namespace ACAI_API.Domain.Util.Security
{
	public enum HashType
	{
		/// <summary>
		/// Message-Digest Algorithm 5
		/// </summary>
		MD5,
		/// <summary>
		/// Secure Hash Algorithm
		/// </summary>
		SHA1,
		/// <summary>
		/// Secure Hash Algorithm 256 bits
		/// </summary>
		SHA256,
		/// <summary>
		/// Secure Hash Algorithm 384 bits
		/// </summary>
		SHA384,
		/// <summary>
		/// Secure Hash Algorithm 512 bits
		/// </summary>
		SHA512,
		/// <summary>
		/// Keyed Hash Algorithm
		/// </summary>
		KeyedHashAlgorithm
	}
}
