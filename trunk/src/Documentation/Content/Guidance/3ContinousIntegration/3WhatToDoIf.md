﻿#### I broke the build

DROP EVERYTHING AND FIX IT IMMEDIATELY!

When you check in a changeset that triggers a broken build, your top (ONLY) priority is getting that build fixed, no matter how you do it!
The easiest way to fix a broken build is to simply revert your changeset - if it was working before your check in, undoing your check in should get it back to a working state. 
However, broken builds are often caused by simple things like forgetting to check in a file so reverting the entire changeset is probably overkill.

The more reasonable steps for fixing a broken build are:

1.  Review the build log. Every build has an associated log which should plainly show what failed.

2.  **Get the latest source code**. 
    The most common reason for build failures is that the developer did not have the latest source when he/she compiled and checked in. 
    You should have already gotten the latest source as part of the Check In Process but either way, get it again!

3.  Make sure everything compiles and executes properly on your local machine (after getting the latest source code).

4.  If the problem is simple (a missing file or similar), attempt a small, targeted check in to resolve it.

5.  Get the latest source code via `Get Specific Version`  
    
	If the problem persists yet you cannot reproduce the issue locally, your local copy may be out of sync and TFS might not know it. 
	Get the latest source code, but instead of using the `Get Latest` command, use the `Get Specific Version` and select the `Overwrite local files` option. 
	This will ensure that your local source is in sync with the server. 
	Then, go back to step 3.
    Rollback your previous changes.  
    
	If you have committed another check-in to fix the issue but it still persists then it will probably take some time to resolve it. 
	Do your coworkers a favor and revert the previous changes that broke the build in the first place. 
	Then, replay them and solve the issues locally. 
	This way your issues are not blocking anyone else.

#### The build breaks and I don't know why

Every build produces build logs that help determine the source of the failure. 
To view the log, open the build in `Build Explorer`, then locate and click the link to the build log.

If the source of the failure is not obvious (i.e. compilation errors or test failures), try the following:

1.  Perform a full build by disabling *IncrementalGet* and set *CleanOutputs* to `All`.  
    
	Most of our builds are "incremental builds" meaning that for every build only the changes are retrieved as opposed to downloading the full source tree every time. 
	This generally works except in rare circumstances where the build's local source gets out of sync with the server's. 
	A Full Build generally resolves these issues.
2.  Revert your last checkin  

    If the everything built fine prior to your checkin and now it's broken (and it's not an issue with the build server), the likely cause is that your last checkin broke it! 
	Try to revert your last checkin and see if the build succeeds. 
	If this works, feel free try your change again and if it fails a second time, there is something wrong with your checkin.

If the issue continues, please contact the TFS Project's build administrator.