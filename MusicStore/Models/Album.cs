using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using iTunesSearch.Library;

namespace MusicStore.Models;

public class Album(string artist, string title, string coverUrl)
{
    private static readonly iTunesSearchManager SSearchManager = new();

    public string Artist { get; set; } = artist;
    public string Title { get; set; } = title;
    private string CoverUrl { get; set; } = coverUrl;

    private static readonly HttpClient SHttpClient = new();
    private string CachePath => $"./Cache/{Artist} - {Title}";

    public Album():this(string.Empty, string.Empty, string.Empty)
    {
        
    }

    public async Task<Stream> LoadCoverBitmapAsync()
    {
        if (File.Exists(CachePath + ".bmp"))
        {
            return File.OpenRead(CachePath + ".bmp");
        }
        else
        {
            var data = await SHttpClient.GetByteArrayAsync(CoverUrl);
            return new MemoryStream(data);
        }
    }
    public static async Task<IEnumerable<Album>> SearchAsync(string searchTerm)
    {
        var query = await SSearchManager.GetAlbumsAsync(searchTerm)
            .ConfigureAwait(false);
                
        return query.Albums.Select(x =>
            new Album(x.ArtistName, x.CollectionName, 
                x.ArtworkUrl100.Replace("100x100bb", "600x600bb")));
    }
    
    public async Task SaveAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        await using var fs = File.OpenWrite(CachePath);
        await SaveToStreamAsync(this, fs);
    }

    public Stream SaveCoverBitmapStream()
    {
        return File.OpenWrite(CachePath + ".bmp");
    }

    private static async Task SaveToStreamAsync(Album data, Stream stream)
    {
        await JsonSerializer.SerializeAsync(stream, data).ConfigureAwait(false);
    }
    
    public static async Task<Album> LoadFromStream(Stream stream)
    {
        return (await JsonSerializer.DeserializeAsync<Album>(stream).ConfigureAwait(false))!;
    }

    public static async Task<IEnumerable<Album>> LoadCachedAsync()
    {
        if (!Directory.Exists("./Cache"))
        {
            Directory.CreateDirectory("./Cache");
        }

        var results = new List<Album>();

        foreach (var file in Directory.EnumerateFiles("./Cache"))
        {
            if (!string.IsNullOrWhiteSpace(new DirectoryInfo(file).Extension)) continue;

            await using var fs = File.OpenRead(file);
            results.Add(await Album.LoadFromStream(fs).ConfigureAwait(false));
        }

        return results;
    }
}