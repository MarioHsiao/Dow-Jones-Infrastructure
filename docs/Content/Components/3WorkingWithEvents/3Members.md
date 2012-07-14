The following table describes the public members available on DJ.Subscribe.

   Member						|  Arguments					|Description
--------------------------------|-------------------------------|-------------------------------------------------------------------------------------------------------------------
DJ.publish						| eventName,args                |Publishes to all events that match the eventName and passes parameters to the object. 
DJ.subscribe					| eventName,handlerFunction     |Subscribes to all events that match the eventName and calls the passed-in handlerFunction.
on	           					| eventName,handlerFunction     |Subscribes to all events that match the eventName for a specific instance and calls the passed-in handlerFunction.
