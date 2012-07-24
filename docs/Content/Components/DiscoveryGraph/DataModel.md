The data model for this component is an object of type `DiscoveryGraphData`.

###DiscoveryGraphData
Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
discovery						| Object of type `DiscoveryGraphParentNewsEntities`

###DiscoveryGraphParentNewsEntities
Property 						| Description									
--------------------------------|----------------------------------------------------------------------------------------------
companyNewsEntities				| Object of type `ParentNewsEntity` that contains a collection of company related `NewsEntity` objects.
industryNewsEntities			| Object of type `ParentNewsEntity` that contains a collection of industry related `NewsEntity` objects.
personNewsEntities				| Object of type `ParentNewsEntity` that contains a collection of people related `NewsEntity` objects.
regionNewsEntities				| Object of type `ParentNewsEntity` that contains a collection of region related `NewsEntity` objects.
subjectNewsEntities				| Object of type `ParentNewsEntity` that contains a collection of subject related `NewsEntity` objects.

###ParentNewsEntity
Property						| Description
--------------------------------|----------------------------------------------------------------------------------------------
title							| Title of this entity group.
newsEntities					| Collection of `NewsEntity` objects, each containing number of hitcounts for a news entity

###NewsEntity
Property						| Description
--------------------------------|----------------------------------------------------------------------------------------------
code							| News entity's code.
currentTimeFrameNewsVolume		| News volume for the currently selected time frame.
descriptor						| News entity's descriptor.
type							| News entity's type. This value should match with the type of the ParentNewsEntity specified above.
typeDescriptor					| Descriptor of news entity's type.
