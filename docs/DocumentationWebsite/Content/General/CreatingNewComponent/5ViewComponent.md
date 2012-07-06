In project `DowJones.Web.Mvc.UI.Components`

* Under folder `SampleComponent`, create a new file `SampleComponent.cshtml`

* Go to properties of newly created file, set Custom Tool field to: `RazorViewComponentClassGenerator`. This should create the following file:
	* SampleComponent.cs '(if you do not see this file, you have not properly installed the Razor View Component Class Generator plugin)'

* Replace the contents of `TestComponent.cshtml` with the following:

		@@* /*
		Component model, defined in step 2
		*/ *@@
		@@model SampleComponentModel
           
		@@* /*
		Name of the client plugin, should match with plugin name defined in step 3 - Javascript Code
		*/ *@@
		@@ClientPlugin dj_SampleComponent

		@@* /*
		Templates are defined in step 4 - ClientTemplates
		TemplateId's (error, success) will be available in javascript code as this.templates.error, this.templates.success
		*/ *@@
		@@ClientTemplate RelativeResourceName=ClientTemplates.Error.htm, TemplateId=error
		@@ClientTemplate RelativeResourceName=ClientTemplates.Success.htm, TemplateId=success

		@@* /*
		The component't JS file defined in step 3
		*/ *@@
		@@ScriptResource RelativeResourceName=SampleComponent.js
		@@{ 
			// CSS class that will be added to the component's container
			CssClass = "dj_SampleComponent";
		}