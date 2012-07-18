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
		DJ.add( [Component Name] , {
			container : [DOM ID or element],
			options: { [...] },
			callbacks: { [...] },
			data: [component data]
		}); 
	</script>
~~~~

While `container` can refer to any DOM element, each component has specific set of pre-defined options, callbacks and data structure. 
You can find the specific details of any component by navigating to [Components](components) and exploring individual components.


#### Client API Demo

@Html.DemoFrame(System.Configuration.ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"]+"/Home/ClientDemo")