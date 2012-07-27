THe .NET Gateway can use either "RTS" or "HTTP" as a transport method. If the Platform Infrastructure is installed locally, you can use "RTS" This is the default setting. IF not, you have to use "HTTP"

In order to use the .NET Gateway without the infrastructure installed locally, you have to update the configuration in the .NET Gateway.

Follow the below steps to update the configuration tyo use "HTTP" as the transport mode.

1. Open the "**Factiva.config**" file

	You will find this in the following directory under you project:

		\GatewayResources\Config\Factiva.config

2. You must change the value of the "preferredTransport" to **HTTP**

		<preferedTransport>HTTP</preferedTransport>

3. You also need to make sure the the "httpEndPoint" is configured properly to point to a valid URL for an instance of the HTTP Gateway. It should be:

		<httpEndPoint>http://utilities.int.dowjones.com/platformgateway/gatewayservice.asmx</httpEndPoint>
