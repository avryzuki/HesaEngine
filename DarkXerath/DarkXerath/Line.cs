using System;
using SharpDX;

namespace DarkXerath
{
    internal class Line
    {
        public Line(Vector2 start, Vector2 end)
        {
            pointA = start;
            pointB = end;
            A = pointA.Y - pointB.Y;
            B = pointB.X - pointA.X;
            C = -A * pointA.X - B * pointA.Y;
        }

        public bool Contains(Vector2 point)
        {
            return Math.Abs(A * point.X + B * point.Y + C) < float.Epsilon;
        }

        public bool IsOnLine(Vector2 point, float halfWidth, float boundingRadius = 0)
        {
            return DistanceTo(point) <= boundingRadius + halfWidth;
        }

        public float DistanceTo(Vector2 point)
        {
            return Math.Abs(A * point.X + B * point.Y + C) / (float) Math.Sqrt(A * A + B * B);
        }

        public Vector2 PointProjectOf(Vector2 point)
        {
            Vector2 vec = pointB - pointA;
            float tmp = (-vec.X * (pointA.X - point.X) - vec.Y * (pointA.Y - point.Y)) / (vec.X * vec.X + vec.Y * vec.Y);
            return new Vector2(pointA.X + vec.X*tmp, pointA.Y + vec.Y * tmp);
        }

        public readonly Vector2 pointA, pointB;
        private readonly float A, B, C;
    }

    internal class LineSegment
    {
        public LineSegment(Vector2 start, Vector2 end)
        {
            line = new Line(start, end);
        }

        public bool Contains(Vector2 point, bool done)
        {
            if (done || line.Contains(point))
            {
                float v1 = 0f, v2 = 0f;
                Vector2 pointA = line.pointA, pointB = line.pointB;
                if (pointA.X == pointB.X)
                {
                    v1 = pointA.Y - point.Y;
                    v2 = pointB.Y - point.Y;
                }
                else
                {
                    v1 = pointA.X - point.X;
                    v2 = pointB.X - point.X;
                }
                return v1 == 0f || v2 == 0f || (v1 > 0f && v2 < 0f) || (v1 < 0f && v2 > 0f);
            }
            return false;
        }

        public bool IsOnLine(Vector2 point, float halfWidth, float boundingRadius = 0)
        {
            return Contains(line.PointProjectOf(point), true) && line.IsOnLine(point, halfWidth, boundingRadius);
        }

        public float Length()
        {
            return (line.pointB - line.pointA).Length();
        }

        private readonly Line line;
    }
}
