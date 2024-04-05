using System.ComponentModel;
using System.Xml.Serialization;
using Engine;

namespace SpreadSheet.Models;

[XmlType("Cell")]
[XmlRoot("Cell")]
public class CellData 
{
    [ReadOnly(true)]
    [XmlElement("Text")]
    public string Text { get; set; }
    
    [ReadOnly(true)]
    [XmlElement("Color")]
    public uint BackgroundColor { get; set;  }

    public CellData()
    {
        Text = "\0\0";
        BackgroundColor = 0xFFFFFFFF;
    }

    public CellData(Cell cell)
    {
        Text = cell.Text;
        BackgroundColor = cell.BackgroundColor;
    }

}