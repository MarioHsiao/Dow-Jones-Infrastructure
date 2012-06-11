To use the Article Widget, include the following source into your web page:

	<!--Dependency - Place in the <head> tag--> 
	<script type="text/javascript" src="http://widgets.dowjones.com/Widgets/2.0/common.js?sessionId=27140ZzZINHEQT2TAAAGUBAAAAAA2HYUAAAAAABSGAYTEMBVGMYDAOJQGY2TGNBY"></script>

Next, place a container for your widget somewhere on your page. 
This container will be where the widget is added after it has loaded and should be placed where you want the widget to display.

	<!--Container - Place where you want the widget to display--> 
	<div id="myDiv"></div>

Finally, include the widget script refrence as far down in your page as possible. 
Be sure to reference the container ID specified in the previous step in the query string of the script reference.

	<!--Widget - Place as close to the closing body tag as possible --> 
	<script type="text/javascript" src="http://widgets.dowjones.com/Widgets/2.0/widget.js#w=article&container=myDiv&[ADDITIONAL PARAMETERS HERE]"></script>
