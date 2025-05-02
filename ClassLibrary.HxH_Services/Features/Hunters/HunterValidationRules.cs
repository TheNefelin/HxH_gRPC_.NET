namespace ClassLibrary.HxH_Services.Features.Hunters;

public static class HunterValidationRules
{
    public static string? Validate(string name, int age, string origin)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Invalid Name";
        
        if (age <= 0)
            return "Invalid Age";

        if (string.IsNullOrWhiteSpace(origin))
            return "Invalid Origin";

        return null;
    }
}
