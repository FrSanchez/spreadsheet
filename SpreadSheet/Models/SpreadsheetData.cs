using System.Collections.Generic;
using System.Xml.Serialization;

namespace SpreadSheet.Models;

[XmlType("Spreadsheet")]
[XmlRoot("Spreadsheet")]
public class SpreadsheetData : List<RowData>;