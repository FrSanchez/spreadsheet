using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine;

public abstract class Cell(int row, int col) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    private string _text = "";
    protected string _value = "";
    public virtual string Text
    {
        get => _text;
        set
        {
            if (_text.Equals(value)) return;
            _text = value;
            OnPropertyChanged();

        }
    }

    private uint _color = 0xffffffff;

    public virtual uint BackgroundColor
    {
        get => _color;
        set
        {
            if (_color.Equals(value)) return;
            _color = value;
            OnPropertyChanged();
        }
    }

    public override string ToString()
    {
        return $"({Row},{Col})[{Text}][{Value}]";
    }

    public string Value => _value;

    public int Row { get; } = row;
    public int Col { get;  } = col;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}