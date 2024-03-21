using EMedia_1;
using EMedia_1.Pixels;
using FftSharp;
using OxyPlot;
using OxyPlot.Series;
using PdfExporter = OxyPlot.SkiaSharp.PdfExporter;

var filename = @"C:\Users\bacht\Downloads\2.png";

if (!File.Exists(filename))
{
    PrintError($"File {filename} does not exist");
    return;
}

using var stream = File.Open(filename, FileMode.Open);
var reader = new PngReader(stream);

var image = reader.Read();

var pixelsCount = image.Pixels.Length;
Console.WriteLine($"Pixels: {pixelsCount}");

var signal = image.Pixels
    .Cast<IPixel>()
    .Take(LargestPowerOf2SmallerThan(pixelsCount))
    .Select(x => x.AverageValue)
    .ToArray();

var spectrum = FFT.Forward(signal);

var plotModel = new PlotModel { Title = "FFT" };
var realSeries = new LineSeries { Title = "Real", Color = OxyColors.Red };
var imaginarySeries = new LineSeries { Title = "Imaginary", Color = OxyColors.Blue };

for (var i = 2; i < spectrum.Length - 1; i++)
{
    realSeries.Points.Add(new DataPoint(i, spectrum[i].Real));
    imaginarySeries.Points.Add(new DataPoint(i, spectrum[i].Imaginary));
}

plotModel.Series.Add(realSeries);
plotModel.Series.Add(imaginarySeries);

var exporter = new PdfExporter {Width = 500, Height = 500};

using var exportStream = File.Create("../../../fft.pdf");

exporter.Export(plotModel, exportStream);

static void PrintError(string error)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Error: {error}");
    Console.ResetColor();
}

static int LargestPowerOf2SmallerThan(int n)
{
    n |= n >> 1;
    n |= n >> 2;
    n |= n >> 4;
    n |= n >> 8;
    n |= n >> 16;
    
    return n & ~(n >> 1);
}