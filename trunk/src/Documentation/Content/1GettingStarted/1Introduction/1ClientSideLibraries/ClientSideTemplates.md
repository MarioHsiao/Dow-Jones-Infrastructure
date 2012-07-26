The basic idea behind client side templating is to not only offload the server from rendering the template, but also to easily leverage existing web service APIs while un-cluttering JavaScript code with markup.

#### Picking a templating technology
It turns out that there are many client-side templating solutions to choose from. 
Some are based on John Resig's microtemplating (e.g. underscore.js), some are logic-less (e.g. mustache), some use a Haml syntax (e.g. Jade), and some are Java friendly (e.g. Google Closure).

#### Our solution
Currently, we use the doT.js templating engine in our component development enviroment. 
At runtime, we take these client side template files and **compile** them into a javascript function for use by the component.  
These functions hang off the templates object attached to the component.  

The user of the component will only see one file representing the component although, it can consist of various resources.
The main reason behind this approach is that our code does not become a dependency on any given sites ability to use another templating engine. 

The compilation process helps speed up the rendering of the html on client browsers as well allows the framework to handle caching and other script optmizations (such as combining and minification). 
This also makes the components agnostic of the underlying engine - as long as they adhere to the semantics, the engine can be freely swapped out for a better implementation without affecting the component's templates.

#### Conclusion
Today, javascript templates are one of the key components of our front-end infrastructure strategy: they enable browser/CDN template caching, they allow for a cleaner separation of presentation tier concerns, and at the same time, they enable greater unification of our front-end stacks across the company.