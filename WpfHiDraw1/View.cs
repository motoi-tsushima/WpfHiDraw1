//#define DrawingContext_DrawLine

//#define DrawingContext_DrawGeometry_LineTo
//#define DrawingContext_DrawGeometry_LineTo_OneGeometry
//#define DrawingContext_DrawGeometry_PolyLineTo_MultiGeometry
//#define DrawingContext_DrawGeometry_PolyLineTo_OneGeometry

//#define DrawingContext_DrawPathGeometry_LineTo
#define DrawingContext_DrawPathGeometry_LineTo_OneGeometry

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace WpfHiDraw1
{
    public class View : Control
    {
        public List<Tuple<Point, Point>> Points;
        public Label ResultLabel;
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (Points == null || ResultLabel == null)
            {
                return;
            }
            var pen = new Pen(Brushes.Aqua, 1);
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();

#if DrawingContext_DrawLine

            foreach (Tuple<Point, Point> point in Points)
            {
                drawingContext.DrawLine(pen, point.Item1, point.Item2);
            }

#elif DrawingContext_DrawGeometry_LineTo

            foreach (Tuple<Point, Point> point in Points)
            {
                var geometry = new StreamGeometry();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(point.Item1, false, false);
                    context.LineTo(point.Item2, true, false);
                }
                drawingContext.DrawGeometry(null, pen, geometry);
            }

#elif DrawingContext_DrawPathGeometry_LineTo

            foreach (Tuple<Point, Point> point in Points)
            {
                PathFigure pointFigure = new PathFigure();
                pointFigure.StartPoint = new Point(point.Item1.X, point.Item1.Y);
                pointFigure.Segments.Add(new LineSegment(new Point(point.Item2.X, point.Item2.Y), true));

                var geometry = new PathGeometry();
                geometry.Figures.Add(pointFigure);

                drawingContext.DrawGeometry(null, pen, geometry);
            }

#elif DrawingContext_DrawGeometry_LineTo_OneGeometry

            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                foreach (Tuple<Point, Point> point in Points)
                {
                    context.BeginFigure(point.Item1, false, false);
                    context.LineTo(point.Item2, true, false);
                }
            }
            drawingContext.DrawGeometry(null, pen, geometry);

#elif DrawingContext_DrawPathGeometry_LineTo_OneGeometry

            var geometry = new PathGeometry();

            foreach (Tuple<Point, Point> point in Points)
            {
                PathFigure pointFigure = new PathFigure();
                pointFigure.StartPoint = new Point(point.Item1.X, point.Item1.Y);
                pointFigure.Segments.Add(new LineSegment(new Point(point.Item2.X, point.Item2.Y), true));
                geometry.Figures.Add(pointFigure);
            }
            drawingContext.DrawGeometry(null, pen, geometry);

#elif DrawingContext_DrawGeometry_PolyLineTo_MultiGeometry

            foreach (Tuple<Point, Point> point in Points)
            {
                var geometry = new StreamGeometry();
                using (var context = geometry.Open())
                {
                    context.BeginFigure(point.Item1, false, false);
                    context.PolyLineTo(new Point[] { point.Item2 }, true, false);
                }
                drawingContext.DrawGeometry(null, pen, geometry);
            }

#elif DrawingContext_DrawGeometry_PolyLineTo_OneGeometry

            var geometry = new StreamGeometry();
            using (var context = geometry.Open())
            {
                foreach (Tuple<Point, Point> point in Points)
                {
                    context.BeginFigure(point.Item1, false, false);
                    context.PolyLineTo(new Point[] { point.Item2 }, true, false);
                }
            }
            drawingContext.DrawGeometry(null, pen, geometry);

#endif
            sw.Stop();
            ResultLabel.Content = "Result: " + sw.ElapsedMilliseconds.ToString() + " ms";
        }
    }
}
