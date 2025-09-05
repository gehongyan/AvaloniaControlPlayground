using System;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace AvaloniaControlPlayground.Themes;

public partial class AppTheme : Styles
{
    public AppTheme(IServiceProvider? serviceProvider = null)
    {
        AvaloniaXamlLoader.Load(serviceProvider, this);
    }
}

