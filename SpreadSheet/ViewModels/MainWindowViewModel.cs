using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Serialization;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using ReactiveUI;
using SpreadSheet.Models;

namespace SpreadSheet.ViewModels;

using Engine;

public class MainWindowViewModel : ViewModelBase
{
    public List<List<Cell>> Spreadsheet { get; }

    private readonly SpreadSheet _spreadsheet;

    private const int RowCount = 50;
    private const int ColumnCount = 'Z' - 'A' + 1;

    public Interaction<ColorDialogViewModel, ColorChangeViewModel?> ShowDialog { get; }
    public ICommand SelectColorCommand { get; }
    public ICommand UndoCommand { get; }
    public ICommand RedoCommand { get; }

    private readonly Random _rand = new();

    public void DoDemoBonus(object msg)
    {
        for (var row = 0; row < RowCount; row++)
        {
            for (var col = 0; col < ColumnCount; col++)
            {
                if (row % 3 == 1 && col % 2 == 0)
                {
                    _spreadsheet.SetCellText(row, col, $"={(char)(col + 'A')}{row}");
                }

                if (row % 3 == 0)
                {
                    _spreadsheet.SetCellText(row, col, $"{_rand.Next(0xFFFF):x4}");
                }
            }
        }
    }

    public void SetCellText(int row, int col, string value)
    {
        _spreadsheet.SetCellText(row, col, value);
    }

    private int NextRandom()
    {
        int rndRow = _rand.Next(0, RowCount);
        int rndCol = _rand.Next(2, ColumnCount);
        return (rndRow * ColumnCount) + rndCol;
    }

    public void DoDemoHw(object msg)
    {
        HashSet<int> randomCells = new();
        while (randomCells.Count < 50)
        {
            randomCells.Add(NextRandom());
        }

        int randomText = 0;
        for (var row = 0; row < RowCount; row++)
        {
            for (var col = 0; col < ColumnCount; col++)
            {
                _spreadsheet.SetCellText(row, col, col switch
                {
                    0 => $"=B{row + 1}",
                    1 => $"This is cell B{(row + 1):d2}",
                    _ => string.Empty
                });

                int val = (row * ColumnCount) + col;
                if (randomCells.Contains(val))
                {
                    _spreadsheet.SetCellText(row, col, $"Rando {++randomText}");
                }
            }
        }
    }

    private uint BgColor { get; set; }
    public int UndoStackSize => _spreadsheet.GetUndoSize();

    public int RedoStackSize => _spreadsheet.GetRedoSize();

    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<ColorDialogViewModel, ColorChangeViewModel?>();
        BgColor = 0xFF000000;
        Spreadsheet = [];
        _spreadsheet = new SpreadSheet(RowCount, ColumnCount);
        foreach (var rowIndex in Enumerable.Range(0, RowCount))
        {
            var columns = new List<Cell>(ColumnCount);
            foreach (var columnIndex in Enumerable.Range(0, ColumnCount))
            {
                columns.Add(_spreadsheet.GetCell(rowIndex, columnIndex));
            }

            Spreadsheet.Add(columns);
        }

        SelectColorCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            var colorChooser = new ColorDialogViewModel(Color.FromUInt32(BgColor));

            var result = await ShowDialog.Handle(colorChooser);
            if (result != null)
            {
                BgColor = result.Color.ToUInt32();
                Console.WriteLine("Change color" + result.Color);
            }
        });

        UndoCommand = ReactiveCommand.Create(() => { _spreadsheet.Undo(); });

        RedoCommand = ReactiveCommand.Create(() => { _spreadsheet.Redo(); });
    }

    public async Task SaveAsync(IStorageFile file)
    {
        var output = new SpreadsheetData();
        foreach (var cells in Spreadsheet)
        {
            var row = new RowData();
            row.AddRange(cells.Select(cell => new CellData(cell)).ToList());
            output.Add(row);
        }
        var serializer = new XmlSerializer(output.GetType());
        await using var stream = await file.OpenWriteAsync();
        await using var writer = new StreamWriter(stream);
        serializer.Serialize(writer, output);
    }

    public async Task ReadAsync(IStorageFile file)
    {
        var input = new SpreadsheetData();
        var serializer = new XmlSerializer(input.GetType());
        await using var stream = await file.OpenReadAsync();
        using var reader = new StreamReader(stream);
        input = (SpreadsheetData)serializer.Deserialize(reader)!;
        var row = 0;
        foreach (var cells in input)
        {
            var col = 0;
            foreach (var cell in cells)
            {
                Spreadsheet[row][col].Text = cell.Text;
                Spreadsheet[row][col].BackgroundColor = cell.BackgroundColor;
                col++;
            }

            row++;
        }
        // _spreadsheet.ReadFile(stream);
    }
}