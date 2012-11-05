using System;

namespace DowJones.Factiva.Currents.Website.Contracts
{
	/// <summary>
	/// Abstraction for caching mechanism
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface ICacheProvider<T>
	{

		/// <summary>
		/// Gets the item from cache.
		/// </summary>
		/// <param name="key">Key to cache item</param>
		/// <param name="minutes">Time to keep item in cache in minutes</param>
		/// <param name="builder">Builder function that creates the item</param>
		/// <returns></returns>
		T GetItemFromCache(string key, int minutes, Func<T> builder);
	}
}