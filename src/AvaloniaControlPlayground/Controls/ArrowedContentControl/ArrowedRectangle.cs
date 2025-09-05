using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Media;

namespace AvaloniaControlPlayground.Controls;

public class ArrowedRectangle : Rectangle
{
    public static readonly StyledProperty<double> ArrowSizeProperty =
        AvaloniaProperty.Register<ArrowedRectangle, double>(
            nameof(ArrowSize), defaultValue: 10);

    public static readonly StyledProperty<double> ArrowRadiusProperty =
        AvaloniaProperty.Register<ArrowedRectangle, double>(
            nameof(ArrowRadius), defaultValue: 2);

    public static readonly StyledProperty<double> ArrowRatioProperty =
        AvaloniaProperty.Register<ArrowedRectangle, double>(
            nameof(ArrowRatio), defaultValue: 0.6);

    public static readonly StyledProperty<ArrowPlacement> ArrowPlacementProperty =
        AvaloniaProperty.Register<ArrowedContentControl, ArrowPlacement>(
            nameof(ArrowPlacement));

    public static readonly StyledProperty<bool> ShowArrowProperty =
        AvaloniaProperty.Register<ArrowedContentControl, bool>(
            nameof(ShowArrow), defaultValue: true);

    public static readonly StyledProperty<double> ArrowOffsetProperty =
        AvaloniaProperty.Register<ArrowedRectangle, double>(
            nameof(ArrowOffset), defaultValue: 20);

    static ArrowedRectangle()
    {
        AffectsGeometry<ArrowedRectangle>(
            ArrowSizeProperty,
            ArrowRatioProperty,
            ArrowRadiusProperty,
            ArrowPlacementProperty,
            ArrowOffsetProperty,
            ShowArrowProperty,
            StrokeThicknessProperty);
    }

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

    /// <inheritdoc />
    protected override Geometry CreateDefiningGeometry()
    {
        if (!ShowArrow)
            return base.CreateDefiningGeometry();

        var rect = new Rect(Bounds.Size).Deflate(StrokeThickness / 2);
        StreamGeometry geometryStream = new();
        using StreamGeometryContext context = geometryStream.Open();
        GeometryBuilder.DrawRoundedCornersArrowRectangle(
            context,
            rect,
            RadiusX,
            RadiusY,
            ArrowPlacement,
            ArrowSize,
            ArrowRatio,
            ArrowRadius,
            ArrowOffset
        );

        return geometryStream;
    }
}
