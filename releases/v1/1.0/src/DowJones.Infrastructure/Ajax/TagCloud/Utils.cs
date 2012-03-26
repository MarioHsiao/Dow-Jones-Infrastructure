using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace DowJones.Utilities.Ajax.TagCloud
{

	/// <summary>
	/// Represents how the tag cloud can sort its tags
	/// </summary>
	public enum TagCloudOrder
	{
		/// <summary>
		/// Renders tags alphabetically
		/// </summary>
		Alphabetical,

		/// <summary>
		/// Renders tags alphabetically descending
		/// </summary>
		AlphabeticalDescending,

		/// <summary>
		/// Renders tags with higher weight at the end
		/// </summary>
		Weight,

		/// <summary>
		/// Renders tags with higher weight at the beginning
		/// </summary>
		WeightDescending,

		/// <summary>
		/// Renders tags with higher weight in the middle
		/// </summary>
		Centralized,

		/// <summary>
		/// Renders tags with higher weight at the edges (start and end)
		/// </summary>
		Decentralized,

		/// <summary>
		/// Renders tags rendomly
		/// </summary>
		Random
	}
}