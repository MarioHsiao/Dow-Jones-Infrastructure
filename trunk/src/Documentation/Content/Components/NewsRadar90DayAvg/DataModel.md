The data model for this component is a collection of `EntityModel` object.

Each `EntityModel` represents a company.

####EntityModel
Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
companyName						| Name of the company
ownershipType					| Company's ownership type. Possible values: `0:Private`, `1:Public`
newsEntities					| Collection of `NewsEntityModel`, each representing company's news data for a news subject.
totalNewsItems					| Collection of `NewsVolumeModel` objects, each representing the total number of news hitcount for a `TimeFrame`
maxNewsItems					| Collection of `NewsVolumeModel` objects, each representing the highest number of news hitcount for a single company/subject for a `TimeFrame`
instrumentReference				| Object of type `InstrumentReferenceModel`
isNewsCoded						| Specifies if the company is news coded or not.
newsSearch						| Search string for the current company.

####NewsEntityModel
Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
entityType						| Entity type. The only expected value here is `0:NewsSubject`
newsVolumes						| Collection of `NewsVolumeModel` objects, each representing hit counts for the current company/news subject for a particlar `TimeFrame`
subjectCode						| Code of the news subject
radarSearchQuery				| Object of type `RadarSearchQueryModel` that represents the search string behind the current news entity.

####NewsVolumeModel
Property						| Description
--------------------------------|----------------------------------------------------------------------------------------------
timeFrame						| Time frame the current news volume applies to. Possible values: `0:Day`, `1:Week`, `2:TwoWeek`, `3:Month`, `4:TwoMonth`, `5:ThreeMonth`
hitCount						| Number of hitcounts for the specified time frame.

####RadarSearchQueryModel
Property						| Description
--------------------------------|----------------------------------------------------------------------------------------------
name							| Name of the search query
queryType						| Type of the query. The only possible value: `0:NewsSubject`
searchString					| Search string behind the query.
scope							| Scope of the query. The only possible value: `0:ns`
searchMode						| Query's search mode.

####InstrumentReferenceModel
Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
dunsNumber						| 
fcode							| 
type							| 
source							| 
ticker							| 
