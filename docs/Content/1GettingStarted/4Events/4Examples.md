**Attaching to a specific Event and Component Type**

In this example, the _onItemClickedhandler function will be fired every time the itemClicked.dj.Component event is raised by a component type.

<pre><code>DJ.subscribe('itemClicked.dj.Component', _onItemClickedhandler);</code></pre> 

**Publishing to a specific Event and Component Type**

In this example, the event itemClicked.dj.Component is publised by the the component type.

<pre><code>DJ.publish('itemClicked.dj.Component', data);</code></pre> 

**Attaching to a specific Event and Component Instance**

In this example, the _onItemClickedhandler function will be fired every time the itemClicked.dj.Component event is raised by the specific component instance.

<pre><code>componentInstance.on('itemClicked.dj.Component', _onItemClickedhandler);</code></pre> 
 
