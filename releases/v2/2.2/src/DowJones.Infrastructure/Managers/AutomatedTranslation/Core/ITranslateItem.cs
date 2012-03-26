using System;
using DowJones.Globalization;

namespace DowJones.Managers.AutomatedTranslation.Core
{
    public interface ITranslateItem  : ICloneable
    {
        ContentLanguage IntoLanguage { get; }

        ContentLanguage GetLanguage();
        TextFormat GetFormat();
        string[] GetFragments();
        void SetFragments(string[] fragments);
    }
}
