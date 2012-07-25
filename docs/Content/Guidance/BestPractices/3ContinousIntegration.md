#### ALWAYS fix the build immediately after breaking it

Fixing a broken build is the entire team's top priority.
When you commit a change that breaks the build, drop everything you are doing and focus on checking in a fix to make the build (and the team) happy again.
Remember, when the build server cannot build the solution, neither can the rest of the team - and that means you are keeping everyone from working!

#### ALWAYS get latest and run all unit tests prior to **every check-in**

Please take the standard precautions of compiling and running the unit tests locally.
The following steps are required prior to any check-in.
If you follow these steps, your chances of breaking the build are drastically reduced.

1.  **Get latest source code from Source Control Explorer** <a href='#versioncontrol'>?</a> 

2.  Rebuild the entire solution.

3.  Execute the unit tests.

    -   [TestDriven.NET](http://testdriven.net/) is a great tool that provides the ability to run unit tests from a context menu pretty much anywhere in Visual Studio

    -   [jetBrains ReSharper](http://www.jetbrains.com/resharper/) users can right click on the `Solution` and select `Run Unit Tests`.

    -   Everyone else can use the Visual Studio test runner located in `Test > Run > All Tests in Solution` (or `Ctl-R, A`).

#### ALWAYS wait to see the results of your build

Immediately after committing, **DROP EVERYTHING AND WAIT FOR YOUR BUILD TO COMPLETE!**

To make sure your build is successful:

1.  Navigate to the `Builds` section in `Team Explorer` for the TFS Project you just committed code to

2.  Open the build named *Continuous*

3.  Switch to the *Queued* tab

4.  Wait for your build to "go green"

    -   Feel free to open the build and watch the log in real-time.

To be notified of the status of the builds for the entire team, be sure to run the `TFS Build Notifications system tray app system tray` application.

#### AVOID checking in on top of a broken build

The only check-ins that should occur once the build is broken are changesets that fix the issues that lead to the broken build.
If you have pending changes that will not fix the build, please wait to commit them until after the build is fixed.
If they must be committed immediately, offer to help fix the build!

Our current check in policy will trigger an error when you try to check in on top of broken build.
When policy exceptions occur, do not simply select "override".
Check-in policies are in place to encourage the proper processes, and overriding them is rarely the correct option: instead, focus on fixing the issue to make the policy exceptions go away.

#### AVOID breaking the build by using Shelvesets or working in a branch

If you intend to make major changes that may break the build or otherwise interrupt the workflow for other members on the team, please make a branch and proceed to break your branch all you like - branches are not included in the Continuous build.
Feel free to merge your branch back into the trunk whenever you feel it is stable enough to do so.
However, please note: working outside of the trunk might be a good short-term solution for some changes, but inevitably leads to trouble when the branch lives too long. So, try to keep your branches focused and short-lived.