using System;
using Avalonia;
using Avalonia.Media;

namespace AvaloniaControlPlayground.Controls;

public class GeometryBuilder
{
    private const double PiOver2 = 1.57079633; // 90 deg to rad

    public static void DrawRoundedCornersArrowRectangle(
        StreamGeometryContext context,
        Rect boundsRect,
        double radiusX,
        double radiusY,
        ArrowPlacement arrowPlacement,
        double arrowSize,
        double arrowRatio,
        double arrowRadius,
        double arrowOffset)
    {
        // The rectangle is constructed as follows:
        //
        //   (origin)
        //   Corner 4                    Corner 1
        //   Top/Left      Line 1        Top/Right
        //      \_  ╭-------------------╮  _/
        //          |                   |
        //   Line 4 |                   | Line 2
        //          |                   |
        //       _  ╰-------------------╯  _
        //      /          Line 3           \
        //   Corner 3                    Corner 2
        //   Bottom/Left                 Bottom/Right
        //
        // - Lines 1,3 follow the deflated rectangle bounds minus RadiusX
        // - Lines 2,4 follow the deflated rectangle bounds minus RadiusY
        // - All rectangle corners are constructed using elliptical arcs
        //
        // The arrow is constructed as follows, assuming the arrow is placed at the BottomEdgeAlignedLeft:
        //
        //          ╰----   -------------╯            ArrowSize
        //            /  \◡/                           \   /   ArrowHeight = ArrowSize * ArrowRatio
        //   ArrowOffset                                \◡/
        //                                           ArrowRadius
        // - When the arrow is places with specific alignment,
        //   the arrow offset represents the distance from the edge of the rectangle
        // - The arrow size represents the width of the arrow
        // - The arrow height is calculated as ArrowSize * ArrowRatio
        // - The arrow radius represents the radius of the arrow corners

        Size arcSize = new Size(radiusX, radiusY);
        ArrowPointing arrowPointing = arrowPlacement.GetPointingDirection();
        double arrowHeight = arrowSize * arrowRatio;
        double arrowEdgeWidth = Math.Max(0, arrowSize / 2 - arrowRadius);
        double arrowEdgeHeight = arrowEdgeWidth * arrowRatio * 2;
        double arrowActualHeight = (arrowHeight + arrowEdgeHeight) / 2;
        Rect rect = boundsRect.Deflate(arrowPointing switch
        {
            ArrowPointing.Left => new Thickness(arrowActualHeight, 0, 0, 0),
            ArrowPointing.Top => new Thickness(0, arrowActualHeight, 0, 0),
            ArrowPointing.Right => new Thickness(0, 0, arrowActualHeight, 0),
            ArrowPointing.Bottom => new Thickness(0, 0, 0, arrowActualHeight),
            _ => new Thickness(0)
        });

        Point arrowBaseCenter = arrowPlacement switch
        {
            ArrowPlacement.Left => new Point(rect.Left, (rect.Top + rect.Bottom) / 2),
            ArrowPlacement.LeftEdgeAlignedTop => new Point(rect.Left, rect.Top + arrowOffset + arrowSize / 2),
            ArrowPlacement.LeftEdgeAlignedBottom => new Point(rect.Left, rect.Bottom - arrowOffset - arrowSize / 2),
            ArrowPlacement.Top => new Point((rect.Left + rect.Right) / 2, rect.Top),
            ArrowPlacement.TopEdgeAlignedLeft => new Point(rect.Left + arrowOffset + arrowSize / 2, rect.Top),
            ArrowPlacement.TopEdgeAlignedRight => new Point(rect.Right - arrowOffset - arrowSize / 2, rect.Top),
            ArrowPlacement.Right => new Point(rect.Right, (rect.Top + rect.Bottom) / 2),
            ArrowPlacement.RightEdgeAlignedTop => new Point(rect.Right, rect.Top + arrowOffset + arrowSize / 2),
            ArrowPlacement.RightEdgeAlignedBottom => new Point(rect.Right, rect.Bottom - arrowOffset - arrowSize / 2),
            ArrowPlacement.Bottom => new Point((rect.Left + rect.Right) / 2, rect.Bottom),
            ArrowPlacement.BottomEdgeAlignedLeft => new Point(rect.Left + arrowOffset + arrowSize / 2, rect.Bottom),
            ArrowPlacement.BottomEdgeAlignedRight => new Point(rect.Right - arrowOffset - arrowSize / 2, rect.Bottom),
            _ => throw new NotSupportedException()
        };
        Point arrowEntrance = arrowPointing switch
        {
            ArrowPointing.Left => new Point(rect.Left, arrowBaseCenter.Y + arrowSize / 2),
            ArrowPointing.Top => new Point(arrowBaseCenter.X - arrowSize / 2, rect.Top),
            ArrowPointing.Right => new Point(rect.Right, arrowBaseCenter.Y - arrowSize / 2),
            ArrowPointing.Bottom => new Point(arrowBaseCenter.X + arrowSize / 2, rect.Bottom),
            _ => throw new NotSupportedException()
        };
        Point arrowArcEntrance = arrowPointing switch
        {
            ArrowPointing.Left => new Point(arrowEntrance.X - arrowEdgeHeight, arrowEntrance.Y - arrowEdgeWidth),
            ArrowPointing.Top => new Point(arrowEntrance.X + arrowEdgeWidth, arrowEntrance.Y - arrowEdgeHeight),
            ArrowPointing.Right => new Point(arrowEntrance.X + arrowEdgeHeight, arrowEntrance.Y + arrowEdgeWidth),
            ArrowPointing.Bottom => new Point(arrowEntrance.X - arrowEdgeWidth, arrowEntrance.Y + arrowEdgeHeight),
            _ => throw new NotSupportedException()
        };
        Point arrowArcControlPoint = arrowPointing switch
        {
            ArrowPointing.Left => new Point(arrowBaseCenter.X - arrowHeight, arrowBaseCenter.Y),
            ArrowPointing.Top => new Point(arrowBaseCenter.X, arrowBaseCenter.Y - arrowHeight),
            ArrowPointing.Right => new Point(arrowBaseCenter.X + arrowHeight, arrowBaseCenter.Y),
            ArrowPointing.Bottom => new Point(arrowBaseCenter.X, arrowBaseCenter.Y + arrowHeight),
            _ => throw new NotSupportedException()
        };
        Point arrowExit = arrowPointing switch
        {
            ArrowPointing.Left => new Point(rect.Left, arrowBaseCenter.Y - arrowSize / 2),
            ArrowPointing.Top => new Point(arrowBaseCenter.X + arrowSize / 2, rect.Top),
            ArrowPointing.Right => new Point(rect.Right, arrowBaseCenter.Y + arrowSize / 2),
            ArrowPointing.Bottom => new Point(arrowBaseCenter.X - arrowSize / 2, rect.Bottom),
            _ => throw new NotSupportedException()
        };
        Point arrowArcExit = arrowPointing switch
        {
            ArrowPointing.Left => new Point(arrowExit.X - arrowEdgeHeight, arrowExit.Y + arrowEdgeWidth),
            ArrowPointing.Top => new Point(arrowExit.X - arrowEdgeWidth, arrowExit.Y - arrowEdgeHeight),
            ArrowPointing.Right => new Point(arrowExit.X + arrowEdgeHeight, arrowExit.Y - arrowEdgeWidth),
            ArrowPointing.Bottom => new Point(arrowExit.X + arrowEdgeWidth, arrowExit.Y + arrowEdgeHeight),
            _ => throw new NotSupportedException()
        };

        // Begin the figure from the connection point of the Line 1 and Corner 4
        context.BeginFigure(new Point(rect.Left + radiusX, rect.Top), isFilled: true);

        // Line 1 + Corner 1
        if (arrowPointing is ArrowPointing.Top)
        {
            context.LineTo(arrowEntrance);
            context.LineTo(arrowArcEntrance);
            context.QuadraticBezierTo(arrowArcControlPoint, arrowArcExit);
            context.LineTo(arrowExit);
        }
        context.LineTo(new Point(rect.Right - radiusX, rect.Top));
        context.ArcTo(
            new Point(rect.Right, rect.Top + radiusY),
            arcSize,
            rotationAngle: PiOver2,
            isLargeArc: false,
            SweepDirection.Clockwise);

        // Line 2 + Corner 2
        if (arrowPointing is ArrowPointing.Right)
        {
            context.LineTo(arrowEntrance);
            context.LineTo(arrowArcEntrance);
            context.QuadraticBezierTo(arrowArcControlPoint, arrowArcExit);
            context.LineTo(arrowExit);
        }
        context.LineTo(new Point(rect.Right, rect.Bottom - radiusY));
        context.ArcTo(
            new Point(rect.Right - radiusX, rect.Bottom),
            arcSize,
            rotationAngle: PiOver2,
            isLargeArc: false,
            SweepDirection.Clockwise);

        // Line 3 + Corner 3
        if (arrowPointing is ArrowPointing.Bottom)
        {
            context.LineTo(arrowEntrance);
            context.LineTo(arrowArcEntrance);
            context.QuadraticBezierTo(arrowArcControlPoint, arrowArcExit);
            context.LineTo(arrowExit);
        }
        context.LineTo(new Point(rect.Left + radiusX, rect.Bottom));
        context.ArcTo(
            new Point(rect.Left, rect.Bottom - radiusY),
            arcSize,
            rotationAngle: PiOver2,
            isLargeArc: false,
            SweepDirection.Clockwise);

        // Line 4 + Corner 4
        if (arrowPointing is ArrowPointing.Left)
        {
            context.LineTo(arrowEntrance);
            context.LineTo(arrowArcEntrance);
            context.QuadraticBezierTo(arrowArcControlPoint, arrowArcExit);
            context.LineTo(arrowExit);
        }
        context.LineTo(new Point(rect.Left, rect.Top + radiusY));
        context.ArcTo(
            new Point(rect.Left + radiusX, rect.Top),
            arcSize,
            rotationAngle: PiOver2,
            isLargeArc: false,
            SweepDirection.Clockwise);

        context.EndFigure(isClosed: true);
    }
}
