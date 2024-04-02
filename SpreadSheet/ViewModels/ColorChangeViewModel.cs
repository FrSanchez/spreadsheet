using Avalonia.Media;
using ReactiveUI;

namespace SpreadSheet.ViewModels;

public class ColorChangeViewModel(Color color) : ViewModelBase
{
    public Color Color
    {
        get => color;
        set => this.RaiseAndSetIfChanged(ref color, value);
    }
}