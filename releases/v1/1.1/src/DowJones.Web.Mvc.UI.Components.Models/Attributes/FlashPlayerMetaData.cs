using System;

namespace DowJones.Web.Mvc.UI.Components.Attributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    internal class FlashPlayerMetaData : Attribute
    {
        private readonly int m_Width;
        private readonly int m_Height;
        private readonly string m_DewplayerVersion;
        private readonly string m_TargetFlashPlayerVersion;

        public string DewplayerVersion
        {
            get { return m_DewplayerVersion; }
        }

        public int Height
        {
            get { return m_Height; }
        }

        public int Width
        {
            get { return m_Width; }
        }

        public string TargetFlashPlayerVersion
        {
            get { return m_TargetFlashPlayerVersion; }
        }

        public FlashPlayerMetaData(int width, int height, string dewplayerVersion, string targetFlashPlayerVersion)
        {
            m_Width = width;
            m_Height = height;
            m_DewplayerVersion = dewplayerVersion;
            m_TargetFlashPlayerVersion = targetFlashPlayerVersion;
        }
    }
}
