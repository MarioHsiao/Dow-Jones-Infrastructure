Along with having the proper tools to do our development, we also need the ability to access the necessary data.
All of our data services are built to be accessed via our Platfrom Infrastrure.

This infrastructure is a message based infrastruture that consists of three (3) main parts:

* XIPC
* RTS
* FCS

FCS is the ultimate entry point into this infrastrure. 
Build on top of these FCS transactions are a set of API's that provide access to the backend data.

FDK is a SOAP based API that can be used to access the data via calls to FCS transactions.

We also have a .NET assembly referred to as the Gateway that exposes all platform services through a set of objects that then serialize/deserialize messages sent through FCS to the platform services.
This is the preferred method of accessing data within an internall hosted Windows based application.
The Gateway can be used whether the platform infrastructure is installed or not. 
If the platform infrastructure is installed, it will use RTS as a transport mode. This is the mode that is used in integration and production.
If the platform infrastructure is not installed, it can be confgured to use HTTP as a transport mode and all requests will be sent to a central set of servers that will process these meesages using the platform infrastructure. 
This is primarily used during development only.
