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

    public override string ToString()
    {
        return $"({Row},{Col})[{Text}][{Value}]";
    }

    public string Value => _value;

    private int Row { get; } = row;
    private int Col { get;  } = col;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}