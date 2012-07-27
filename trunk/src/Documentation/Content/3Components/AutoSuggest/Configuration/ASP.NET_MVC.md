
Populate the `AutoSuggest` model:

	var model = new AutoSuggestModel
            {
                SuggestServiceUrl = [suggestServiceURl],
                AutocompletionType = AutoCompletionType.Keyword,
                AuthType = AuthType.SuggestContext,
                AuthTypeValue = [authTypeToken],
                ControlId = "djKeywordAutoSuggest",
                FillInputOnKeyUpDown = true,
                SelectFirst = true
            };

Render the model in your view which will render the component in the browser:

	<!-- Render the component -->
	@@model DowJones.Web.Mvc.UI.Components.Models.AutoSuggestModel

	@@Html.DJ().Render(Model, new { id = Model.ControlId })