The following table describes the public members available for working with Events.

   Member	 |  Arguments				  |Description
-------------|----------------------------|-------------------------------------------------------------------------------------------------------------------
DJ.publish	 | eventName, args            |Publishes the named event with the given paramaters. 
DJ.subscribe | eventName, handlerFunction |Subscribes to named event via the passed-in `handlerFunction`. 
on	         | eventName, handlerFunction |Subscribes to named event via the passed-in `handlerFunction` for a ***specific*** instance of a component.
