namespace ClassLibrary.HxH_Services.Features.Hunters.Update;

public class UpdateHunterCommand
{
    public int Id_Hunter { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public string Origin { get; set; } = string.Empty;
}
