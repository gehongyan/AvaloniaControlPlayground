using System;

namespace AvaloniaControlPlayground.Controls;

public static class ArrowPlacementExtensions
{
    public static ArrowPointing GetPointingDirection(this ArrowPlacement arrowPlacement)
    {
        return arrowPlacement switch
        {
            ArrowPlacement.Left
                or ArrowPlacement.LeftEdgeAlignedBottom
                or ArrowPlacement.LeftEdgeAlignedTop => ArrowPointing.Left,
            ArrowPlacement.Top
                or ArrowPlacement.TopEdgeAlignedLeft
                or ArrowPlacement.TopEdgeAlignedRight => ArrowPointing.Top,
            ArrowPlacement.Right
                or ArrowPlacement.RightEdgeAlignedBottom
                or ArrowPlacement.RightEdgeAlignedTop => ArrowPointing.Right,
            ArrowPlacement.Bottom
                or ArrowPlacement.BottomEdgeAlignedLeft
                or ArrowPlacement.BottomEdgeAlignedRight => ArrowPointing.Bottom,
            _ => throw new NotSupportedException()
        };
    }

}
