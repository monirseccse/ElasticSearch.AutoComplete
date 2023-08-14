using Nest;

namespace AutoComplete.API.Model
{
    public class Product
    {
        public string Name { get; set; }
        public CompletionField Suggest { get; set; } = null!;
    }
}
