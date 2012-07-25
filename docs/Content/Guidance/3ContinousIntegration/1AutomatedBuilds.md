@using DowJones.Documentation.Website.Extensions

We have automated builds configured on the project trunk which compile the solution and execute the unit tests on every checkin.   
As such, any changes you check in have the potential to "break the build", which means that no one on the team can continue working!

@Html.Note("See <a href='#bestpractices'>Best Practises</a> for tips on how to avoid breaking the build.")

#### The TFS Build Notifications system tray app

Unfortunately, we do not have email-based build notifications configured right now so until we do, it is your responsibility to make sure that your commits do not break the build.
The easiest way to watch the current build status is using the TFS Build Notifications tool (the image below shows where it resides in my Start menu).

<img src="@Url.Content("~/Content/images/CI_BuildNotificationsSystemTray.png")" alt="Build Notification System Tray">

Configuration is a breeze: simply select the builds you wish to observe.
Please watch the Continuous build at the very least.

<img src="@Url.Content("~/Content/images/CI_BuildNotificationOptions.png")" alt="Build Notification Options">

