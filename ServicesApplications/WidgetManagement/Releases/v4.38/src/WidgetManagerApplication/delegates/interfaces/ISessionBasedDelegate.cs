using System;
using System.Collections.Generic;
using System.Text;
using EMG.widgets.ui.dto;

namespace EMG.widgets.ui.delegates.interfaces
{
    interface ISessionBasedDelegate
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
        void Fill();
    }
}
