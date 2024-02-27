using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using SpreadSheet.Models;

namespace SpreadSheet.ViewModels;

using Engine;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<IEnumerable<Cell>> Spreadsheet { get; }
    
    private Random rand = new ();
    public void PerformAction(object msg)
    {
        for (int i = 0; i < Spreadsheet.Count; i++)
        {
            foreach (Cell cell in Spreadsheet[i])
            {
                var text = String.Format("#{0:x4}", rand.Next(0xFFFF));
                cell.Text = $"{text}";
            }
        }
    }

    public MainWindowViewModel()
    {
        
        var spreadsheet = new Spreadsheet(new SpreadSheet(50, 26));
        for (int r = 0; r < 50; r++)
        {
            for (int c = 0; c < 26; c++)
            {
                var text = String.Format("#{0:x4}", rand.Next(0xFFFF));
                spreadsheet[r, c] = $"{text}";
            }
        }

        Spreadsheet = new ObservableCollection<IEnumerable<Cell>>(spreadsheet);
    }
}