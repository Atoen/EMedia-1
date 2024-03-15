using EMedia_1;

Console.WriteLine("Enter filename to open");

var filename = @"C:\Users\bacht\Pictures\Screenshots\Zrzut ekranu 2024-03-12 212455.png";

if (!File.Exists(filename))
{
    PrintError($"File {filename} does not exist");
    return;
}

using var stream = File.Open(filename, FileMode.Open);
var reader = new PngReader(stream);

reader.Read();

void PrintError(string error)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error: {error}");
    Console.ResetColor();
}