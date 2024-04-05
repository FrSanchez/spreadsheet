using Avalonia.Platform.Storage;

namespace SpreadSheet.Storage;

public class MyFilePickerFileTypes
{
    public static FilePickerFileType Csv { get; } = new("CSV File")
    {
        Patterns = new[] { "*.csv" },
        AppleUniformTypeIdentifiers = new[] { "public.plain-text" },
        MimeTypes = new[] { "text/csv" }
    };

    public static FilePickerFileType Xml { get; } = new("Xml File")
    {
        Patterns = new[] { "*.xml" },
        AppleUniformTypeIdentifiers = new[] { "public.xml-text" },
        MimeTypes = new[] { "text/xml" }
    };
}