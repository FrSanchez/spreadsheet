﻿using System.Reactive.Concurrency;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using MusicStore.Models;
using ReactiveUI;

namespace MusicStore.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ICommand BuyMusicCommand { get;  }
    
    public Interaction<MusicStoreViewModel, AlbumViewModel?> ShowDialog { get; }
    
    public ObservableCollection<AlbumViewModel> Albums { get; } = [];

    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<MusicStoreViewModel, AlbumViewModel?>();
        BuyMusicCommand = ReactiveCommand.Create(async () =>
        {
            var store = new MusicStoreViewModel();
            var result = await ShowDialog.Handle(store);
            if (result != null)
            {
                Albums.Add(result);
                await result.SaveToDiskAsync();
            }
        });
        RxApp.MainThreadScheduler.Schedule(LoadAlbums);
    }
    
    private async void LoadAlbums()
    {
        var albums = (await Album.LoadCachedAsync()).Select(x => new AlbumViewModel(x));

        foreach (var album in albums)
        {
            Albums.Add(album);
        }

        foreach (var album in Albums.ToList())
        {
            await album.LoadCover();
        }
    }
}