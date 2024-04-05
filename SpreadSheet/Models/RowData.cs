using System.Collections.Generic;
using System.Xml.Serialization;

namespace SpreadSheet.Models;

[XmlType("Row")]
[XmlRoot("Row")]
public class RowData : List<CellData>;