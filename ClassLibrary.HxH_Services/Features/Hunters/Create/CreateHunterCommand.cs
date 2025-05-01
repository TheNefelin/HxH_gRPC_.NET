namespace ClassLibrary.HxH_Services.Features.Hunters.Create;

public class CreateHunterCommand
{
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Origin { get; set; } = string.Empty;
}
