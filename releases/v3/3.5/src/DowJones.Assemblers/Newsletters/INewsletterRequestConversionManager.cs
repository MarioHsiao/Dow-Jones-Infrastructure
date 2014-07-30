using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DowJones.Assemblers.Workspaces;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;

namespace DowJones.Assemblers.Newsletters
{
    public interface INewsletterRequestConversionManager
    {
        UpdateWorkspaceRequest GetUpdateWorkspaceRequest(ManualWorkspace newsletterContent, WorkspaceRequestDto newsletterRequestDto, int maxHeadlinesInNewsletter);
    }
}
