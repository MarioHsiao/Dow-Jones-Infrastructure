using DowJones.Json.Gateway.Interfaces;

namespace DowJones.Json.Gateway.Extensions
{
    public static class ControlDataExtensions
    {
        public static bool IsNotNullAndValid(this IControlData controlData)
        {
            return controlData != null && controlData.IsValid();
        }
    }
}