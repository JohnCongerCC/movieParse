using System;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using CsvHelper;

class Program
{
    static void Main()
    {
        string rootDirectory = @"E:\Movies\Movies";
        string[] movieFolders = Directory.GetDirectories(rootDirectory);

        using (var writer = new StreamWriter("movies.csv"))
        using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture, false))
        {
            csv.WriteField("Title");
            csv.WriteField("IMDB Rating");
            csv.NextRecord();

            foreach (string movieFolder in movieFolders)
            {
                string imdbRating = "FileNotFound";

                try
                {
                string imdbUrl = File.ReadAllText(Path.Combine(movieFolder, "basic_movie_info.nfo"));    
                imdbRating = GetIMDBRating(imdbUrl);
                }
                catch (System.IO.FileNotFoundException)
                {
                    imdbRating = "FileNotFound";
                }
                csv.WriteField(Path.GetFileName(movieFolder));
                csv.WriteField(imdbRating);
                csv.NextRecord();
            }
        }
    }

    static string GetIMDBRating(string imdbUrl)
    {
        string html = new WebClient().DownloadString(imdbUrl);
        Match match = Regex.Match(html, @"<span class=""sc-7ab21ed2-1 eUYAaq"">(.*?)</span>");
        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return "N/A";
    }
}