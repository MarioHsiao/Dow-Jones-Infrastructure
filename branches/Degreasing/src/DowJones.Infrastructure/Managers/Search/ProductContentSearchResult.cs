namespace DowJones.Managers.Search
{
    public class ProductContentSearchResult
    {
        public Factiva.Gateway.Messages.Search.V2_0.ContentSearchResult ContentSearchResult { set; get; }

        public string ProductId { get;  private set; }

        public string PrimarySourceGroupId { get; private set; }

        public string SecondarySourceGroupId { get; private set; }

        public ProductContentSearchResult(string productId, string sourceGroupId = null, string secondarySourceGroupId = null)
        {
            ProductId = productId;
            PrimarySourceGroupId = sourceGroupId;
            SecondarySourceGroupId = secondarySourceGroupId;
        }
    }
}