using Newtonsoft.Json;
using System.Text.Json.Serialization;
namespace ScryfallAPI.Utilities
{
    public class APIRetriever
    {
        private ILogger<APIRetriever>? _logger;

        public APIRetriever(ILogger<APIRetriever>? logger) 
        => _logger = logger;
        
        public APIRetriever() { }

        //Retrieving API data using HttpClient
        public async Task<ModelToParse> GetData()
        {

			try
            {
                var url = "https://api.scryfall.com/cards/search?q=b%3Ausg";
                ModelToParse dataRetrieved = new();
                JsonSerializer serializer = new JsonSerializer();
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "Accept");
				using Stream stream = await client.GetStreamAsync(url);
                using StreamReader streamReader = new StreamReader(stream);
                using (JsonTextReader reader = new JsonTextReader(streamReader)) 
                {
                    
                    while(reader.Read()) 
                    {
                        if(reader.TokenType == JsonToken.StartObject)
                        {
                            //Retrieving data and deserialzing it
                            dataRetrieved = serializer.Deserialize<ModelToParse>(reader) ?? new ModelToParse();

                            if (dataRetrieved is not null)
                            {

                                /*
                                 * limiting size to 20 results for a faster query operation
                                 * and demostration puroposes
                                 */
                                dataRetrieved.Data = dataRetrieved.Data
                                                    .OrderByDescending(p => p.PennyRank)
                                                    .Take(20)
                                                    .ToList();

                            }
                        }
                    }

                    await Task.CompletedTask;

                    return dataRetrieved ?? new ModelToParse();

                    
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Error retrieving data, creating an empty list, error {ex.InnerException.Message}");
                return new ModelToParse();
            }
        }
    }

	public class ModelToParse
	{

		public string? Object { get; set; }
        
        [JsonPropertyName("total_cards")]	
        public int TotalCards { get; set; }
        
        [JsonPropertyName("has_more")]
        public bool HasMore { get; set; }
        
        [JsonPropertyName("mext_page")]
        public string? NextPage { get; set; }
		public List<Data> Data { get; set; } = new List<Data>();
	}

	public class Data
	{
		public Guid? Id { get; set; }
		
        [JsonPropertyName("mtgo_id")]
		public string? MtgoId { get; set; }
		
        [JsonPropertyName("mtgo_foil_Id")]
		public string? MtgoFoilId { get; set; }
		
        [JsonPropertyName("tcgplayer_Id")]
		public string? TcgPlayerId { get; set; }

		public string? Name { get; set; }
		public string? Lang { get; set; }
		public string? Uri { get; set; }
		
        [JsonPropertyName("scryfall_uri")]
		public string? ScryfallUri { get; set; }
		public string? Layout { get; set; }

		[JsonPropertyName("highres_image")]
		public bool HighResImage { get; set; }

		[JsonPropertyName("released_at")]
		public string? ReleasedAt { get; set; }
		
        [JsonPropertyName("penny_rank")]
		public int PennyRank { get; set; }

	}

}
