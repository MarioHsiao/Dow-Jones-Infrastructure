@using DowJones.Documentation.Website.Extensions;
Assemblers/TypeMappers are helper classes provided by the infrastructure framework.

The main purpose of Assemblers/TypeMappers is to provide an interface that allows data from one source to be mapped to a component's model.
Some of the infrastructure components use assemblers to convert and map `Fativa Platform *Gateway*` response into the format the the model is expecting.

Assemblers are meant to be mapper classes that have business logic applied when converting from one type to another.  While, TypeMappers tend to have provide a more one-to-one object mapping from the source object to the target object.

@Html.Note("Developers have the freedom to write their own mapper functions instead of using default Assemblers.")


The following table list the assemblers that can be used infrastructure components. 

**Component: `PortalHeadlineList`**

**Namespace: `DowJones.Assemblers.Headlines`**

Source Data													 | Assembler/TypeMappers									| Output Type										
-------------------------------------------------------------|----------------------------------------------------------|-----------------------------------------------
HeadlineListDataResult **`DowJones.Ajax.HeadlineList`**  | PortalHeadlineConversionManager							| PortalHeadlineListDataResult
RSS: **`File, URI`**										 | HeadlineListConversionManager							| HeadlineListDataResult
Atom: **`File, URI`**										 | HeadlineListConversionManager	  						| HeadlineListDataResult
Factiva Platform: **`Search, Alerts`**					 | HeadlineListConversionManager						 	| HeadlineListDataResult
Factiva Platform: **`Signals or Triggers`**				 | HeadlineListConversionManager						 	| HeadlineListDataResult
Factiva Platform: **`PCM AccessionNumber Collection`**	 | HeadlineListConversionManager						 	| HeadlineListDataResult
Factiva Platform: **`PCM Syndication Services`**			 | HeadlineListConversionManager						 	| HeadlineListDataResult
Factiva Platform: **`Realtime Alerts`**					 | RealtimeHeadlinelistConversionManager				 	| HeadlineListDataResult


**Component: `Article`**

**Namespace: `DowJones.Assemblers.Articles`**

Source Data													 | Assembler/TypeMappers									| Output Type										
-------------------------------------------------------------|----------------------------------------------------------|-----------------------------------------------
Factiva Platform: **`Archive`**							 | AriticleConversionManager								| ArticleResultset 		

**Component: `Stock Kiosk`**

**Namespace: `DowJones.Assemblers.Charting.MarketData`**

Source Data													 | Assembler/TypeMappers									| Output Type										
-------------------------------------------------------------|----------------------------------------------------------|-----------------------------------------------
`Dylan` Symbology Services	via Infrastructure MarketDataChartingManager Service								 | MarketDataInstrumentIntradayResultSetAssembler			| MarketDataInstrumentIntradayResultSet 		

@Html.Note("Infrastructure provides a service that connects to `Dylan Symbology` services.")
