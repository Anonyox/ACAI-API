namespace ACAI_API.Domain.Util.Caching
{
    public interface ICache
	{
		string Name { get; }

		/// <summary>
		/// Clear the entire cache. Remove all keys.
		/// </summary>
		/// <returns></returns>
		bool Clear();

		/// <summary>
		/// Check if a key exists on the cache.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <returns></returns>
		bool ExistsKey(string key);

		/// <summary>
		/// Fetch a content from the cache. 
		/// If the key does not exists then a new entry on cache is added using the default timeSpan for one minute with absolute expiration.
		/// </summary>
		/// <typeparam name="T">Type used on the result.</typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="retrieveData">Function that retrieves the data to cache.</param>
		/// <returns></returns>
		T Fetch<T>(string key, Func<T> retrieveData);

		/// <summary>
		/// Fetch a content from the cache. 
		/// If the key does not exists then a new entry on cache is added using the default timeSpan for one minute and sliding expiration when necessary.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="retrieveData"></param>
		/// <param name="slidingExpiration">Set to true if you want to renew the cache after the access.</param>
		/// <returns></returns>
		T Fetch<T>(string key, Func<T> retrieveData, bool slidingExpiration);

		/// <summary>
		/// Fetch a content from the cache. 
		/// If the key does not exists then a new entry on cache is added using period.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="retrieveData"></param>
		/// <param name="expiresAt">Period to expire a cache entry.</param>
		/// <returns></returns>
		T Fetch<T>(string key, Func<T> retrieveData, DateTime expiresAt);

		/// <summary>
		/// Fetch a content from the cache. 
		/// If the key does not exists then a new entry on cache is added using period.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="retrieveData"></param>
		/// <param name="validFor">Cache expiration time.</param>
		/// <returns></returns>
		T Fetch<T>(string key, Func<T> retrieveData, TimeSpan validFor);

		/// <summary>
		/// Fetch a content from the cache. 
		/// If the key does not exists then a new entry on cache is added using period and sliding expiration when necessary.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="retrieveData"></param>
		/// <param name="expiresAt">Period to expire a cache entry.</param>
		/// <param name="slidingExpiration"></param>
		/// <returns></returns>
		T Fetch<T>(string key, Func<T> retrieveData, DateTime expiresAt, bool slidingExpiration);

		/// <summary>
		/// Fetch a content from the cache. 
		/// If the key does not exists then a new entry on cache is added using period and sliding expiration when necessary.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="retrieveData"></param>
		/// <param name="validFor">Cache expiration time.</param>
		/// <param name="slidingExpiration"></param>
		/// <returns></returns>
		T Fetch<T>(string key, Func<T> retrieveData, TimeSpan validFor, bool slidingExpiration);

		/// <summary>
		/// Fetch a content from the cache. 
		/// If the key does not exists then a new entry on cache is added using period and sliding expiration when necessary.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="retrieveData"></param>
		/// <param name="validFor">Cache expiration time.</param>
		/// <param name="slidingExpiration"></param>
		/// <returns></returns>
		Task<T> FetchAsync<T>(string key, Func<T> retrieveData, TimeSpan validFor, bool slidingExpiration);

		/// <summary>
		/// Get a key from cache. If the key does not exists the null is returned.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <returns></returns>
		object Get(string key);

		/// <summary>
		/// Get a key from cache and convert it to the given type. If the key does not exists the null is returned.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="type">Type to return.</param>
		/// <returns></returns>
		object Get(string key, Type type);

		/// <summary>
		/// Get a key from cache. If the key does not exists default(T) is returned.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <returns></returns>
		T Get<T>(string key);

		/// <summary>
		/// Get a key from cache. If the key does not exists default(T) is returned. The cache is renewed with DateTime defined.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="expiresAt">Period to expire a cache entry.</param>
		/// <returns></returns>
		T Get<T>(string key, DateTime expiresAt);

		/// <summary>
		/// Get a key from cache. If the key does not exists default(T) is returned. The cache is renewed with TimeSpan defined.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="validFor">Cache expiration time.</param>
		/// <returns></returns>
		T Get<T>(string key, TimeSpan validFor);

		/// <summary>
		/// Removes a key from cache.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <returns></returns>
		bool Remove(string key);

		/// <summary>
		/// Removes a key from cache.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		Task<bool> RemoveAsync(string key);

		/// <summary>
		/// Set a new entry on cache for default timeSpan of one minute. If key already holds a value, it is overwritten.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="data">Data to persist on cache.</param>
		/// <returns></returns>
		bool Set(string key, object data);

		/// <summary>
		/// Set a new entry on cache. If key already holds a value, it is overwritten.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="data">Data to persist on cache.</param>
		/// <param name="expiresAt">Period to expire a cache entry.</param>
		/// <returns></returns>
		bool Set(string key, object data, DateTime expiresAt);

		/// <summary>
		/// Set a new entry on cache. If key already holds a value, it is overwritten.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="data">Data to persist on cache.</param>
		/// <param name="validFor">Cache expiration time.</param>
		/// <returns></returns>
		bool Set(string key, object data, TimeSpan validFor);

		/// <summary>
		/// Set a new entry on cache for default timeSpan of one minute. If key already holds a value, it is overwritten.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="data">Data to persist on cache.</param>
		/// <returns></returns>
		Task<bool> SetAsync(string key, object data);

		/// <summary>
		/// Set a new entry on cache. If key already holds a value, it is overwritten.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="data">Data to persist on cache.</param>
		/// <param name="expiresAt">Period to expire a cache entry.</param>
		/// <returns></returns>
		Task<bool> SetAsync(string key, object data, DateTime expiresAt);

		/// <summary>
		/// Set a new entry on cache. If key already holds a value, it is overwritten.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="data">Data to persist on cache.</param>
		/// <param name="validFor">Cache expiration time.</param>
		/// <returns></returns>
		Task<bool> SetAsync(string key, object data, TimeSpan validFor);

		/// <summary>
		/// Define an expiration for a key given a period.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="validFor">Cache expiration time.</param>
		/// <returns></returns>
		bool SetExpire(string key, TimeSpan validFor);

		/// <summary>
		/// Define an expiration for a key.
		/// </summary>
		/// <param name="key">Key for the cache entry.</param>
		/// <param name="expiresAt">Period to expire a cache entry.</param>
		/// <returns></returns>
		bool SetExpire(string key, DateTime expiresAt);

		/// <summary>
		/// Get all the keys on the cache.
		/// </summary>
		/// <returns></returns>
		string[] GetKeys();
	}
}
