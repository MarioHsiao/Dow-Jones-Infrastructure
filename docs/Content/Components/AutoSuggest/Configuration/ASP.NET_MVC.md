
Populate the `AutoSuggest` model:

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
	@@model keywordSuggestModel

	@@Html.DJ().Render(keywordSuggestModel, new { id = keywordSuggestModel.ControlId })