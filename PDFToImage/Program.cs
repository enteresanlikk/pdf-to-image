using ImageMagick;

string inputFileName = "sample.pdf";

var currentFolder = "YOUR_PROJECT_PATH";

MagickNET.SetGhostscriptDirectory(Path.Combine(currentFolder, "GhostScripts"));

List<string> _imageExtensions = new List<string>() { "jpg", "png", "webp" };

string inputFilePath = $"{currentFolder}\\Inputs\\{inputFileName}";

string exportFolder = $"{currentFolder}\\Exports\\" + Guid.NewGuid();
Directory.CreateDirectory(exportFolder);

string exportInputFilePath = $"{exportFolder}\\{inputFileName}";
File.Copy(inputFilePath, exportInputFilePath);

using (MagickImageCollection images = new MagickImageCollection())
{
    images.Read(exportInputFilePath);

    int page = 1;
    foreach (MagickImage image in images)
    {
        foreach (var extension in _imageExtensions)
        {
            string fileName = page + "." + extension;
            string filePath = Path.Combine(exportFolder, fileName);
            
            image.Resize(new MagickGeometry(1280, 720));
            image.Alpha(AlphaOption.Remove);
            image.Density = new Density(300);
            image.ColorSpace = ColorSpace.sRGB;

            image.Write(filePath);
        }
        page++;
    }
}

Console.WriteLine("Done...");