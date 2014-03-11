using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Extentions
{
    public static class ControlDataExtensions
    {
        public static bool IsValid(this IControlData controlData)
        {
            return controlData != null;
        }
    }
}