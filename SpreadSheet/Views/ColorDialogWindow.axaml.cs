using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using SpreadSheet.ViewModels;

namespace SpreadSheet.Views;

public partial class ColorDialogWindow : ReactiveWindow<ColorDialogViewModel>
{
    public ColorDialogWindow()
    {
        InitializeComponent();
    }
    
}