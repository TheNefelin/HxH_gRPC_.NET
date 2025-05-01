// See https://aka.ms/new-console-template for more information

int[] numeros = { 5, 2, 9, 1, 7 };

for (int i = 0; i < numeros.Length - 1; i++)
{
    for (int j = 0; j < numeros.Length - 1 - i; j++)
    {
        if (numeros[j] < numeros[j + 1]) // cambio para orden DESCENDENTE
        {
            int temp = numeros[j];
            numeros[j] = numeros[j + 1];
            numeros[j + 1] = temp;
        }
    }
}

Console.WriteLine("Arreglo ordenado de mayor a menor:");
foreach (var num in numeros)
{
    Console.Write(num + " ");
}
