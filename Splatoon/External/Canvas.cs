namespace Splatoon.External
{
    internal unsafe static class Canvas
    {
        // ----------- actor-aware draw methods --------------
        internal static void ActorConeXZ(GameObject actor, float radius, float startRads, float endRads, Brush brush)
        {
            ConeXZ(actor.Position, radius, startRads + actor.Rotation, endRads + actor.Rotation, brush);
        }

        internal static void ActorArrowXZ(GameObject actor, float radius, float angle, float scale, Brush brush)
        {
            var direction = angle + actor.Rotation;

            // scale the drawing by shifting the "circle center" up the radial
            // and reducing the radius accordingly
            var centerOffset = radius * (1 - scale);
            var pos = actor.Position + new Vector3(
                centerOffset * (float)Math.Sin(direction),
                0,
                centerOffset * (float)Math.Cos(direction)
            );
            var arrowSize = radius - centerOffset;

            // edge case: when == 1 and there is a thickness, the arrow pokes out the sides.
            var drawBottom = scale != 1f;
            var shape = new ConvexShape(brush);
            if (drawBottom) shape.Point(pos);
            shape.PointRadial(pos, arrowSize, direction + Maths.Radians(90));
            shape.PointRadial(pos, arrowSize, direction + Maths.Radians(0));
            shape.PointRadial(pos, arrowSize, direction + Maths.Radians(-90));
            if (drawBottom) shape.Point(pos);
            shape.Done();
        }

        internal static void ActorDonutSliceXZ(GameObject actor, float innerRadius, float outerRadius, float startRads, float endRads, Brush brush)
        {
            DonutSliceXZ(actor.Position, innerRadius, outerRadius, startRads + actor.Rotation, endRads + actor.Rotation, brush);
        }

        internal static void CircleXZ(Vector3 position, float radius, Brush brush)
        {
            CircleArcXZ(position, radius, 0f, Maths.TAU, brush);
        }

        // ----------- position-based draw methods --------------
        internal static void ConeXZ(Vector3 center, float radius, float startRads, float endRads, Brush brush)
        {
            var shape = new ConvexShape(brush);
            shape.Point(center);
            shape.Arc(center, radius, startRads, endRads);
            shape.Point(center);
            shape.Done();
        }

        internal static void DonutSliceXZ(Vector3 center, float innerRadius, float outerRadius, float startRads, float endRads, Brush brush)
        {
            if (innerRadius == 0 && endRads - startRads <= (Maths.PI + Maths.Epsilon))
            {
                // special case: a cone, which is a convex polygon
                ConeXZ(center, outerRadius, startRads, endRads, brush);
                return;
            }

            // a donut slice is a non-convex object so is not cleanly handled by imgui
            // instead, approximate with slices
            var segments = Maths.ArcSegments(startRads, endRads);
            var radsPerSegment = (endRads - startRads) / (float)segments;

            // outline
            var outlineBrush = brush with { Fill = new() };
            var outline = new ConvexShape(outlineBrush);
            outline.Arc(center, outerRadius, startRads, endRads);
            outline.Arc(center, innerRadius, endRads, startRads);
            outline.PointRadial(center, outerRadius, startRads);
            outline.Done();

            // fill
            if (brush.HasFill())
            {
                var sliceBrush = brush with { Thickness = 0f };
                for (var i = 0; i < segments; i++)
                {
                    var start = startRads + i * radsPerSegment;
                    var end = startRads + (i + 1) * radsPerSegment;

                    var shape = new ConvexShape(sliceBrush);
                    shape.Arc(center, outerRadius, start, end);
                    shape.Arc(center, innerRadius, end, start);
                    shape.PointRadial(center, outerRadius, start);
                    shape.Done();
                }
            }
        }

        internal static void ConeCenteredXZ(Vector3 center, float radius, float directionRads, float angleRads, Brush brush)
        {
            var startRads = directionRads - (angleRads / 2);
            var endRads = directionRads + (angleRads / 2);

            ConeXZ(center, radius, startRads, endRads, brush);
        }

        internal static void CircleArcXZ(Vector3 gamePos, float radius, float startRads, float endRads, Brush brush)
        {
            var shape = new ConvexShape(brush);
            shape.Arc(gamePos, radius, startRads, endRads);
            shape.Done();
        }

        internal static void Segment(Vector3 startPos, Vector3 endPos, Brush brush)
        {
            var shape = new ConvexShape(brush);
            shape.Point(startPos);
            shape.Point(endPos);
            shape.Done();
        }
    }
}