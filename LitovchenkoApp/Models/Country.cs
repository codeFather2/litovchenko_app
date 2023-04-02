namespace LitovchenkoApp.Models;

public class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public List<Province> Provinces { get; set; } = new();

}