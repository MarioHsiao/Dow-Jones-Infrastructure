   Option						|  Description																				| 	Default						| Is Required?								
--------------------------------|-------------------------------------------------------------------------------------------------------------------------------------
SuggestServiceUrl				| The service url to fetch the data.														| None							| Yes
ControlId						| The Control to which the autosuggest functionality to be attached.						| None							| Yes
ControlClassName				| The custom class for the control.															| None							| No
AutocompletionType				| The autocompletion type of the widget (Ex: Executive, Categories, Keyword etc)			| None							| Yes
UseSuggestContext				| Authentication Token which is used to send the request.									| None							| Yes
UseEncryptedKey					| Encrypted which is required to generate the authentication token.							| None							| Yes
UseSessionId					| SessionId which is required to generate the authentication token.							| None							| Yes
ResultsClass					| The css class which is applied to the autosuggest results container.						| dj_emg_autosuggest_results	| No
ResultsOddClass					| The css class which is applied to the autosuggest odd results container.					| dj_emg_autosuggest_odd		| No
ResultsEvenClass				| The css class which is applied to the autosuggest even results container.					| dj_emg_autosuggest_even		| No
ResultsOverClass				| The css class which is applied to the autosuggest list item when it is hovered.			| dj_emg_autosuggest_over		| No
MaxResults						| Maximum number of results to be displayed.												| 10							| No
SelectFirst						| Selects the first element on the list on key press										| true							| No
FillInputOnKeyUpDown			| Selects the element and puts it into the input textbox as we traverse up & down.			| false							| No
ShowHelp						| Displays Help row on the top of the suggest list based on the value						| false							| No
HelpLabelText					| The text to be displayed when help row is enabled											| None							| No
ShowViewAll						| Displays View All row at the bottom of the suggest list based on the value				| false							| No
ViewAllText						| The text to be displayed when help row is enabled											| None							| No
ServiceOptions					| Object consisting of options specific to the control type(Ex: Source, Executive etc).		| None							| No
Tokens							| Tokens used to translate the text based on the language									| None							| No
Columns							| Columns to display in the suggest results													| None							| No