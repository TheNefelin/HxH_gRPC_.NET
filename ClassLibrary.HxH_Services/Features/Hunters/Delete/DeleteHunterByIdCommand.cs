namespace ClassLibrary.HxH_Services.Features.Hunters.Delete;

public class DeleteHunterByIdCommand
{
    public int Id_Hunter { get; set; }

    public DeleteHunterByIdCommand(int idHunter)
    {
        Id_Hunter = idHunter;
    }
}
