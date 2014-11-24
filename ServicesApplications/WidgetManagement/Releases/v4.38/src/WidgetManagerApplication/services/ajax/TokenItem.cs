namespace EMG.widgets.ui.services.web
{
    /// <summary>
    /// Struct representing a Name value pair 
    /// </summary>
    public struct TokenItem
    {
        /// <summary>
        /// Key of the token Item
        /// </summary>
        public string Key;

        /// <summary>
        /// Value of the token Item
        /// </summary>
        public string Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenItem"/> struct.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public TokenItem(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
