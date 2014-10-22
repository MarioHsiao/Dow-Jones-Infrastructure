namespace EMG.widgets.ui.syndication.integration
{
    interface IWidgetPortalEndPoint
    {
        string MimeType { get; }
        string GetIntegrationCode();
    }
}
