using DowJones.Utilities.Attributes;

namespace DowJones.Utilities.Core
{
    public enum InterfaceLanguage
    {
        /// <summary>
        /// Corresponds to English.
        /// </summary>
        [AssignedToken("langEn")]
        [AssignedString("English")]
        [ActiveItem]
        en = 0,
        /// <summary>
        /// Corresponds to German.
        /// </summary>
        [AssignedToken("langDe")]
        [AssignedString("Deutsch")]
        [ActiveItem]
        de,
        /// <summary>
        /// Corresponds to Spanish.
        /// </summary>
        [AssignedToken("langEs")]
        [AssignedString("Español")]
        [ActiveItem]
        es,
        /// <summary>
        /// Corresponds to French.
        /// </summary>
        [AssignedToken("langFr")]
        [AssignedString("Français")]
        [ActiveItem]
        fr,
        /// <summary>
        /// Corresponds to Russian.
        /// </summary>
        [AssignedToken("langRu")]
        [AssignedString("Русский")]
        [ActiveItem]
        ru,
        /// <summary>
        /// Corresponds to Italian.
        /// </summary>
        [AssignedToken("langIt")]
        [AssignedString("Italiano")]
        [ActiveItem]
        it,
        /// <summary>
        /// Corresponds to Japanese.
        /// </summary>
        [AssignedToken("langJa")]
        [AssignedString("日本語")]
        [ActiveItem]
        ja,
        /// <summary>
        /// Corresponds to Chinese-Simplified.
        /// </summary>
        [AssignedToken("langZhcn")]
        [AssignedString("中文 (简体)")]
        [ActiveItem]
        zhcn,
        /// <summary>
        /// Corresponds to Chinese-Taiwan.
        /// </summary>
        [AssignedToken("langZhtw")]
        [AssignedString("中文(繁體)")]
        [ActiveItem]
        zhtw,
        [AssignedToken("template")]
        [AssignedString("Template")]
        [ActiveItem]
        template,
    }
}
