using Avalonia.Controls.Shapes;

namespace AvaloniaControlPlayground.Data;

public readonly struct IndexedRectangle
{
    public int Index { get; init; }
    public Rectangle Rectangle { get; init; }
}
