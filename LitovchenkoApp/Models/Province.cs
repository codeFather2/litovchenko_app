namespace LitovchenkoApp.Models;

using System.Text.Json.Serialization;

public class Province
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public int CountryId { get; set; }

    public Country? Country { get; set; }
    
    [JsonIgnore]
    public List<User> Users { get; set; } = new();
}