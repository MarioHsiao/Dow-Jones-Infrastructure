@using DowJones.Documentation.Website.Extensions

Because build scripts may execute just about any logic you can imagine, it's important to define a scope for each build that determines what, exactly, the build expects to accomplish.

For example, TFS offers you the following types of automated builds:

Continuous Build
:	**Continuous builds** are triggered whenever any team member commits a change to the code base and their primary purpose is to provide near-immediate feedback about the quality of the change that was committed. 
    In order to validate quality, the tasks that a continuous build executes are usually restricted to compiling the application and running a suite of unit tests that provide a baseline verification that the code works and complete in a short amount of time.
	Because of the frequency at which continuous builds are triggered, it is crucial that these types of builds finish as quickly as possible in order to both tighten feedback loop and avoid multiple builds stacking up on one another.

Rolling Build
:	**Rolling builds** are just like continuous builds, except that they impose limits as to how many builds may execute within a certain time-frame.
	For instance, you may configure a rolling build to execute only once every 5 minutes rather than every time someone commits a change.
	As developers commit changes within that 5 minutes, those changes simply accumulate until the 5 minutes elapses and the next build executes.

Gated Check-In
:	**Gated Check-in** builds are much like continuous builds, but rather than raising a red flag when someone commits a breaking change, gated check-ins serve to disallow the breaking commit to even reach the codebase. 
	Gated check-ins may execute once per commit (like continuous builds), or may have a limit to how many times they can run in a given timespan (as with rolling builds).

Scheduled Builds
:	**Scheduled builds** execute on a specific schedule, and are not explicitly tied to commit activity. 
	The most popular example of scheduled build approach is known as a "**nightly build**" because it is scheduled to run at the same time every night, after the development team is done working for the day.
	Since these types of builds are not directly tied to commit activity, it is generally more acceptable for them to be somewhat out-of-date and not reflect the most up-to-date code in the codebase.
	
	Scheduled builds are also able to take more time to execute, perhaps executing more in-depth automated tests or creating artifacts such as installation packages that should only be produced in limited quantities.

The primary theme among the various types of automated builds is how often they execute, and how long each build execution takes to finish.
The work each type of build performs is an extension of this: as builds become less frequent, they have more time to accomplish their tasks and thus can perform a larger number of increasingly complex and time-consuming tasks.

For instance, continuous builds focus on performing the minimum amount of work in order to verify the on-going quality of the codebase, while at the other end of the spectrum, nightly or weekly builds may take hours or even days to perform massive tasks such as executing an extensive suite of in-depth automated tests, compiling large amounts of documentation, or packaging a product suite for release.

The best approach to build automation typically includes a few different types of builds operating at the same time, each with different priorities.  

For instance, you might consider implementing three different builds for the same application:

1. a continuous build to validate the quality of every check-in
2. a rolling build that occurs no more than every hour and executes more detailed automated tests but takes a while to do so
3. a nightly build that publishes the day's changes to a test website so that users or a QA team can track progress and report bugs as early as possible.