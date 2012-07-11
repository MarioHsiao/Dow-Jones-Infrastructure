   Option						|  Description																												| 	Default						| Is Required?								
--------------------------------|--------------------------------------------------------------------------------------------------------------------------------------------------------------------------
suggestServiceUrl				| The service url to fetch the data.																						| None							| Yes
controlId						| The Control to which the autosuggest functionality to be attached.														| None							| Yes
controlClassName				| The custom class for the control.																							| None							| No
autocompletionType				| The autocompletion type of the widget.<code>Company</code>,<code>Executive</code>,<code>Source</code> etc					| None							| Yes								  
authType						| Authentication Type of the AutoSuggest. <code>SessionId</code>,<code>EncryptedToken</code>,<code>SuggestContext</code>	| None							| Yes
authTypeValue					| String representation of Authentication Type Value.																		| None							| Yes
resultsClass					| The css class which is applied to the autosuggest results container.														| dj_emg_autosuggest_results	| No
resultsOddClass					| The css class which is applied to the autosuggest odd results container.													| dj_emg_autosuggest_odd		| No
resultsEvenClass				| The css class which is applied to the autosuggest even results container.													| dj_emg_autosuggest_even		| No
resultsOverClass				| The css class which is applied to the autosuggest list item when it is hovered.											| dj_emg_autosuggest_over		| No
maxResults						| Maximum number of results to be displayed.																				| 10							| No
selectFirst						| Selects the first element on the list on key press																		| true							| No
fillInputOnKeyUpDown			| Selects the element and puts it into the input textbox as we traverse up & down.											| false							| No
showHelp						| Displays Help row on the top of the suggest list based on the value														| false							| No
helpLabelText					| The text to be displayed when help row is enabled																			| None							| No
showViewAll						| Displays View All row at the bottom of the suggest list based on the value												| false							| No
viewAllText						| The text to be displayed when help row is enabled																			| None							| No
serviceOptions					| Object consisting of options specific to the control type(Ex: Source, Executive etc).										| None							| No
tokens							| Tokens used to translate the text based on the language																	| None							| No
columns							| Columns to display in the suggest results		 																			| None							| No