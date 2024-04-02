using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using Avalonia.Controls;
using SpreadSheet.ViewModels;

namespace SpreadSheet.Views;

public partial class ColorDialogWindow : ReactiveWindow<ColorDialogViewModel>
{
    public ColorDialogWindow()
    {
        InitializeComponent();
        // This line is needed to make the previwer happy (the previewer plugin cannot handle the following line).
        if (Design.IsDesignMode) return;

        this.WhenActivated(d => d(ViewModel!.OkColor.Subscribe(Close)));
    }
}