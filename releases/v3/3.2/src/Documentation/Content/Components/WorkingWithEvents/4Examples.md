#### Subscribing to an Event via `DJ.subscribe`

In this example, the `_onItemClickedhandler` function will be fired every time the `itemClicked.dj.Component` event is raised by the component type.

    DJ.subscribe('itemClicked.dj.Component', _onItemClickedhandler);    // handle to a valid function

with inline handler:
    
    DJ.subscribe('itemClicked.dj.Component', function(data) {
        // handle events
    });     

This is the preferred mechanism to react to events as it provides a loose coupling between the event and your application code.
Further, if you have multiple instances of the same component on a page, it allows you to write your event handling code in one place instead of repeating it for every instance.

#### Publishing an Event via `DJ.publish`

In this example, the event `itemClicked.dj.Component` is published by the the component type with given `data`.

    DJ.publish('itemClicked.dj.Component', data);

#### Attaching to a specific Event and Component Instance

In this example, the `_onItemClickedhandler` function will be called every time the `itemClicked.dj.Component` event is raised by the _*specific*_ component instance.

    componentInstance.on('itemClicked.dj.Component', _onItemClickedhandler);

It is useful if you have multiple instances of the same component on a page and you need to react differently to each component.
This type of usage is rare and should be used sparingly.

#### Finding component instance

You can find a component instance by capturing the return value from `DJ.add`:

    var componentInstance = Dj.add(...);    

or by finding it in the DOM:

    var componentInstance = $('#componentContainer').findComponent(DJ.UI.Component);
    
    if(componentInstance) {
        // do stuff
    }