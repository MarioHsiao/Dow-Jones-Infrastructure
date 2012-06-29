#### Rendering the component from a Razor View

Populate the AutoSuggest model (either in your controller or model):

	var keywordSuggestModel = new AutoSuggestModel
            {
                SuggestServiceUrl = [suggestServiceURl],
                AutocompletionType = "Keyword",
                AuthType = "SuggestContext",
                AuthTypeValue = [authTypeToken],
                ControlId = "djKeywordAutoSuggest",
                FillInputOnKeyUpDown = true,
                SelectFirst = true
            };

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@Html.DJ().Render(keywordSuggestModel, new { id = keywordSuggestModel.ControlId })