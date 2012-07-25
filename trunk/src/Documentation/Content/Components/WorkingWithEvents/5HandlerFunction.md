`handlerFunction` must be a handle to a valid function in the page scope or it can be an inline function.
Passing an invalid hanlde will not raise any errors, although you can see a warning in the developer console.

The handler function passed to `DJ.subscribe` or `.on` should be prepared to take the following argument(s):

Argument	|Type			|Description
------------|---------------|--------------------------------------------------------------
data		|object			|Any data the event deems fit to send to the observing function.