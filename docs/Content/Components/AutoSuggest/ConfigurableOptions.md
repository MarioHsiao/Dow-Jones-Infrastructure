The following table lists out the configurable options (Properties in ASP.NET MVC and Options in Javascript):

Properties			 |   Options 			 |  Description																								
---------------------|-----------------------|--------------------------------------------------------------------------------------
SuggestServiceUrl*	 | suggestServiceUrl*	 | The service url to fetch the data.													
ControlId*			 | controlId*			 | The Control to which the autosuggest functionality to be attached.					
ControlClassName	 | controlClassName		 | The custom class for the control.													
AutocompletionType*	 | autocompletionType*	 | The autocompletion type of the widget.`Company`,`Executive`,`Source` etc.							  
AuthType*			 | authType*			 | Authentication Type of the AutoSuggest. `SessionId`,`EncryptedToken`,`SuggestContext`. 
AuthTypeValue*		 | authTypeValue*		 | String representation of Authentication Type Value.									 
ResultsClass		 | resultsClass			 | The css class which is applied to the autosuggest results container.	<span class="label">Default: dj_emg_autosuggest_results</span>			
ResultsOddClass		 | resultsOddClass		 | The css class which is applied to the autosuggest odd results container.	<span class="label">Default: dj_emg_autosuggest_odd</span>			
ResultsEvenClass	 | resultsEvenClass		 | The css class which is applied to the autosuggest even results container. <span class="label">Default: dj_emg_autosuggest_even</span>		
ResultsOverClass	 | resultsOverClass		 | The css class which is applied to the autosuggest list item when it is hovered. <span class="label">Default: dj_emg_autosuggest_over</span>	
MaxResults			 | maxResults			 | Maximum number of results to be displayed. <span class="label">Default: 10</span>													
SelectFirst			 | selectFirst			 | Selects the first element on the list on key press <span class="label">Default: true</span>											
FillInputOnKeyUpDown | fillInputOnKeyUpDown	 | Selects the element and puts it into the input textbox as we traverse up & down. <span class="label">Default: false</span>			
ShowHelp			 | showHelp				 | Displays Help row on the top of the suggest list based on the value. <span class="label">Default: false</span>						
HelpLabelText		 | helpLabelText		 | The text to be displayed when help row is enabled. 				
ShowViewAll			 | showViewAll			 | Displays View All row at the bottom of the suggest list based on the value. <span class="label">Default: false</span>				
ViewAllText			 | viewAllText			 | The text to be displayed when help row is enabled.									 
ServiceOptions		 | serviceOptions		 | Object consisting of options specific to the control type (E.g: Source, Executive etc). 
Tokens				 | tokens				 | Tokens used to translate the text based on the language.								 
Columns				 | columns				 | Columns to display in the suggest results.		 									 

<span class="atn">* : Required property/option</span>