The data model for this component is an object of type `MarketDataInstrumentIntradayResultSet`, which is a collection of `MarketDataInstrumentIntradayResult`.
Each `MarketDataInstrumentIntradayResult` contains market data for a company.

####MarketDataInstrumentIntradayResult
Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
requestedId						| Id of the company
symbol							| Market symbole of the company
fCode							| FCode of the company
currency						| Currency used.
isin							|
sedol							|
cusip							|
name							| Name of the company
low								| Object of type `DoubleNumberStock`.
high							| Object of type `DoubleNumberStock`.
previousClose					| Object of type `DoubleNumberStock`.
last							| Object of type `DoubleNumberStock`.
open							| Object of type `DoubleNumberStock`.
dataPoints						| Object of type `BasicDataPointCollection`, which is a collection of `BasicDataPoint` objects.
end								| Object of type `DateTime`.
start							| Object of type `DateTime`.
startDescripter					| String representation of start date.
stop							| Object of type `DateTime`.
stopDescripter					| String representation of stop date.
adjustedStart					| Object of type `DateTime`.
adjustedStartDescripter			| String representation of adjustedStart date.
adjustedStop					| Object of type `DateTime`.
adjustedStopDescripter			| String representation of adjustedStop date.
lastUpdated						| Object of type `DateTime`.
lastUpdatedDescripter			| String representation of lastUpdated date.
adjustedLastUpdated				| Object of type `DateTime`.
adjustedLastUpdatedDescripter	| String representation of adjustedLastUpdated date.
percentChange					| Object of type `PercentStock`
isIndex							|
provider						| Object of type `Provider`, identifying the data provider.
exchange						| Object of type Exchange.

####BasicDataPoint
Property 						| Description									
--------------------------------|----------------------------------------------------------------------------------------------
date							| Date of the data point.
dateDisplay						| String representation of the date value.
dataPoint						| Object of type `Number` that contains numeric value of the data point.
isLast							| Specifies if this data point is the last in the list.

####Provider
Property 						| Description									
--------------------------------|----------------------------------------------------------------------------------------------
name							| Name of the provider.
externalUrl						| Url of the provider's website.