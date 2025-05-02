namespace ClassLibrary.HxH_Services.Features.Hunters;

public static class HunterValidationId
{
    public static string? ValidateId(int id)
    {
        if (id <= 0)
            return "Invalid Id_Hunter";

        return null;
    }
}
