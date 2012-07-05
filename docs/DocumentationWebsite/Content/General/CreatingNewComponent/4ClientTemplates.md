In project `DowJones.Web.Mvc.UI.Components`

* Under folder `SampleComponent`, create a new folder - `ClientTemplates`

* Under newly created folder - `ClientTemplates` - create the following 2 files:

`Success.htm`
<pre><code>
&lt;% var data = self.data, options = self.options;  %&gt;
&lt;div class="dj_SampleComponentContent" style="border: 1px solid black;"&gt;
    Text One:
    &lt;div class="textOne" style="color: &lt;%= options.textColor %&gt;; font-size: &lt;%= options.textSize %&gt;;"&gt;
        &lt;%= data.textOne %&gt;
    &lt;/div&gt;
    &lt;br/&gt;&lt;br/&gt;
    Text Two:
    &lt;div class="textTwo" style="color: &lt;%= options.textColor %&gt;; font-size: &lt;%= options.textSize %&gt;;"&gt;
        &lt;%= data.textTwo %&gt;
    &lt;/div&gt;
&lt;/div&gt;
</code></pre>

`Error.html`
<pre><code>
&lt;% if (self.Error.Message) { %&gt;
	&lt;div class="djError"&gt;
		&lt;p&gt;
			Error: 
			&lt;span class="djErrorMessage"&gt;&lt;%= self.Error.Message %&gt;&lt;/span&gt; 
			&lt;span class="djErrorCode"&gt;(&lt;%= self.Error.Code %&gt;)&lt;/span&gt;
		&lt;/p&gt;
	&lt;/div&gt;
&lt;% } %&gt;
</code></pre>