#### Rendering the component from a Razor View

	<!-- Instantiate and fill model-->
	var model = new SocialButtonsModel
    {
        SocialNetworks = new List<SocialNetworks>
						{
							SocialNetworks.Delicious,
							SocialNetworks.LinkedIn,
							SocialNetworks.Twitter,
							SocialNetworks.Technorati,
							SocialNetworks.Yahoo,
							SocialNetworks.StumbleUpon
						},
        ImageSize = ImageSize.Large,
        Title = "Share",
        Url = "http://www.dowjones.com",
        Keywords = "Social",
        Description = "Share Content"
    };

	<!-- Render the component using the model-->
	@@Html.DJ().Render(Model)