using System;
using DowJones.Utilities.Core;

namespace DowJones.Utilities.Managers.AutomatedTranslation.Core
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
