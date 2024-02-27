using System.Collections;
using System.Collections.Generic;

namespace SpreadSheet.Models;
using Engine;

public class Spreadsheet : IEnumerable<IEnumerable<Cell>>
{
    private readonly SpreadSheet _spreadSheet;

    public Spreadsheet(SpreadSheet spreadSheet)
    {
        _spreadSheet = spreadSheet;
    }
    public IEnumerator<IEnumerable<Cell>> GetEnumerator()
    {
        return _spreadSheet.GetRows().GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _spreadSheet.GetRows().GetEnumerator();
    }

    public string this[int row, int col]
    {
        get => _spreadSheet.GetCell(row, col).Value;
        set => _spreadSheet.SetCell(row, col, value);
    }
}