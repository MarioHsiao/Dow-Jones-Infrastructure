
Populate the `SocialButtons` model:

	var socialButtonsModel = new SocialButtonsModel
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

	
 Render the model in your view which will render the component in the browser:

 <!-- Render the component -->
	@@model socialButtonsModel

	@@Html.DJ().Render(socialButtonsModel)