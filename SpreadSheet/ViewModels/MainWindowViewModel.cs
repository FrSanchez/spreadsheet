using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;

namespace SpreadSheet.ViewModels;

using Engine;

public class MainWindowViewModel : ViewModelBase
{
    // public ObservableCollection<IEnumerable<Cell>> Spreadsheet { get; }
    public List<List<Cell>> Spreadsheet { get;  }

    private readonly SpreadSheet _spreadsheet;

    private const int RowCount = 50;
    private const int ColumnCount = 'Z' - 'A' + 1;
    
    public Interaction<ColorDialogViewModel, ColorDialogViewModel?> ShowDialog { get; }
    public ICommand SelectColorCommand { get; }
    
    private readonly Random _rand = new ();
    public void DoDemoBonus(object msg)
    {
        for (var row = 0; row < RowCount; row++)
        {
            for(var col = 0; col < ColumnCount; col++)
            {
                if (row % 3 == 1 && col % 2 == 0)
                {
                    _spreadsheet[row, col].Text = $"={(char)(col + 'A')}{row}";
                }
                if (row % 3 == 0)
                {
                    _spreadsheet[row, col].Text = $"{_rand.Next(0xFFFF):x4}";
                }
            }
        }
    }

    public void SetCellText(int row, int col, string value)
    {
        _spreadsheet[row, col].Text = value;
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
            for(var col = 0; col < ColumnCount; col++)
            {
                _spreadsheet[row, col].Text = col switch
                {
                    0 => $"=B{row + 1}",
                    1 => $"This is cell B{(row + 1):d2}",
                    _ => string.Empty
                };

                int val = (row * ColumnCount) + col;
                if (randomCells.Contains(val))
                {
                    _spreadsheet[row, col].Text = $"Rando {++randomText}";
                }
            }
        }
    }
    
    public uint BgColor { get; set; }

    public MainWindowViewModel()
    {
        ShowDialog = new Interaction<ColorDialogViewModel, ColorDialogViewModel?>();
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
            var store = new ColorDialogViewModel();

            var result = await ShowDialog.Handle(store);
        });
    }
}