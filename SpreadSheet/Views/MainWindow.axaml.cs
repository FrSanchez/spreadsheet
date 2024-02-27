using System;
using System.Collections;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;

namespace SpreadSheet.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        MainGrid.Columns.Clear();
        for (char c = 'A'; c <= 'Z'; c++)
        {
            var col = new DataGridTextColumn();
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
    }
}