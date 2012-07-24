@using DowJones.Documentation.Website.Extensions

The Team Foundation Server organizational structure consists of one to many "Collections" (of which we only have one – the "DefaultCollection"), each of which contains any number of "Team Projects" that contain all of the artifacts for a given project.

@Html.Important("Almost every artifact in TFS (e.g. source control, work items, build definitions) belongs to a single Team Project and cannot be shared with or referenced by other Team Projects.")

The Source Control Explorer hierarchy directly reflects this organizational structure; each Team Project has its own folder.
The file and folder structure within each of these folders is up to the team that owns it; however, it is highly recommended that all active development should be done in a mainline "trunk" folder from which one or more of the following branching strategies can be applied.