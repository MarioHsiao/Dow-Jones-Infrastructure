#### Rendering the component via JavaScript

Add a reference to `common.js` with a valid `sessionId` anywhere in the `<head>` section of you page.

	<script type="text/javascript" src="http://<tbd>/common.js"></script>

Add a container to your page where you would like to display the component.

	<div id="socialButtonsContainer"></div>

Finally, add the component to the page:

	<script type="text/javascript">
        DJ.add("SocialButtons", {
            container: "socialButtonsContainer",
            options: {
                imageSize: 1,
                title: "Share",
                url: "http://www.dowjones.com",
                keywords: "Social",
                description: "Share Content",
                socialNetworks: ["Delicious", "LinkedIn", "Twitter", "Technorati", "Yahoo", "StumbleUpon"]
            }
        });
	</script>	  
