The Headlines family of Widgets features a collection of controls that make it easy
to add Dow Jones headlines to a website or web application.  With the Headlines
Widget, you display a list of headlines populated by a search using the Factiva
Query Language or one of our editorially curated collections of articles. 
This powerful Widget is the most popular way to integrate Dow Jones news into your
site.  

To use the Headlines Widget, include the following source into your web page:

    <script type="text/javascript" src="http://widgets.dowjones.com/Widgets/2.0/common.js?sessionId=27137ZzZINHEQ42TAAAGUAIAAAAAFDAKAAAAAABSGAYTEMBVGIZDCMJTG4ZTOMBT"></script>

Next, place a container for your widget somewhere on your page. This container will
be where the widget is added after it has loaded
and should be placed where you want the widget to display.

    <div id="myDiv"></div>

Finally, include the widget script refrence as far down in your page as possible.
Be sure to reference the container ID specified 
in the previous step in the query string of the script reference.

    <script type="text/javascript" src="http://widgets.dowjones.com/Widgets/2.0/widget.js#w=headlines&container=myDiv&[ADDITIONAL PARAMETERS HERE]"></script>