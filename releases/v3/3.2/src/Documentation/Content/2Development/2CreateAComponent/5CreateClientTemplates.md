In project `DowJones.Web.Mvc.UI.Components`

* Under folder `SampleComponent`, create a new folder - `ClientTemplates`

* Under newly created folder - `ClientTemplates` - create the following 2 files as `Embedded resources`:
	
	* Success.html
	* Error.html
		
	Right click on the newly created files -> Properties, set `Build Action` as `Embedded resource`.

#### Success.html

	<% var data = self.data, options = self.options;  %>
	<div class="dj_SampleComponentContent" style="border: 1px solid black;">
		Text One:
		<div class="textOne" style="color: <%= options.textColor %>; font-size: <%= options.textSize %>;">
			<%= data.textOne %>
		</div>
		<br/><br/>
		Text Two:
		<div class="textTwo" style="color: <%= options.textColor %>; font-size: <%= options.textSize %>;">
			<%= data.textTwo %>
		</div>
	</div>

#### Error.html

	<% if (self.Error.Message) { %>
		<div class="djError">
			<p>
				Error: 
				<span class="djErrorMessage"><%= self.Error.Message %></span> 
				<span class="djErrorCode">(<%= self.Error.Code %>)</span>
			</p>
		</div>
	<% } %>
