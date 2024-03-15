using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine;

internal sealed class SpreadSheetCell : Cell
{
    
    public event PropertyChangingEventHandler? PropertyChanging = delegate { };

    public SpreadSheetCell(int row, int col) : this(row, col, string.Empty)
    {
        
    }

    private SpreadSheetCell(int row, int col, string value) : base(row, col)
    {
        Text = value;
        SetValue(value);
    }

    public void SetValue(string value)
    {
        if (value != _value)
        {
            _value = value;
            OnPropertyChanged(nameof(Value));
        }
    }

    public void Bind(Cell other)
    {
        other.PropertyChanged += OnOtherCellChanged;
    }

    public void UnBind(Cell other)
    {
        other.PropertyChanged -= OnOtherCellChanged;
    }

    private void OnOtherCellChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == "Value")
        {
            OnPropertyChanged(nameof(Text));
        }
    }

    public sealed override string Text
    {
        get => base.Text;
        set
        {
            OnPropertyChanging();
            base.Text = value;
        }
    }

    private void OnPropertyChanging([CallerMemberName] string? propertyName = null)
    {
        PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }
}