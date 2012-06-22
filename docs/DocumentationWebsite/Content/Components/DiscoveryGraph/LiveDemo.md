<div class="showcase">
	Please wait while the demo loads...
</div>
<script type="text/javascript">
	$(function(){
	
		var iframe = document.createElement('iframe'),
			doc = iframe.contentWindow.document;

		iframe.class = "showcase async";

		doc.open().write('<body onload="this.src=\'@(System.Configuration.ConfigurationManager.AppSettings["InfrastructureShowcase.BasePath"])/DiscoveryGraph\'">');
		doc.close(); //iframe onload event happens
		$('#livedemo').find('div').append(iframe);

	});
</script>
