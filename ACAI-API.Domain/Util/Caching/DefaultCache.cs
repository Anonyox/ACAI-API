using System.Runtime.Caching;


namespace ACAI_API.Domain.Util.Caching
{
    /// <summary>
    /// The ICache implemention for DefaultCache
    /// </summary>
    public class DefaultCache : BaseCache
	{
		private static readonly MemoryCache Cache = MemoryCache.Default;

		protected override bool UseSetExpiration => false;

		public override string Name => "MemoryCache";

		public override bool ExistsKey(string key)
		{
			return Cache.Contains(key);
		}

		public override bool Clear()
		{
			var keys = Cache.Select(x => x.Key).ToArray();

			foreach (var key in keys)
				Cache.Remove(key);

			return true;
		}

		public override bool Remove(string key)
		{
			var result = Cache.Remove(key);

			return result != null;
		}

		public override async Task<bool> RemoveAsync(string key)
		{
			var result = await Task.Run(() => Cache.Remove(key));

			return result != null;
		}

		public override bool SetExpire(string key, DateTime expiresAt)
		{
			var data = Get(key);

			if (data == null)
				return false;

			return SetData(key, data, expiresAt - DateTime.Now);
		}

		public override async Task<bool> SetExpireAsync(string key, DateTime expiresAt)
		{
			var data = Get(key);

			if (data == null)
				return false;

			var result = await SetDataAsync(key, data, expiresAt - DateTime.Now);

			return result;
		}

		protected override object GetData(string key)
		{
			var data = Cache.Get(key);

			return data;
		}

		protected override async Task<object> GetDataAsync(string key)
		{
			var data = await Task.Run(() => Cache.Get(key));

			return data;
		}

		protected override bool SetData(string key, object data, TimeSpan validFor)
		{
			var absoluteExpiration = DateTimeOffset.Now.Add(validFor);

			Cache.Set(key, data, absoluteExpiration);

			return true;
		}

		protected override async Task<bool> SetDataAsync(string key, object data, TimeSpan validFor)
		{
			var absoluteExpiration = DateTimeOffset.Now.Add(validFor);

			await Task.Run(() => Cache.Set(key, data, absoluteExpiration));

			return true;
		}

		public override string[] GetKeys()
		{
			return Cache.Select(x => x.Key).ToArray();
		}
	}
}
