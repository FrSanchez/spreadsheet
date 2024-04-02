using System.Reactive;
using Avalonia.Media;
using ReactiveUI;

namespace SpreadSheet.ViewModels;

public class ColorDialogViewModel : ViewModelBase
{
    public ReactiveCommand<Unit, ColorChangeViewModel> OkColor { get; }
    
    private Color? _color;

    private ColorChangeViewModel _selectedColor;

    public ColorChangeViewModel SelectedColor
    {
        get => _selectedColor;
        set => this.RaiseAndSetIfChanged(ref _selectedColor, value);
    }

    public Color? Color
    {
        get => _color;
        set => this.RaiseAndSetIfChanged(ref _color, value);
    }

    public ColorDialogViewModel(Color color)
    {
        this._color = color;
        _selectedColor = new ColorChangeViewModel(color);

        OkColor = ReactiveCommand.Create(() => SelectedColor);
    }
}