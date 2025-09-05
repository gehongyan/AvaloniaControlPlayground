using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using AvaloniaControlPlayground.Helpers;

namespace AvaloniaControlPlayground.Controls.WaterfallPanel;

public class WaterfallPanel : Panel
{
    static WaterfallPanel()
    {
        AffectsMeasure<WrapPanel>(OrientationProperty, MaxColumnsProperty);
        AffectsArrange<WrapPanel>(OrientationProperty, MaxColumnsProperty);
    }

    /// <summary>
    ///     Defines the <see cref="Orientation"/> property.
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<WaterfallPanel, Orientation>(
            nameof(Orientation), Orientation.Vertical);

    /// <summary>
    ///     Gets or sets the orientation in which child controls will be laid out.
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public static readonly StyledProperty<int> MaxColumnsProperty =
        AvaloniaProperty.Register<WaterfallPanel, int>(
        nameof(MaxColumns), 1, validate: x => x > 0);

    public int MaxColumns
    {
        get => GetValue(MaxColumnsProperty);
        set => SetValue(MaxColumnsProperty, value);
    }

    public static readonly AttachedProperty<int> AssignedColumnProperty =
        AvaloniaProperty.RegisterAttached<WaterfallPanel, Control, int>("AssignedColumn");

    private static void SetAssignedColumn(Control obj, int value) => obj.SetValue(AssignedColumnProperty, value);
    public static int GetAssignedColumn(Control obj) => obj.GetValue(AssignedColumnProperty);

    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        Orientation orientation = Orientation;
        Span<double> widths = stackalloc double[MaxColumns];
        Span<double> offsets = stackalloc double[MaxColumns];
        foreach (Control child in Children)
        {
            UVSize size = new(orientation);
            child.Measure(Size.Infinity);
            size.U = child.DesiredSize.Height;
            size.V = child.DesiredSize.Width;
            int shortestIndex = GetLowestIndex(offsets);
            if (FuzzyMath.GreaterThan(size.Width, widths[shortestIndex]))
                widths[shortestIndex] = size.Width;
            offsets[shortestIndex] += size.Height;
        }

        UVSize desiredSize = new(orientation)
        {
            U = Max(offsets),
            V = Sum(widths)
        };
        return desiredSize.ToSize();
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(Size finalSize)
    {
        Orientation orientation = Orientation;
        Span<double> widths = stackalloc double[MaxColumns];
        Span<double> childOffsets = stackalloc double[Children.Count];
        Span<double> offsets = stackalloc double[MaxColumns];
        Span<int> columnIndices = stackalloc int[Children.Count];
        for (int i = 0; i < Children.Count; i++)
        {
            UVSize size = new(orientation);
            Control child = Children[i];
            size.U = child.DesiredSize.Height;
            size.V = child.DesiredSize.Width;
            int shortestIndex = GetLowestIndex(offsets);
            columnIndices[i] = shortestIndex;
            if (FuzzyMath.GreaterThan(size.Width, widths[shortestIndex]))
                widths[shortestIndex] = size.Width;
            childOffsets[i] = offsets[shortestIndex];
            offsets[shortestIndex] += size.Height;
        }

        Span<double> columnOffsets = stackalloc double[MaxColumns];
        for (int i = 0; i < MaxColumns; i++)
            columnOffsets[i] = Sum(widths, 0, i - 1);
        bool isVertical = orientation == Orientation.Vertical;
        for (int i = 0; i < Children.Count; i++)
        {
            Control child = Children[i];
            int columnIndex = columnIndices[i];
            SetAssignedColumn(child, columnIndex);

            child.Arrange(new Rect(
                isVertical ? columnOffsets[columnIndex] : childOffsets[i],
                isVertical ? childOffsets[i] : columnOffsets[columnIndex],
                child.DesiredSize.Width,
                child.DesiredSize.Height));
        }

        UVSize desiredSize = new(orientation)
        {
            U = Max(offsets),
            V = Sum(widths)
        };
        return desiredSize.ToSize();
    }

    private static int GetLowestIndex(Span<double> span)
    {
        if (span.IsEmpty) return -1;
        int lowestIndex = 0;
        double lowestValue = span[0];
        for (int i = 1; i < span.Length; i++)
        {
            if (!FuzzyMath.GreaterThan(lowestValue, span[i])) continue;
            lowestValue = span[i];
            lowestIndex = i;
        }

        return lowestIndex;
    }

    private static double Sum(Span<double> values) => Sum(values, 0, values.Length - 1);

    private static double Sum(Span<double> values, int start, int end)
    {
        if (values.IsEmpty) return 0;
        double sum = 0;
        for (int i = start; i <= end; i++)
        {
            double value = values[i];
            sum += value;
        }

        return sum;
    }

    private static double Max(Span<double> values)
    {
        if (values.IsEmpty) return 0;
        double max = 0;
        foreach (double value in values)
        {
            if (FuzzyMath.GreaterThan(value, max))
                max = value;
        }

        return max;
    }

    private struct UVSize
    {
        private readonly Orientation _orientation;

        internal UVSize(Orientation orientation)
        {
            U = V = 0d;
            _orientation = orientation;
        }

        public double U { get; set; }
        public double V { get; set; }

        internal double Width => _orientation == Orientation.Horizontal ? U : V;

        internal double Height => _orientation == Orientation.Horizontal ? V : U;

        public Size ToSize() => new(Width, Height);
    }
}
