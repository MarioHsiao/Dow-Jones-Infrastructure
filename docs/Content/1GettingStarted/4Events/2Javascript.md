####ATTACHING TO EVENTS WITH DJ.SUBSCRIBE####
----

####PUBLISH SUBSCRIBE (PUB/SUB) PATTERN####
----

DJ.subscribe is implemented using a Publish Subscribe Pattern, or Pub/Sub. The Pub/Sub pattern is used to logically decouple object(s) that generate an event, and object(s) that act on it.

**Advantages**

It is a useful pattern for object oriented development in general and especially useful when developing asynchronous JavaScript applications.
The ability to break down applications into smaller, more loosely coupled modules, which can also improve general manageability

####MEMBERS####
----

The following table describes the public members available on DJ.Subscribe.

   Member						|  Arguments					|Description
--------------------------------|-------------------------------|-------------------------------------------------------------------------------------------------------------------
DJ.publish						| eventName,args                |Publishes to all events that match the eventName and passes parameters to the object. 
DJ.subscribe					| eventName,handlerFunction     |Subscribes to all events that match the eventName and calls the passed-in handlerFunction.
on	           					| eventName,handlerFunction     |Subscribes to all events that match the eventName for a specific instance and calls the passed-in handlerFunction.

		
	

####EXAMPLES####
----

**Attaching to a specific Event and Component Type**

In this example, the _onItemClickedhandler function will be fired every time the itemClicked.dj.Component event is raised by a component type.

<pre><code>DJ.subscribe('itemClicked.dj.Component', _onItemClickedhandler);</code></pre> 

**Publishing to a specific Event and Component Type**

In this example, the event itemClicked.dj.Component is publised by the the component type.

<pre><code>DJ.publish('itemClicked.dj.Component', data);</code></pre> 

**Attaching to a specific Event and Component Instance**

In this example, the _onItemClickedhandler function will be fired every time the itemClicked.dj.Component event is raised by the specific component instance.

<pre><code>componentInstance.on('itemClicked.dj.Component', _onItemClickedhandler);</code></pre> 
 


####HANDLER FUNCTION####
----

The handler function passed to DJ.subscribe or on should be prepared to take the following arguments:

Argument	|Type			|Description
------------|---------------|--------------------------------------------------------------
data		|object			|Any data the event deems fit to send to the observing function.