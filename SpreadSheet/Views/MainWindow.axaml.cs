using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using Avalonia.ReactiveUI;
using Engine;
using SpreadSheet.Models;
using SpreadSheet.ViewModels;

namespace SpreadSheet.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();
        
        MainGrid.Columns.Clear();
        for (char c = 'A'; c <= 'Z'; c++)
        {
            var col = new DataGridTextColumn();
            col.IsReadOnly = false;
            col.Header = c.ToString();
            col.Binding = new Binding($"[{c - 'A'}].Value");
            MainGrid.Columns.Add(col);
        }

        MainGrid.LoadingRow += MainGridOnLoadingRow;
    }

    private void MainGridOnLoadingRow(object? sender, DataGridRowEventArgs e)
    {
        var row = e.Row;
        row.Header = (row.GetIndex() + 1).ToString();
        row.Background = (row.GetIndex() % 2 == 0) ? new SolidColorBrush(0xffe0e0e0) : new SolidColorBrush(0xffd0d0d0);
    }

    private void MainGrid_OnCellPointerPressed(object? sender, DataGridCellPointerPressedEventArgs e)
    {
        var vm = ViewModel;
        var dg = (DataGrid)sender!;
        int row = e.Row.GetIndex();
        if (e.Column != null)
        {
            int col = e.Column.Header.ToString()[0] - 'A';
            var cells = (List<List<Cell>>)dg.ItemsSource;
            if (row < vm.Spreadsheet.Count && col < vm.Spreadsheet[0].Count)
            {
                var cell = cells[row][col];
                MyText.Text = $"[{e.Column.Header}{row + 1}] : {cell.Text}";
            }
        }
    }

    private void MainGrid_OnPreparingCellForEdit(object? sender, DataGridPreparingCellForEditEventArgs e)
    {
        var vm = ViewModel;
        var block = (TextBox)e.EditingElement;
        var dg = (DataGrid)sender!;
        int row = e.Row.GetIndex();
        if (e.Column != null)
        {
            int col = e.Column.Header.ToString()[0] - 'A';
            var cells = (List<List<Cell>>)dg.ItemsSource;
            if (row < vm.Spreadsheet.Count && col < vm.Spreadsheet[0].Count)
            {
                var cell = cells[row][col];
                block.Text = cell.Text;
            }
        }
    }

    private void MainGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        var vm = ViewModel;
        var block = (TextBox)e.EditingElement;
        var dg = (DataGrid)sender!;
        int row = e.Row.GetIndex();
        if (vm != null && e.Column != null && block.Text != null)
        {
            int? col = e.Column?.Header?.ToString()[0] - 'A';
            if (col.HasValue && row < vm.Spreadsheet.Count && col < vm.Spreadsheet[0].Count)
            {
                vm.SetCellText(row,col.Value,block.Text);
            }
        }
    }
}