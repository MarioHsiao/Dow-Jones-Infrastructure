@using DowJones.Documentation.Website.Extensions

The Dow Jones UI Component Library consists of the following assemblies:

- `DowJones.Web.Mvc.UI.Components`
- `DowJones.Web.Mvc.UI.Components.Models`

The Framework offers two primary types of components: "normal" UI Components, and [Composite Components](#djcompositecomponents).

#### Normal UI Components
Dow Jones UI Components interact with the DOM directly. 
They listen and react to DOM events such as clicks and mouse-overs.
The DOM event handler listens to an event and then _publishes_ the event to its container with various arguments. 

The arguments usually include:

- name of the event (along with its namespace)
- data associated with the event (e.g. in case of a headline click, accession number of the headline)
- any other optional attributes or data that might be needed higher up (in the container or page level handler).

When an event is published by a component, it is captured by its owner, if defined. 
If no owner is defined, it simply bubbles up to the page level.

Each UI Component consists of the following parts:

- A View (using the Razor view syntax)
- A Model, which includes:
	- Client Side Options
	- Client Side Data
	- Methods and business logic that provide a calculated value that can be used in the View
- Client-side logic (i.e. a JavaScript file)
- One or more Client Templates (optional, but recommended)

##### Notable Components
Some of the notable components are:

- HeadlineList
- PortalHeadlineList
- Article
- HeadlineCarousel
- HeadlinePostProcessing
- RegionalMap
- Radar
- NewsChart
- NavBar
- Menu
- CalloutPopup
- SearchBuilder
- ... and many more!



##### Restrictions
- UI components do not fetch their own data; rather, it is provided data from an outside source (such as its parent Composite Component).
- UI components do not include any CSS styling -- they must be styled and branded as per each application's requirements.
- "Normal" UI components do not contain other UI components; components that do so are considered *Composite Components*