The following table lists data model for the component:

Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
authors[]						| String array representing the authors of the current headline.
codedArthors[]					| Collection or array of `Para` objects that represent authors that are associated with a code for database lookup.
baseLanguage					| String representing the base language code of the headline. Ex. **en** or **fr**.
baseLanguageDescriptor			| String representing the full qualified name of the language. Ex. **English** or **French**.
contentCategoryDescriptor		| String representing the full qualified name for the category of content that the headline falls into.
contentSubCategoryDescriptor	| String representing the full qualified name for the sub-category of content that the headline falls into.
hasPublicationTime				| Boolean, repesenting whether or not the headline has a publication time associated with it.
headlineUrl						| String representing a url associated with the headline for click-throughs.
mediaLength						| String representing the time span of a Multimedia based headline.
modificationDateDescriptor		| String representing the Date portion of when the headline was last-modified.
modificationDateTime			| String **\(ISO Formated\)** represeting the Date and Time of when the headline was last-modified.
modificationDateTimeDescriptor	| String representing the Date and Time of the headline was last-modified.  It will include Time if applicable.
modificationTimeDescriptor		| String representig the Time portion of when the headline was last-modified.
publicationDateDescriptor		| String representing the date portion of when the headline was last published.
publicationDateTime				| String **\(ISO Formated\)** representing the Date and Time of when the headline was published.
publicationDateTimeDescriptor	| String representing the Date and Time of when the headline was last published.  It will include Time if applicable.
publicationTimeDescriptor		| String representig the Time portion of when a headline was published.
reference{}						| Object that represnts the metadata associated with a headline. This is the object passed to the client from the `headlineClick.dj.PortalHeadlineList`.
snippets[]						| String array that represents the snippet associated with the headline.
sourceCode						| String representing the code of the source for the headline. Ex. **j**
sourceDescriptor				| String representing the full qualified name of the source for the headline. Ex. **The Wall Street Journal**
thumbnailImage					| Object represent the a thumbnail imaage that has been associated with the headline.
title							| String that represents the title of the headline.
toolTip							| String that represents a tooltip that is associated with the title.
truncatedTitle					| String that represents a trucated version of the title. 
wordCount{}						| Object that represents the number of words attached to the article view of the headline.
wordCountDescriptor				| String used to display the word count to the client. Ex. "1000 words"

The following table describes the options available for the `Para` item.

Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
items[]							| This array represents a list of items of type `MarkupItem`. `Para` object is used to render the list of coded authors.


The following table describes the options available for the `MarkupItem` collection.

Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
entityType						| Integer value of an Enumeration that denotes the type of entity. Enumerated values and descriptions are: `0:UnSpecified`, `1:Company`, `2:Person`, `3:Organization`, `4:Industry`, `5:NewsSubject`, `6:Region`, `7:City`, `8:Highlight`, `9:Textual`, `10:InsightIssue`, `11:InsightDiscovery`, `12:Author`
entityDescriptor				| String representing the descriptor portion of the for the entity. Ex. **Company** 
guid							| String that represents the code associated with the entity. Generally, this is a *FII (Factiva Intelligent Indexing)* code.  
value							| String that represents the text that should be displayed to the client.   

The following table describes the options available for the `reference` object.

Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
contentCategory					| Integer value of an Enumeration that represents the category of content that the headline falls under. Enumerated values and descriptions are: `0:UnSpecified`, `1:External`, `2:Publication`,`3:Website`, `4:Picture`, `5:Multimedia`, `6:Blog`, `7:Board`, `8:Internal`, `9:Summary`, `10:CustomerDoc`
contentCategoryDescriptor		| String representg the descriptor portion of the Enumeration for the category of content that the headline falls under.
contentSubCategory				| Integer value of an Enumeration that represents the sub-category of content that the headline falls under. Enumerated values and descriptions are `0:UnSpecified`,`1:Analyst`,`2:Blog`,`3:Newspaper`,`4:Audio`,`5:Video`, `6:PDF`, `7:HTML`, `8:Graphic`, `'9:Article`, `10:Rss`, `11:Atom`, `12:Multimedia`, `13:Board`, `14:Internal`, `15:Summary`, `16:CustomerDoc`, `17:File`, `18:WebPage`        
contentSubCategoryDescriptor	| String representg the descriptor portion of the Enumeration for the sub-category of content that the headline falls under. 
externalUri						| String representing the location of a Uri resource that represents the headline. 
guid							| String representing a unique identifier for the headline. *Ex. The accession number of a headline from the Factiva platform.* 
imageType						| String value needed to make a binary content call from the 'Factiva Platform'. 
mimetype						| String value needed to make a binary content call from the 'Factiva Platform'. 
ref								| String value needed to make a binary content call from the 'Factiva Platform'. 
subType							| String value needed to make a binary content call from the 'Factiva Platform'. 
type							| String value needed to make a binary content call from the 'Factiva Platform'. 

The following table describes the options available for the `wordCount` object.

Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
displayText						| Object representing the display that the client sees. 
value							| Number repesenting the word count for the headline. 

The following table describes the options available for the `displayText` object.

Property 						| Description											
--------------------------------|----------------------------------------------------------------------------------------------
value							| String representing the text that the customer sees when the word count is displayed to the client. 

The following table describes the options available for the `thumbnailImage` object.

Property 					| Description											
----------------------------|----------------------------------------------------------------------------------------------
guid						| String representing a unique identifier associate with the image. 
src							| String representing the URI location of the resource. *This is the primary one used if there is a value provided.  Otherwise, the URI propery will be used* 
uri							| String representing the original URI location of the image. 

