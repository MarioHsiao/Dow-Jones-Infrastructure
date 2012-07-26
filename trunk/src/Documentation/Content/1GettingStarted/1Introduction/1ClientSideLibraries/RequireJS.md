#### RequireJs

##### Overview
When a project reaches a certain size, managing the script files for a project starts to get tricky. 
You need to be sure to sequence the scripts in the right order, and you need to start seriously thinking about combining scripts together into a bundle for deployment, so that only one or a very small number of requests are made to load the scripts.

##### Problem
* Web sites are turning into Web apps
* Code complexity grows as the site gets bigger
* Assembly gets harder
* Developer wants discrete JS files/modules
* Deployment wants optimized code in just one or a few HTTP calls

##### Solution
RequireJS can help you manage the script modules, load them in the right order, and make it easy to combine the scripts later via the RequireJS optimizer or other process without needing to change your markup. 
It also gives you an easy way to load scripts after the page has loaded, allowing you to spread out the download size over time.

RequireJS has a module system that lets you define well-scoped modules, but you do not have to follow that system to get the benefits of dependency management and build-time optimizations. 
Over time, if you start to create more modular code that needs to be reused in a few places, the module format for RequireJS makes it easy to write encapsulated code that can be loaded on the fly. 
It can grow with you, particularly if you want to incorporate internationalization (i18n) string bundles, to localize your project for different languages, or load some HTML strings and make sure those strings are available before executing code, or even use JSONP services as dependencies.
