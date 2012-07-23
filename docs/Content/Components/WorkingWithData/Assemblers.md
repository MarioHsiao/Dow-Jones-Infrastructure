Assemblers are pre defined converter classes provided by the infrastructure framework.
The main purpose of Assemblers is to provide an interface that allows data from a Factiva (Gateway) response to be mapped to components model.
Some of the infrastructure components use assemblers to convert and map `gateway` response into the format the the model is expecting.
The developers who consume the infrastructure components has complete freedom to write their own mapper functions instead of using default Assemblers.

The following table list the assemblers used by the infrastructure components. 

**Components: `PortalHeadlineList`, `HeadlineList`, `RealtimeHeadlineList`, `HeadlineCarousel`**

**Namespace: `DowJones.Assemblers.Headlines`**

Name												| Initial DataSource			| Target Data Model											
----------------------------------------------------|-------------------------------|----------------------------------------------
AbstractHeadlineListDataResultSetConverter			| 								|
AccessionNumberSearchResponseConverter				|  								|
AtomFeedConverter									|  								|
CreateSharedAlertResponseConverter					|  								|
GatewayAccessionNumberSearchResponseConverter		|  								| 								
GetSharedAlertContentResponseConverter				|  								|
GetTriggerDetailsResultConverter					|  								|
HeadlineListConversionManager 						|  								|
HeadlineUtility										| 								|
IExtendedListDataResultConverter					| 								|
PCMAccessionNumberSearchResponseConverter			| 								|
PerformContentSearchResponseConverter				| 								|
PortalHeadlineConversionManager						| 								|
RealtimeHeadlinelistConversionManager				| 								|
RealtimeHeadlineListManager							| 								|
RssAtomSyndicationManager							| 								|
RssFeedConverter									| 								|
SyndicationHeadlineResponseConverter				| 								|
