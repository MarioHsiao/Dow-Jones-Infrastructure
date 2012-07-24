### ALWAYS `Get Latest` directly from Source Control Explorer

Visual Studio offers a convenient - and highly problematic - project
context menu item named `Get Latest...`. Visual Studio also offers the
`Check in...` menu item.

**DO NOT USE THESE FEATURES!**

Dow Jones solutions rely on many files and Visual Studio’s context menu
items are only aware of the files that are directly registered with the
solution and its projects. Thus, source control operations with these
menu items often create very costly oversights.

To avoid this, always be sure to perform your source control operations
directly against the `Source Control Explorer`.

### NEVER override check-in policy errors

The commit rules we have in place exist to ensure a consistent level of
code quality. **DO NOT ignore them!** Investigate each exception and
rectify it.

### ALWAYS provide a commit message

Commit messages help future developers (possibly yourself) identify the
purpose of a particular checkin and are invaluable.

**Please write meaningful commit messages - don’t just write garbage
text.**