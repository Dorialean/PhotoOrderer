string workingDirectory = Environment.CurrentDirectory;
string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

var photoDir = new DirectoryInfo(Path.Combine(projectDirectory, "photos"));
var orderedDir = new DirectoryInfo(Path.Combine(projectDirectory, "ordered"));

// Файл к его году
Dictionary<FileInfo, int> fileToYear = new();

FillFileToDate(photoDir);

foreach (var ftd in fileToYear)
{
    var photo = ftd.Key;
    var year = ftd.Value;
    if (orderedDir.GetDirectories()
		.Any(dir => dir.Name == ftd.Value.ToString()))
	{
		var path = Path.Combine(orderedDir.FullName, year.ToString());
		try
		{
            photo.MoveTo(Path.Combine(path, photo.Name));
        }
		catch(IOException e)
		{
			continue;
		}
    }
	else
	{
        var path = Path.Combine(orderedDir.FullName, year.ToString());
		Directory.CreateDirectory(path);
        try
        {
            photo.MoveTo(Path.Combine(path, photo.Name));
        }
        catch (IOException e)
        {
            continue;
        }
	}
}
Console.WriteLine(GetDirectorySize(orderedDir.FullName));

void FillFileToDate(DirectoryInfo dir)
{
	if (dir.Exists)
	{
		FileInfo[] photos = dir.GetFiles();
		foreach (var photo in photos)
		{
            fileToYear.Add(photo, photo.LastWriteTime.Year);
        }
	}
}

static long GetDirectorySize(string parentDirectory)
{
    return new DirectoryInfo(parentDirectory).GetFiles("*.*", SearchOption.AllDirectories).Sum(file => file.Length);
}





