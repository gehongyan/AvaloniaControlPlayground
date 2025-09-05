using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaControlPlayground.Controls.WaterfallPanel;
using AvaloniaControlPlayground.Data;

namespace AvaloniaControlPlayground.Pages;

public partial class WaterfallPanelPage : UserControl
{
    private readonly IReadOnlyCollection<IndexedRectangle> _rectangles;

    public WaterfallPanelPage()
    {
        InitializeComponent();
        _rectangles = Enumerable
            .Range(1, (int)Math.Ceiling(CountSlider.Maximum))
            .Select(x => new Rectangle
            {
                Width = 100,
                Height = Random.Shared.Next(30, 100),
                Fill = RandomBrush()
            })
            .Select((x, i) => new IndexedRectangle
            {
                Index = i,
                Rectangle = x
            })
            .ToArray();
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        RectangleItemsControl.ItemsSource = _rectangles.Take((int)CountSlider.Value);
    }

    private static SolidColorBrush RandomBrush()
    {
        int r = Random.Shared.Next(0, 255);
        int g = Random.Shared.Next(0, 255);
        int b = Random.Shared.Next(0, 255);
        return new SolidColorBrush(Color.FromRgb((byte)r, (byte)g, (byte)b));
    }

    private void CountSlider_OnValueChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        if (sender is not Slider slider) return;
        UpdateItemsSource((int)slider.Value);
    }

    private void OrientationToggleSwitch_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (sender is not ToggleSwitch toggleSwitch) return;
        if (RectangleItemsControl.ItemsPanelRoot is not WaterfallPanel waterfallPanel) return;
        waterfallPanel.Orientation = toggleSwitch.IsChecked == true
            ? Orientation.Horizontal
            : Orientation.Vertical;
        UpdateItemsSource((int)CountSlider.Value);
    }

    private void UpdateItemsSource(int count)
    {
        RectangleItemsControl.ItemsSource = _rectangles.Take(count);
    }
}
