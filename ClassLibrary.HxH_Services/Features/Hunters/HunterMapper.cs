using System.Data;

namespace ClassLibrary.HxH_Services.Features.Hunters;

public static class HunterMapper
{
    public static HunterDto FromDataRow(DataRow row)
    {
        return new HunterDto
        {
            Id_Hunter = Convert.ToInt32(row["Id_Hunter"]),
            Name = row["Name"].ToString() ?? string.Empty,
            Age = Convert.ToInt32(row["Age"]),
            Origin = row["Origin"].ToString() ?? string.Empty
        };
    }
}