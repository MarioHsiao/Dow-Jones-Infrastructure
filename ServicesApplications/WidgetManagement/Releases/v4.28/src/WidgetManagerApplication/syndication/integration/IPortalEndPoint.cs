namespace factiva.widgets.ui.syndication.integration
{
    interface IPortalEndPoint
    {
        string MimeType { get;}
        string GetIntegrationCode();
    }
}
