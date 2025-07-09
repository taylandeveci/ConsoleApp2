namespace ConsoleApp2.Dto
{
    using Newtonsoft.Json;

    public class PokemonDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public ICollection<MoveDto> Moves { get; set; } = new List<MoveDto>();
    }

}
