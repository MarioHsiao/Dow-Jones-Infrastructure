namespace factiva.widgets.ui.delegates.interfaces
{
    /// <summary>
    /// Interface that all output Delegates must adhere too
    /// </summary>
    public interface IOutputDelegate
    {
        /// <summary>
        /// Deserializes to RSS.
        /// </summary>
        /// <returns></returns>
        string ToRSS();

        /// <summary>
        /// Deserializes to ATOM.
        /// </summary>
        /// <returns></returns>
        string ToATOM();

        /// <summary>
        /// Deserializes to JSON.
        /// </summary>
        /// <returns></returns>
        string ToJSON();

        /// <summary>
        /// Deserializes to XML.
        /// </summary>
        /// <returns></returns>
        string ToXML();

        /// <summary>
        /// Deserializes to JSONP.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        string ToJSONP(string callback, params string[] args);

        /// <summary>
        /// Fills this instance. This is a contract for Command pattern.
        /// </summary>
        /// <param name="token">The widget token.</param>
        /// <param name="interfaceLanguage">The interface language.</param>
        void Fill(string token, string interfaceLanguage);
    }
}