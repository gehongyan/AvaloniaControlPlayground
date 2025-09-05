using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace AvaloniaControlPlayground.Controls;

[TemplatePart(PartArrowedRectangle, typeof(ArrowedRectangle))]
[TemplatePart(PartContentPresenter, typeof(ContentPresenter))]
public class ArrowedContentControl : ContentControl
{
    private const string PartArrowedRectangle = "PART_ArrowedRectangle";
    private const string PartContentPresenter = "PART_ContentPresenter";

    private ArrowedRectangle? _arrowedRectangle;
    private ContentPresenter? _contentPresenter;

    static ArrowedContentControl()
    {
        ArrowSizeProperty.Changed.AddClassHandler<ArrowedContentControl>((control, args) =>
            control.UpdateContentPresenterMargin());
        ArrowRatioProperty.Changed.AddClassHandler<ArrowedContentControl>((control, args) =>
            control.UpdateContentPresenterMargin());
        ArrowRadiusProperty.Changed.AddClassHandler<ArrowedContentControl>((control, args) =>
            control.UpdateContentPresenterMargin());
        ArrowPlacementProperty.Changed.AddClassHandler<ArrowedContentControl>((control, args) =>
            control.UpdateContentPresenterMargin());
        ShowArrowProperty.Changed.AddClassHandler<ArrowedContentControl>((control, args) =>
            control.UpdateContentPresenterMargin());
        ArrowOffsetProperty.Changed.AddClassHandler<ArrowedContentControl>((control, args) =>
            control.UpdateContentPresenterMargin());
    }

    private void UpdateContentPresenterMargin()
    {
        if (_contentPresenter is null || _arrowedRectangle is null) return;
        double arrowActualHeight = GetArrowActualHeight();
        ArrowPointing arrowPointing = ArrowPlacement.GetPointingDirection();
        Thickness margin = arrowPointing switch
        {
            ArrowPointing.Left => new Thickness(arrowActualHeight, 0, 0, 0),
            ArrowPointing.Top => new Thickness(0, arrowActualHeight, 0, 0),
            ArrowPointing.Right => new Thickness(0, 0, arrowActualHeight, 0),
            ArrowPointing.Bottom => new Thickness(0, 0, 0, arrowActualHeight),
            _ => new Thickness(0)
        };
        _contentPresenter.Margin = margin;
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _arrowedRectangle = e.NameScope.Find<ArrowedRectangle>(PartArrowedRectangle);
        _contentPresenter = e.NameScope.Find<ContentPresenter>(PartContentPresenter);
        UpdateContentPresenterMargin();
    }

    public static readonly StyledProperty<double> ArrowSizeProperty =
        ArrowedRectangle.ArrowSizeProperty.AddOwner<ArrowedContentControl>();

    public static readonly StyledProperty<double> ArrowRadiusProperty =
        ArrowedRectangle.ArrowRadiusProperty.AddOwner<ArrowedContentControl>();

    public static readonly StyledProperty<double> ArrowRatioProperty =
        ArrowedRectangle.ArrowRatioProperty.AddOwner<ArrowedContentControl>();

    public static readonly StyledProperty<ArrowPlacement> ArrowPlacementProperty =
        ArrowedRectangle.ArrowPlacementProperty.AddOwner<ArrowedContentControl>();

    public static readonly StyledProperty<bool> ShowArrowProperty =
        ArrowedRectangle.ShowArrowProperty.AddOwner<ArrowedContentControl>();

    public static readonly StyledProperty<double> ArrowOffsetProperty =
        ArrowedRectangle.ArrowOffsetProperty.AddOwner<ArrowedContentControl>();

    public static readonly StyledProperty<IBrush?> FillProperty =
        Shape.FillProperty.AddOwner<ArrowedContentControl>();

    public static readonly StyledProperty<double> StrokeThicknessProperty =
        Shape.StrokeThicknessProperty.AddOwner<ArrowedContentControl>();

    public static readonly StyledProperty<IBrush?> StrokeProperty =
        Shape.StrokeProperty.AddOwner<ArrowedContentControl>();

    public double ArrowSize
    {
        get => GetValue(ArrowSizeProperty);
        set => SetValue(ArrowSizeProperty, value);
    }

    public double ArrowRatio
    {
        get => GetValue(ArrowRatioProperty);
        set => SetValue(ArrowRatioProperty, value);
    }

    public double ArrowRadius
    {
        get => GetValue(ArrowRadiusProperty);
        set => SetValue(ArrowRadiusProperty, value);
    }

    public ArrowPlacement ArrowPlacement
    {
        get => GetValue(ArrowPlacementProperty);
        set => SetValue(ArrowPlacementProperty, value);
    }

    public bool ShowArrow
    {
        get => GetValue(ShowArrowProperty);
        set => SetValue(ShowArrowProperty, value);
    }

    public double ArrowOffset
    {
        get => GetValue(ArrowOffsetProperty);
        set => SetValue(ArrowOffsetProperty, value);
    }

    public IBrush? Fill
    {
        get => GetValue(FillProperty);
        set => SetValue(FillProperty, value);
    }

    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public IBrush? Stroke
    {
        get => GetValue(StrokeProperty);
        set => SetValue(StrokeProperty, value);
    }

    private double GetArrowActualHeight()
    {
        if (!ShowArrow) return 0;
        double arrowHeight = ArrowSize * ArrowRatio;
        double arrowEdgeWidth = Math.Max(0, ArrowSize / 2 - ArrowRadius);
        double arrowEdgeHeight = arrowEdgeWidth * ArrowRatio * 2;
        return (arrowHeight + arrowEdgeHeight) / 2;
    }
}
