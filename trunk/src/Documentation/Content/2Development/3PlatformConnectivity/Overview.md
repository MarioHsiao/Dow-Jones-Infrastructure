@using DowJones.Documentation.Website.Extensions
Along with having the proper tools to do our development, we also need the ability to access the necessary data.
All of our data services are built to be accessed via our Platfrom Infrastrure.

This infrastructure is a message based infrastruture that consists of three (3) main components:

Component	| Description																											
------------|-------------------------------------------------------------------------------------------
XIPC		| Third-Party product the provides the message based transport mechanizm used by the infrastructure.
RTS			| Intally developed set of services that provide routing and message cleanup.
FCS			| Set of COM components used to send and recieve messages over the infrastructure.

@Html.Note("These are components for Windows based systems.")			

When making a transaction, all transactions either call FCS directly or through a utility layer.

Besides driect calls to FCS, we provide other method's for accessing back end services.

API				| Description																											
----------------|-------------------------------------------------------------------------------------------
FDK				| SOAP based .asmx web services used to access the back end services. It has both parser and object interfaces. FDK makes direct calls to FCS.
.NET Gateway	| A .NET library the provides access to the back end services via a defined object interface. The Gateway makes direct calls to FCS.
REST API		| ReST based WCF v4.0 web services used to access the back end services. Provides responses in both XML and JSON formats. MAkes calls to FCS via the .NET Gateway.

The .NET Gateway can be used whether the platform infrastructure is installed locally or not. If the platform infrastructure is installed, it will use the default transport mode of "RTS". This is the mode that is used in integration and production.

If the platform infrastructure is not installed locally, it can be confgured to use HTTP as a transport mode and all requests will be sent to a central set of servers that will process these meesages using the platform infrastructure. 
This is primarily used during development only.
