#### Rendering the component via JavaScript

Add the following lines to render the Discovery Graph component via JavaScript:

	<div id="MyDiscovery" />
	
	<script type="text/javascript" src="[DJ Component Site]/common.js" />

Then, instantiate with a script tag:

	<script type="text/javascript" src="[DJ Component Site]/DiscoveryGraph.js?target=MyDiscovery" />

... or with the `DJ.add()` method:

	<script type="text/javascript">
	DJ.add("DiscoveryGraph", {
		target: "MyDiscovery"
	});
	</script>