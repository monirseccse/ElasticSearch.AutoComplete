namespace AutoComplete.API.Model
{
    public class ProductSuggestResponse
    {
        public IEnumerable<ProductSuggest> Suggests { get; set; } = null!;
    }

    public class ProductSuggest
    {
        public string Name { get; set; }
        public double Score { get; set; }
    }
}
