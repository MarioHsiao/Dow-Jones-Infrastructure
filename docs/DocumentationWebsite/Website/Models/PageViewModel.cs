namespace DowJones.Documentation.Website.Models
{
    public class PageViewModel : ContentSectionViewModel
    {
        public CategoryViewModel Category { get; private set; }


        public PageViewModel(ContentSection section, CategoryViewModel category)
            : base(section)
        {
            Category = category;
        }
    }
}