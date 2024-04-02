using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;
using MusicStore.Models;
using ReactiveUI;

namespace MusicStore.ViewModels;

public class AlbumViewModel(Album album) : ViewModelBase
{
    public string Artist => album.Artist;
    public string Title => album.Title;
    
    private Bitmap? _cover;

    public Bitmap? Cover
    {
        get => _cover;
        private set => this.RaiseAndSetIfChanged(ref _cover, value);
    }

    public async Task LoadCover()
    {
        await using var imageStream = await album.LoadCoverBitmapAsync();
        Cover = await Task.Run(() =>
        {
            try
            {
                var targetWidth = 400;
                // var image = Bitmap.DecodeToWidth(imageStream, 400);
                // 
                using Bitmap fullImage = new(imageStream);
                var newHeight = fullImage.Size.Width > targetWidth
                    ? 400d / fullImage.Size.Width * fullImage.Size.Height
                    : fullImage.Size.Height;

                var thumbnail = fullImage.CreateScaledBitmap(new PixelSize(targetWidth, (int)newHeight));

                return thumbnail;
            }
            catch (Exception x)
            {
                Console.WriteLine(x.ToString());
            }

            return null;
        });
    }
    
    public async Task SaveToDiskAsync()
    {
        await album.SaveAsync();

        if (Cover != null)
        {
            var bitmap = Cover;

            await Task.Run(() =>
            {
                using var fs = album.SaveCoverBitmapStream();
                bitmap.Save(fs);
            });
        }
    }
}