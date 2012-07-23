Assemblers are pre defined converter classes provided by the infrastructure framework.
The main purpose of Assemblers is to provide an interface that allows data from a Factiva (Gateway) response to be mapped to components model.
Some of the infrastructure components use assemblers to convert and map `gateway` response into the format the the model is expecting.
The developers who consume the infrastructure components has complete freedom to write their own mapper functions instead of using default Assemblers.

The following table list the assemblers used by the infrastructure components. 

**Component: `PortalHeadlineList`**

**Namespace: `DowJones.Assemblers.Headlines`**

Name												| Initial DataSource									| Target Data Model											
----------------------------------------------------|-------------------------------------------------------|----------------------------------------------
HeadlineListConversionManager						|														|  		
AtomFeedConverter									| Atom based Data Source [File, URI]					| HeadlineListDataResult
CreateSharedAlertResponseConverter					|  														| HeadlineListDataResult
GatewayAccessionNumberSearchResponseConverter		|  														| HeadlineListDataResult						
GetSharedAlertContentResponseConverter				| Factiva Platform: *Realtime Alerts initial call*		| HeadlineListDataResult
GetTriggerDetailsResultConverter					| Factiva Platform: *Signals or Triggers*				| HeadlineListDataResult
PCMAccessionNumberSearchResponseConverter			| 														| HeadlineListDataResult
PerformContentSearchResponseConverter				| Factiva Platform: *Search*							| HeadlineListDataResult
RealtimeHeadlinelistConversionManager				| Factiva Platform: *Realtime Alerts subsequent calls*	| HeadlineListDataResult
RealtimeHeadlineListManager							| Factiva Platform: 									| HeadlineListDataResult
RssAtomSyndicationManager							| Factiva Platform:										| HeadlineListDataResult
RssFeedConverter									| RSS based Data Source [File, URI]						| HeadlineListDataResult
SyndicationHeadlineResponseConverter				| Factiva Platform:										| HeadlineListDataResult													 
PortalHeadlineConversionManager						| HeadlineListDataResult [Infrastructure class]			| PortalHeadlineListDataResult


**Component: `Article`**

**Namespace: `DowJones.Assemblers.Articles`**

Name												| Initial DataSource									| Target Data Model											
----------------------------------------------------|-------------------------------------------------------|----------------------------------------------
ArticleConversionManager							| Factiva Platform (Archive)							| ArticleResultset 		
