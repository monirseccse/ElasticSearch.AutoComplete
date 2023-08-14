using AutoComplete.API.Model;

namespace AutoComplete.API.Service
{
    public interface IAutocompleteService
    {
        Task<bool> CreateIndexAsync(string indexName);
        Task IndexAsync(string indexName, List<Product> products);
        Task<ProductSuggestResponse> SuggestAsync(string indexName, string keyword);
    }
}
