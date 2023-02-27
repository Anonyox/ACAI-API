using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ACAI_API.Domain.Util.Caching
{
	public abstract class BaseCache : ICache
	{
		protected virtual bool UseSerialization => true;

		protected virtual bool UseCompression => true;

		protected virtual bool UseSetExpiration => true;

		protected virtual bool SupportPublishSubscribe => false;

		public abstract string Name { get; }

		public abstract bool Clear();

		public virtual bool ExistsKey(string key)
		{
			var data = Get(key);

			var exists = data != null;

			return exists;
		}

		public T Fetch<T>(string key, Func<T> retrieveData)
		{
			return Fetch(key, retrieveData, TimeSpan.FromMinutes(1));
		}

		public T Fetch<T>(string key, Func<T> retrieveData, bool slidingExpiration)
		{
			return Fetch(key, retrieveData, TimeSpan.FromMinutes(1), slidingExpiration);
		}

		public T Fetch<T>(string key, Func<T> retrieveData, DateTime expiresAt)
		{
			return Fetch(key, retrieveData, expiresAt, false);
		}

		public T Fetch<T>(string key, Func<T> retrieveData, DateTime expiresAt, bool slidingExpiration)
		{
			return Fetch(key, retrieveData, expiresAt - DateTime.Now, slidingExpiration);
		}

		public T Fetch<T>(string key, Func<T> retrieveData, TimeSpan validFor)
		{
			return Fetch(key, retrieveData, validFor, false);
		}

		public virtual T Fetch<T>(string key, Func<T> retrieveData, TimeSpan validFor, bool slidingExpiration)
		{
			if (ExistsKey(key))
			{
				T data = slidingExpiration
					? Get<T>(key, validFor)
					: Get<T>(key);

				if (data != null)
					return data;
			}

			T dataRetrieved = retrieveData();

			Set(key, dataRetrieved, validFor);

			return dataRetrieved;
		}

		public virtual async Task<T> FetchAsync<T>(string key, Func<T> retrieveData, TimeSpan validFor, bool slidingExpiration)
		{
			if (ExistsKey(key))
			{
				T data = slidingExpiration
					? Get<T>(key, validFor)
					: Get<T>(key);

				if (data != null)
					return data;
			}

			T dataRetrieved = retrieveData();

			await SetAsync(key, dataRetrieved, validFor);

			return dataRetrieved;
		}

		public object Get(string key)
		{
			var content = GetData(key);

			return content;
		}

		public object Get(string key, Type type)
		{
			var content = GetData(key);

			if (content == null)
			{
				Remove(key);

				return null;
			}

			if (UseSerialization)
			{
				var bytes = content as byte[];

				var data = Deserialize(bytes, type);

				return data;
			}

			return content;
		}

		public T Get<T>(string key)
		{
			var content = GetData(key);

			if (content == null)
			{
				Remove(key);

				return default(T);
			}

			if (UseSerialization)
			{
				var bytes = content as byte[];

				var data = Deserialize<T>(bytes);

				return data;
			}

			return (T)content;
		}

		public T Get<T>(string key, DateTime expiresAt)
		{
			return Get<T>(key, expiresAt - DateTime.Now);
		}

		public T Get<T>(string key, TimeSpan validFor)
		{
			var data = Get<T>(key);

			if (data != null)
			{
				if (UseSetExpiration)
				{
					SetExpire(key, DateTime.Now.Add(validFor));
				}
			}

			return data;
		}

		public abstract bool Remove(string key);

		public abstract Task<bool> RemoveAsync(string key);

		public abstract string[] GetKeys();

		public bool Set(string key, object data)
		{
			return Set(key, data, TimeSpan.FromMinutes(1));
		}

		public bool Set(string key, object data, DateTime expiresAt)
		{
			return Set(key, data, expiresAt - DateTime.Now);
		}

		public bool Set(string key, object data, TimeSpan validFor)
		{
			if (UseSerialization)
			{
				var bytes = Serialize(data);

				return SetData(key, bytes, validFor);
			}

			return SetData(key, data, validFor);
		}

		public async Task<bool> SetAsync(string key, object data)
		{
			var result = await SetAsync(key, data, TimeSpan.FromMinutes(1));

			return result;
		}

		public async Task<bool> SetAsync(string key, object data, DateTime expiresAt)
		{
			var result = await SetAsync(key, data, expiresAt - DateTime.Now);

			return result;
		}

		public async Task<bool> SetAsync(string key, object data, TimeSpan validFor)
		{
			if (UseSerialization)
			{
				var bytes = Serialize(data);

				var resultAsync = await SetDataAsync(key, bytes, validFor);

				return resultAsync;
			}

			var result = await SetDataAsync(key, data, validFor);

			return result;
		}


		public bool SetExpire(string key, TimeSpan validFor)
		{
			return SetExpire(key, DateTime.Now.Add(validFor));
		}

		public async Task<bool> SetExpireAsync(string key, TimeSpan validFor)
		{
			var result = await SetExpireAsync(key, DateTime.Now.Add(validFor));

			return result;
		}

		public abstract bool SetExpire(string key, DateTime expiresAt);

		public abstract Task<bool> SetExpireAsync(string key, DateTime expiresAt);

		#region [Persistence]

		protected abstract object GetData(string key);

		protected abstract Task<object> GetDataAsync(string key);

		protected abstract bool SetData(string key, object data, TimeSpan validFor);

		protected abstract Task<bool> SetDataAsync(string key, object data, TimeSpan validFor);

		#endregion

		#region [Serialization]

		protected virtual byte[] Serialize(object data)
		{
			if (data == null)
				return null;

			var dataToCache = JsonSerializer.Serialize(data);

			if (UseCompression)
			{
				var compressedString = Zip(dataToCache);

				dataToCache = Convert.ToBase64String(compressedString);
			}

			var result = Encoding.UTF8.GetBytes(dataToCache);

			return result;
		}

		protected virtual T Deserialize<T>(byte[] bytes)
		{
			if (bytes == null)
				return default(T);

			var dataFromCache = Encoding.UTF8.GetString(bytes);

			if (UseCompression)
			{
				var compressedBytes = Convert.FromBase64String(dataFromCache);

				var uncompressedString = Unzip(compressedBytes);

				dataFromCache = uncompressedString;
			}

			var result = JsonSerializer.Deserialize<T>(dataFromCache);

			return result;
		}

		protected virtual object Deserialize(byte[] bytes, Type type)
		{
			if (bytes == null)
				return null;

			var dataFromCache = Encoding.UTF8.GetString(bytes);

			if (UseCompression)
			{
				var compressedBytes = Convert.FromBase64String(dataFromCache);

				var uncompressedString = Unzip(compressedBytes);

				dataFromCache = uncompressedString;
			}

			var result = JsonSerializer.Deserialize(dataFromCache, type);

			return result;
		}

		#endregion

		#region [Compression]

		private void CopyTo(Stream source, Stream destionation)
		{
			byte[] bytes = new byte[4096];

			int cnt;

			while ((cnt = source.Read(bytes, 0, bytes.Length)) != 0)
			{
				destionation.Write(bytes, 0, cnt);
			}
		}

		private byte[] Zip(string s)
		{
			var bytes = Encoding.UTF8.GetBytes(s);

			using (var msi = new MemoryStream(bytes))
			{
				using (var mso = new MemoryStream())
				{
					using (var gs = new DeflateStream(mso, CompressionMode.Compress))
					{
						CopyTo(msi, gs);
					}

					return mso.ToArray();
				}
			}
		}

		private string Unzip(byte[] bytes)
		{
			using (var msi = new MemoryStream(bytes))
			{
				using (var mso = new MemoryStream())
				{
					using (var gs = new DeflateStream(msi, CompressionMode.Decompress))
					{
						CopyTo(gs, mso);
					}

					return Encoding.UTF8.GetString(mso.ToArray());
				}
			}
		}

		#endregion
	}
}
