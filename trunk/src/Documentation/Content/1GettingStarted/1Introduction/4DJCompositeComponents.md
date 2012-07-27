@using DowJones.Documentation.Website.Extensions
Composite Components are "normal" UI components that are made up of two or more UI Components.

####Key features of Composite Components are:

- They act as a scope container for the events fired by individual components. 	Each of the composite component or its derived classes contain an instance of `PubSubManager` which acts as a component event sink and an outlet.
- They act as a scope container for retriving data via AJAX methodologies. Can connect to data sources using `DJ.crossDomain` or the `jQuery.Ajax` client-side calls.
- They contain specific well-defined business logic and interaction models.

@Html.Note("<p>It is worth noting that two or more composite components can talk to each other, again via PubSub model. </p><p>Also, a composite component can have another composite component as its child.</p><p><code>SearchResults</code> component is a classic example of this. It is composite component which houses 3 other composite components as well as some UI components.</p>")