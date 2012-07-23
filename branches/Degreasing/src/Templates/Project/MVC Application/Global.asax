<%--
    ***********  IMPORTANT  ***********
    In order to take advantage of the full Dow Jones framework, your application
    must inherit from DowJonesHttpApplication.  As you can see here, the recommended
    approach is to avoid using a code-behind to house Global application logic.
    As a best practice, logic in the Global/Application class should be kept to a
    minimum.

    In order to avoid placing logic in the Application class:
    
    *  For logic that must execute when an application starts up, create a
       bootstrapper task (implement the Dow Jones IBootstrapperTask interface).
       You need not do anything more than simply create it - the framework
       will scan your assemblies and automatically execute any IBootstrapper tasks
       it is able to locate.
    
    *  For logic that listens for events such as BeginRequest, BeginSession,
       EndRequest, EndSession, etc. consider if this logic might be best handled
       by a custom Action Filter.  If the Action Filter approach does not apply,
       consider creating an HTTP Module and registering it in the web.config.
    
    The above advice should handle a majority of common cases.  In some cases it 
    may still be prudent to add code to the Application class. You should, however 
    consider this approach only when none of the other approaches apply.

--%>

<%@ Application Inherits="DowJones.Web.Mvc.DowJonesHttpApplication" Language="C#" %>