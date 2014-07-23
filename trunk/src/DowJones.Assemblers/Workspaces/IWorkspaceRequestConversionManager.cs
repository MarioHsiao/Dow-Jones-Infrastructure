using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Factiva.Gateway.Messages.Assets.Workspaces.V2_0;

namespace DowJones.Assemblers.Workspaces
{
    public interface IWorkspaceRequestConversionManager
    {
        AddItemsToWorkspaceRequest AddWorkspaceItem(AutomaticWorkspace workspaceContent, WorkspaceRequestDto workspaceRequestDto, int maxHeadlinesInWorkspace);
    }
}
