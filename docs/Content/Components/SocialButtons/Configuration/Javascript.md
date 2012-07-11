#### Rendering the component via JavaScript

Add a reference to `common.js` with a valid `sessionId` anywhere in the `<head>` section of you page.

	<script type="text/javascript" 
	     src="http://<tbd>/common.js?sessionId=27137ZzZKJAUQT2CAAAGUAIAAAAANFOUAAAAAABSGAYTEMBWGI2TCNBQGYZTKNZS"></script>

Add a container `<div>` to your page where you would like to display the component.

	<div id="SocialButtonsContainer"></div>

Finally, add the component to the page:

	<script type="text/javascript">
		DJ.add(DJ.UI.SocialButtonsComponent, {
			container : "SocialButtonsContainer",
			options: {
                    imageSize = 1,
					title = "Share",
					url = "http://www.dowjones.com",
					keywords = "Social",
					description = "Share Content",
					socialNetworks: ["Delicious","LinkedIn","Twitter","Technorati","Yahoo","StumbleUpon"]
            }
		}); 
	</script>	  
