namespace ConsoleAppTesting;

public class Animal
{
    public virtual void HacerSonido() { }
}

public class Perro : Animal
{
    public override void HacerSonido()
    {
        Console.WriteLine("Guau");
    }
}

public class Gato : Animal
{
    public override void HacerSonido()
    {
        Console.WriteLine("Miau");
    }
}