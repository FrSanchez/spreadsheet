using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Engine;

public abstract class Cell : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged = delegate { };

    private string _text;
    protected string _value;
    public virtual string Text
    {
        get { return _text;}
        set
        {
            if (!_text.Equals(value))
            {
                _text = value;
                OnPropertyChanged();
            }
            
        }
    }

    public override string ToString()
    {
        return $"({Row},{Col})[{Text}][{Value}]";
    }

    public string Value
    {
        get { return _value; }
    }

    protected Cell(int row, int col)
    {
        Row = row;
        Col = col;
        _text = "";
        _value = "";
    }

    public int Row { get; }
    public int Col { get;  }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}