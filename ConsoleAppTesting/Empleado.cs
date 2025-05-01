namespace ConsoleAppTesting;

public class Empleado
{
    public string Nombre { get; set; }
    public int SueldoBase { get; set; }
    public int HorasExtras { get; set; }

    public int CalcularSueldoTotal()
    {
        return SueldoBase + (HorasExtras * 15);
    }
}
