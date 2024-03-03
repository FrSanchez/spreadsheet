using System;
using System.Collections.Generic;
using System.Linq;

namespace SpreadSheet.ViewModels;

using Engine;

public class MainWindowViewModel : ViewModelBase
{
    // public ObservableCollection<IEnumerable<Cell>> Spreadsheet { get; }
    public List<List<Cell>> Spreadsheet { get;  }
    private SpreadSheet _spreadsheet;

    private const int RowCount = 50;
    private const int ColumnCount = 'Z' - 'A' + 1;

    
    private readonly Random _rand = new ();
    public void DoDemoBonus(object msg)
    {
        for (int row = 0; row < RowCount; row++)
        {
            for(int col = 0; col < ColumnCount; col++)
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
    
    private int nextRandom()
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
            randomCells.Add(nextRandom());
        }

        int randomText = 0;
        for (int row = 0; row < RowCount; row++)
        {
            for(int col = 0; col < ColumnCount; col++)
            {
                _spreadsheet[row, col].Text = string.Empty;
                if (col == 0)
                {
                    _spreadsheet[row, col].Text = $"=B{row + 1}";
                }
                if (col == 1)
                {
                    _spreadsheet[row, col].Text = $"This is cell B{(row + 1):d2}";
                }

                int val = (row * ColumnCount) + col;
                if (randomCells.Contains(val))
                {
                    _spreadsheet[row, col].Text = $"Rando {++randomText}";
                }
            }
        }
    }

    public MainWindowViewModel()
    {
        Spreadsheet = new();
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
    }
}