using EMedia_1;

var filename = @"C:\Users\Michal\Desktop\good_text.png";

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