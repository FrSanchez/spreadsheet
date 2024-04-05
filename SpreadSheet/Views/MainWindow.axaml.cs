using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using Engine;
using ReactiveUI;
using SpreadSheet.Storage;
using SpreadSheet.ViewModels;

namespace SpreadSheet.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        
        MainGrid.Columns.Clear();
        for (var c = 'A'; c <= 'Z'; c++)
        {
            var col = new DataGridTextColumn
            {
                IsReadOnly = false,
                Header = c.ToString(),
                Binding = new Binding($"[{c - 'A'}].Value", BindingMode.TwoWay)
            };
            MainGrid.Columns.Add(col);
        }

        MainGrid.LoadingRow += (sender, args) =>
        {
            var row = args.Row;
            row.Header = (row.GetIndex() + 1).ToString();
            // row.Background = (row.GetIndex() % 2 == 0)
            //     ? new SolidColorBrush(0xffe0e0e0)
            //     : new SolidColorBrush(0xffd0d0d0);
        };
        
        this.WhenActivated(action => 
            action(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
    }
    
    private void MainGrid_OnCellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
    {
        var vm = ViewModel;
        var dg = (DataGrid)sender!;
        var row = e.Row.GetIndex();
        if (e.Column == null) return;
        var col = e.Column.DisplayIndex;
        var cells = (List<List<Cell>>)dg.ItemsSource;
        if (vm == null || row >= vm.Spreadsheet.Count || col >= vm.Spreadsheet[0].Count) return;
        var cell = cells[row][col];
        MyText.Text = $"[{e.Column.Header}{row + 1}] : {cell.Text}";
    }

    
    private void MainGrid_OnCurrentCellChanged(object? sender, EventArgs e)
    {
        if (sender is not DataGrid grid) return;
        if (grid.SelectedItem is not List<Cell> item) return;
        Console.WriteLine(grid.SelectedIndex + "," + grid.CurrentColumn.DisplayIndex );
        var cell = item[grid.CurrentColumn.DisplayIndex];
        MyText.Text = $"[{grid.CurrentColumn.Header}{grid.SelectedIndex + 1}] : {cell.Text}";
    }

    private void MainGrid_OnPreparingCellForEdit(object? sender, DataGridPreparingCellForEditEventArgs e)
    {
        var vm = ViewModel;
        var block = (TextBox)e.EditingElement;
        var dg = (DataGrid)sender!;
        int row = e.Row.GetIndex();
        if (e.Column != null)
        {
            var col = e.Column.Header.ToString()![0] - 'A';
            var cells = (List<List<Cell>>)dg.ItemsSource;
            if (vm != null && row < vm.Spreadsheet.Count && col < vm.Spreadsheet[0].Count)
            {
                var cell = cells[row][col];
                block.Text = cell.Text;
            }
        }
    }

    private void MainGrid_OnCellEditEnding(object? _, DataGridCellEditEndingEventArgs e)
    {
        var vm = ViewModel;
        var block = (TextBox)e.EditingElement;
        var row = e.Row.GetIndex();
        if (vm != null && e.Column != null && block.Text != null)
        {
            var col = e.Column?.Header?.ToString()?[0] - 'A';
            if (col.HasValue && row < vm.Spreadsheet.Count && col < vm.Spreadsheet[0].Count)
            {
                vm.SetCellText(row,col.Value,block.Text);
            }
        }
        // vm?.UpdateBars();
        UndoBar.Value = (double)vm?.UndoStackSize!;
        RedoBar.Value = (double)vm?.RedoStackSize!;
        
    }

    private void MenuItem_OnExit(object? sender, RoutedEventArgs e)
    {
        this.Close();
    }
    
    private async Task DoShowDialogAsync(InteractionContext<ColorDialogViewModel,
        ColorChangeViewModel?> interaction)
    {
        var dialog = new ColorDialogWindow
        {
            DataContext = interaction.Input
        };

        var result = await dialog.ShowDialog<ColorChangeViewModel?>(this);
        interaction.SetOutput(result);
    }

    private async void SaveFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        // Start async operation to open the dialog.
        var file = await topLevel?.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Spreadsheet File",
            FileTypeChoices = new []{ MyFilePickerFileTypes.Xml }
        })!;

        if (file is not null)
        {
            ViewModel?.SaveAsync(file);
        }
    }
    
    private async void OpenFileButton_Clicked(object sender, RoutedEventArgs args)
    {
        // Get top level from the current control. Alternatively, you can use Window reference instead.
        var topLevel = GetTopLevel(this);

        // Start async operation to open the dialog.
        var files = await topLevel?.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Spreadsheet File",
            FileTypeFilter = new[] { MyFilePickerFileTypes.Xml },
            AllowMultiple = false
        })!;

        if (files.Count >= 1)
        {
            ViewModel?.ReadAsync(files[0]);
        }
    }
}