@using DowJones.Documentation.Website.Extensions
Before using TFS in Visual Studio you must first establish a connection to the TFS and tell Visual Studio which Team Projects you would like to track.  
@Html.Note("All configuration steps in this section should be executed within Visual Studio unless otherwise noted.")

#### Adding a Team Foundation Server connection

To connect to the Dow Jones Team Foundation Server within Visual Studio:

1. In Visual Studio, select the `Team > Connect to Team Foundation Server...` menu option
2. Click `Servers...` button, then the `Add...` button
3. Enter the following information to the **Add Team Foundation Server** dialog:

	           Name: sbknwstfs1.win.dowjones.net
	           Path: tfs  	[default]
        Port Number: 8080   [default]
	       Protocol: HTTP	[default]

4.	Click "Ok" to add the server, then "Close" to close the Servers dialog

Once you've added a connection to the Dow Jones Team Foundation Server, Visual Studio will retrieve a list of Team Projects that you have access to.
After the list has populated, select the Team Projects that you'll be working on and click the "Connect" button.

#### Mapping a Source Control Folder
The next step in the process of establishing a new Team Foundation Server environment is to get a local copy of the source code for the project you will be working on. 
You do this by "mapping" the source control repository to a local folder on your machine.

To map the source control repository, select the `View > Other Windows... > Source Control Explorer` menu item to open the "Source Control Explorer" tab.
Then, be sure that the root element (i.e. `sbknwstfs1\DefaultCollection`) is selected.

Any time a source control folder has not been mapped to a local folder this tab will display "Not mapped" as the folder's "Local Path".
This value is actually a link – to map the folder, simply click the "Not mapped" link.
Then, specify the local folder where you'd like to store your source controlled files (e.g. `C:\TFS`) and click "Map" to map the folder.
Visual Studio will then ask you whether you'd like to download all of the source code; we recommend that you do not download all of the source code at this point because it may take 30 minutes or more during which you will not be able to do anything else in Visual Studio.
Though you must download source code before you can change it, the source control repository contains a large number of files that you probably don't need in order to get started with your work. 
Instead, you can selectively download the folders that you need to work on immediately and retrieve the remaining files at some other time (before you leave for the night is a great time to do this!).

*[TFS]: Team Foundation Server 
*[QA]:  Quality Assurance