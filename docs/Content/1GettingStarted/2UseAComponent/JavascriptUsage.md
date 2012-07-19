@using DowJones.Documentation.Website.Extensions
Dow Jones Components may also be rendered using standard HTML and JavaScript directly in the browser.

There are 2 steps to add a component to your web page via the client-side API:

* **Reference "common.js":** "common.js" brings down a set of core JavaScipt libraries that are essential for all Dow Jones Components. 
 To reference `common.js`, simply add a `<script>` tag in your HTML as shown below:

~~~~
<script type="text/javascript" src="http://<tbd>/common.js"></script>
~~~~

* **Add the component:** You can add a component to your page and position it anywhere you like by defining a container element (such as `<div>`) and calling `DJ.add` as shown below:
		
~~~~
<script type="text/javascript">
	DJ.add( "[ComponentName]" , {
		container : [DOM ID or element],
		options: { ... },
		eventHandlers: { "eventName": handler[, ...] },
		data: [component data]
	}); 
</script>
~~~~

The following table explains the parameters.

Parameter            | Description
---------------------|--------------------------
ComponentName        | Component name with optional namespace. If no namespace is specified, "DJ.UI" is assumed.
container            | A DOM ID of a element on the page or the DOM element itself where the component will be rendered.
options				 | An object with a list of options for the component. See the component page for specific examples.
eventHandlers		 | An object with a list of event names and corresponding handler function. See the component page for specific examples.<br/><span class="label">Note: </span> <span class="comment"> Handlers attached this way, are tied to the **specific instance** of the component. For a loosely coupled way of subscribing to events, use **DJ.subscribe** instead. See [Working With Events](Components/WorkingWithEvents) for details.</span>
data				 | JSON representation of the component's model. See the component page for specific examples.

#### Client API Demo

@Html.DemoFrame(System.Configuration.ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/Home/ClientDemo")

#### Promise API
Often times, it is desirable to be notified when the component is actually loaded, after calling `DJ.add`, or in case of any failures so that you can take necessary remedial actions.
`DJ.add` returns a [jQuery promise](http://api.jquery.com/promise/) object that can be used to get notifications about various stages of `DJ.add`.

The following snippet shows an example of wiring up a component load success handler and failure handler:

~~~~
<script type="text/javascript">
	DJ.add( "[Component Name]" , { ... })
	  .done(function(instance){
		  alert('Component loaded: ' + instance.name);	// "instance" refers to the component's instance
	  })
	  .fail(function(err){
		  alert('Error occurred while loading component'); // err is a object containing errors
	  }); 
</script>
~~~~