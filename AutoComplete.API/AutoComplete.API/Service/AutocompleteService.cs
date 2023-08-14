using AutoComplete.API.Model;
using Elasticsearch.Net;
using Nest;

namespace AutoComplete.API.Service
{
    public class AutocompleteService : IAutocompleteService
    {
         ElasticClient _elasticClient;

        public AutocompleteService(ConnectionSettings connectionSettings)
        {
            _elasticClient = new ElasticClient(connectionSettings);
        }

        public async Task<bool> CreateIndexAsync(string indexName)
        {
            var node = new Uri("https://localhost:9200");
            var username = "elastic"; // Replace with your Elasticsearch username
            var password = "5LBR-s7dDQbefu2MtA2m"; // Replace with your Elasticsearch password
            var connectionPool = new SingleNodeConnectionPool(node);
            var settings = new ConnectionSettings(connectionPool)
                .BasicAuthentication(username, password)
                .ServerCertificateValidationCallback((sender, certificate, chain, sslPolicyErrors) => true)
                .DefaultIndex("myindex");

            _elasticClient = new ElasticClient(settings);
            var indices = await _elasticClient.Indices.GetAliasAsync();

            if (!_elasticClient.Indices.Exists(indexName).Exists)
            {
                var createIndexResponse = await _elasticClient.Indices.CreateAsync(indexName, c => c
                .Map<Product>(m => m
                    .AutoMap() 
                ) );

                return createIndexResponse.IsValid;
            }

            return false;
        }

        public async Task IndexAsync(string indexName, List<Product> products)
        {
            await _elasticClient.IndexManyAsync(products, indexName);
        }

        public async Task<ProductSuggestResponse> SuggestAsync(string indexName, string keyword)
        {
            ISearchResponse<Product> searchResponse = await _elasticClient.SearchAsync<Product>(s => s
                                     .Index(indexName)
                                     .Suggest(su => su
                                          .Completion("suggestions", c => c
                                               .Field(f => f.Suggest)
                                               .Prefix(keyword.ToLower())
                                               .Fuzzy(f => f
                                                   .Fuzziness(Fuzziness.Auto)
                                               )
                                               .Size(5))
                                             ));

            var suggests = from suggest in searchResponse.Suggest["suggestions"]
                           from option in suggest.Options
                           select new ProductSuggest
                           {
                               Name = option.Text,
                               Score = option.Score
                           };

            return new ProductSuggestResponse
            {
                Suggests = suggests
            };
        }

       
    }
}
