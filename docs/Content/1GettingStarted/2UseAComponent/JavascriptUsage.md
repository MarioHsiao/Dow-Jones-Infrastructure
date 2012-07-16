If you are not using ASP.NET MVC as your server side framework, you can still consume the Dow Jones Components by rendering them client side, using standard HTML and JavaScript.

There are 3 basic steps to get this going:

* **Obtaining credentials:** You need to authenticate the user to obtain valid credentials. 
You then pass the valid credentials (session id or encrypted token) to `common.js` in order to start using the components as shown in the next step.
* **Referencing "common.js":** "common.js" brings down a set of core JavaScipt libraries that are essential for all Dow Jones Components. 
 To reference `common.js`, simply add a `<script>` tag in your HTML as shown below:

~~~~
	<script type="text/javascript" 
        src="http://<tbd>/common.js?sessionId=27137ZzZKJAUQT2CAAAGUAIAAAAANFOUAAAAAABSGAYTEMBWGI2TCNBQGYZTKNZS"></script>
~~~~

passing either a valid `sessionId` or `encryptedToken` as a query string parameter to authenticate the request.

* **Adding a component to the page:** You can add any component to your page and position it anywhere you like by defining a container element (such as `<div>`) and calling `DJ.add` as shown below:
		
~~~~
	<script type="text/javascript">
		DJ.add(<name_Of_Component>, {
			container : <DOM id of the container>,
			options: { ... },
			callbacks: { ... },
			data: <JSON data>
		}); 
	</script>
~~~~

While container can be any DOM element of your liking, each component has specific set of pre-defined options, callbacks and data structure. 
You can find the specific details of any component by navigating to [Components](components) and exploring individual components.