@using DowJones.Documentation.Website.Extensions

Source code **branching** is the duplication of an object under source control (such as a source code file, or a directory tree) so that modifications can happen in parallel along both branches.  Branching is a powerful strategy that increases the stability of each individual branch and helps to mitigate the risks involved in supporting multiple parallel development efforts (such as "development"ù, "integration"ù and "production"ù; or, "development"ù, "1.1"ù, "2.1"ù, etc.).

Since branching involves maintaining multiple copies of a source-controlled item, it is quite easy for the copies to become out-of-sync with each other and lead to confusion about which is the latest and "most correct"ù.
To avoid this confusion, it's often a good idea for a team to agree on a particular set of conventions ñ known as a **branching strategy** ñ for dealing with branching source-controlled items in their project. 

Branching strategies should match the deployment scenario that they support.
Though teams are free to choose whichever branching strategies they like, there are several well-established strategies (listed below) that have the Platform Integration Team has found useful.

@Html.Important("Note that these strategies are not necessarily mutually-exclusive and some strategies may be used in tandem; for example, the Branch per Release and Branch per Feature strategies complement each other nicely.")

#### Branch per Feature
The **Branch per Feature** strategy is based a mainline folder (named **trunk** in the visual below) under which all active development occurs.
This mainline folder is then branched in preparation for a major "potentially disruptive "development effort to introduce a new feature or set of features.
When the feature is complete, the feature branch is then merged back into the **mainline**.

The primary goal of this strategy is to allow potentially breaking changes such as broken code or failing unit tests to be checked in while keeping the **mainline** relatively stable (i.e. passing continuous builds).
This approach also allows features to be developed separately from the mainline to ensure that they do not affect the team as a whole nor the project's delivery timeline; e.g. a feature branch may be excluded from an up-coming release simply by not merging it into the **mainline**.

	[Team Project]
		|---- trunk
 		|---- branches
 			|---- [New Feature A]
 			|---- [New Feature B]

Due to the overhead involved in keeping feature branches up-to-date with the **mainline** folder, they should be used very judiciously and be very short-lived, merged back into the **mainline** as soon as the feature is stable.
They should also be deleted immediately after being merged into the **mainline** so that every feature branch folder represents active development.

#### Branch per Release
The **Branch per Release** strategy also prescribes a mainline folder under which all active development occurs.
This mainline folder is then branched for each release of the project, with each release branch containing the code for that release.
The primary goal of this strategy is to have an exact snapshot of the code that is deployed for any active release to reduce the time and risk involved in researching, developing, and deploying production fixes.

	[Team Project]
 		|---- trunk
 		|---- releases
 				|---- v1
 					   |---- 1.1
 					   |---- 1.2
 					   |---- [Ö]
 				|---- v2
 					   |---- 2.1
 					   |---- 2.2
 					   |---- [Ö]
 
To enforce this strategy, the release process should consist of creating a new release branch, retrieving a local copy of that branch, and building the project from the source code in the release branch "**the released project should never be built from the mainline folder**.

Once a release is created, any "hot fixes"ù that cannot wait for the next full release should be made directly to the release branch, but these fixes should be kept to a minimum and merged back into the mainline folder immediately.

#### Branch per Release Environment
The **Branch per Release Environment** strategy also uses a mainline folder for active development.
Branches of this mainline folder are then created for each environment that the application will be deployed to (e.g. "integration"ù and "production"ù).

	[Team Project]
		|---- trunk
 		|---- releases
 			|---- integration
 			|---- production

This strategy is fundamentally the same as the **Branch per Release** strategy except that the definition of a "release"ù is slightly altered.  Also, instead of creating a new release branch every time the project is ready to be deployed, the code is simply tested and "promoted"ù (merged) from one environment (existing branch) to the next.

Here is an example of the process with two release environments "**integration** (a stable environment under which the application is verified) and **production** (the production environment):

1. Active development occurs in the mainline folder until the application is ready to be released
2. The entire mainline folder is "promoted"ù to the "integration"ù branch and deployed to the integration environment for further verification
	* Active development can continue on the **mainline** folder without affecting the stability of the integration branch.
3. The **integration** branch is tested and verified (i.e. by the QA team)
	* Any bug fixes or changes in response to the verification process may be made directly to the **integration** branch (and then merged into the **mainline** branch as appropriate)
4. Once the **integration** branch has been approved for release, it is promoted to the production branch off of which the production release is created.
5. When problems occur in the **production** environment, jump back to step #3.  
The problem is fixed in the **integration** branch, and then promoted to the **production** branch.

#### Shelvesets
Team Foundation server offers a somewhat unique concept of temporarily storing a set of changes on the server called [shelvesets](http://msdn.microsoft.com/en-us/library/ms181403(v=vs.80).aspx "shelvesets").
Shelvesets let you save your changes to the source control system without actually "committing"ù them to the project you're working on so that you can retrieve them at a later time.

There are five primary shelving scenarios:

* **Interrupt** When you have pending changes that are not ready for check in but you need to work on a different task, you can shelve your pending changes to set them aside.
* **Integration** When you have pending changes that are not ready for check in but you need to share them with another team member, you can shelve your pending changes and ask your team member to unshelve them.
* **Review** When you have pending changes that are ready for check-in and have to be code-reviewed, you can shelve your changes and inform the code reviewer of the shelveset.
* **Backup** When you have work in progress that you want to back up, but are not ready to check in, you can shelve your changes to have them preserved on the Team Foundation server.
* **Handoff** When you have work in progress that is to be completed by another team member, you can shelve your changes to make a handoff easier.

To shelve a pending source control change, simply right-click on the file(s), project, or solution in Visual Studio and select the "Shelve Pending Changes"ù menu option.
Then, give the shelveset a name so that you can easily find it later on.
The UI for shelving changes is similar to committing pending changes ñ simply include or exclude the files that you'd like to include in the shelveset.
Finally, you can mark the checkbox to choose to undo your local changes after they have been shelved, or leave them on your machine ñ this is helpful for when you would like to temporarily "move"ù your changes to the server while you work on something else.

Though not technically a branching strategy, you can think of shelvesets as a "pseudo-branching"ù tactic that allows you to take advantage of source control while avoiding introducing any breaking changes to the mainline branch.

@Html.Important(@"Shelveset management is completely ""manual""ù ñ shelvesets are not automatically removed just because you've unshelved them. So, be sure to delete shelvesets when you are done with them.")