using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;

namespace AvaloniaControlPlayground.Pages;

public partial class ArrowedContentControlPage : UserControl
{
    public static readonly IValueConverter DoubleThicknessConverter = new FuncValueConverter<double, Thickness>(x => new Thickness(x));

    public ArrowedContentControlPage()
    {
        InitializeComponent();
        ArrowContext.Text = CreateRandomWords();
    }

    private static string CreateRandomWords()
    {
        IEnumerable<string> words = Enumerable.Repeat(
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
            100);
        return string.Join(" ", words);
    }
}

