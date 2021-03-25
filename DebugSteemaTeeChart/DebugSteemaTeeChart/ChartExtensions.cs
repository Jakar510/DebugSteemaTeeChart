using System;
using System.Linq;
using Steema.TeeChart;
using Steema.TeeChart.Drawing;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using Xamarin.Forms;
using SAspect = Steema.TeeChart.Drawing.Aspect;

#nullable enable
namespace DebugSteemaTeeChart
{
	public static class ChartExtensions
	{
		public const double ANNOTATION_OFFSET = 10;
		public const double MIN_ANNOTATION_OFFSET = 5;
		public const string FONT = "Segoe UI";

		public static void ConfigureAxes( this Chart chart )
		{
			chart.Axes.Depth.ConfigureDepth();
			chart.Axes.DepthTop.ConfigureDepth();

			chart.Axes.Right.ConfigureRight();
			chart.Axes.Left.ConfigureLeft();
			chart.Axes.Bottom.ConfigureBottom();
		}
		public static void ConfigureDepth( this DepthAxis axis )
		{
			axis.Title.Font.Name = FONT;
			axis.Title.Font.Size = 13;
			axis.Visible = false;
			axis.Automatic = true;
		}
		public static void ConfigureRight( this Axis right )
		{
			right.AxisPen.Width = 0;
			right.Grid.Visible = false;
			right.Labels.Font.Brush.Color = Color.Gray;
			right.Labels.Font.Name = FONT;
			right.Labels.Font.Size = 31;
			right.Labels.Visible = false;
			right.MinorTicks.Visible = false;
			right.Ticks.Visible = false;
			right.Title.Font.Name = FONT;
		}
		public static void ConfigureLeft( this Axis left )
		{
			left.AxisPen.Visible = false;
			left.AxisPen.Width = 0;
			left.Grid.Color = Color.LightGray; //FromRgb(221, 221, 221); // light gray 

			left.MinorTicks.Visible = false;
			left.TickOnLabelsOnly = false;
			left.Grid.Visible = true;
			//left.Labels.Items
			left.Labels.AutoSize = true;
			left.Labels.Visible = true;
			left.Labels.MultiLine = true;
			left.Labels.Align = AxisLabelAlign.Default;
			left.Labels.Font.Brush.Color = Color.Black;
			left.Labels.Font.Name = FONT;
			left.Labels.Angle = 0;

			left.Ticks.Visible = true;
			left.Ticks.Color = Color.DarkGray;
			left.Ticks.Length = 5;
			left.Title.Font.Brush.Color = Color.DarkGray;
			left.Title.Font.Name = FONT;
			left.Automatic = true;
			left.ZoomLimitToAxisBounds = true;
		}
		public static void ConfigureBottom( this Axis bottom )
		{
			bottom.Horizontal = true;
			bottom.AxisPen.Color = Color.FromRgb(110, 110, 110); // dark gray 
			bottom.AxisPen.Width = 0;
			bottom.Grid.Color = Color.FromRgb(221, 221, 221); // light gray 
			bottom.Grid.Visible = true;
			bottom.Ticks.Color = Color.FromRgb(128, 128, 128); // gray 
			bottom.MinorTicks.Visible = false;
			bottom.Title.Caption = "Date";
			bottom.Title.Font.Name = FONT;
			bottom.TitleSize = 18;
			bottom.Labels.Font.Brush.Color = Color.Gray;
			bottom.Labels.Font.Name = FONT;
			bottom.Labels.Font.Size = 10;
			bottom.Labels.Angle = 0;
			bottom.MinAxisIncrement = .5;
			bottom.Automatic = true;
			bottom.AutomaticMaximum = true;
			bottom.AutomaticMinimum = true;
			bottom.ZoomLimitToAxisBounds = true;
			bottom.MinorTickCount = 4;
		}


		public static void Title( this Chart chart, string? title )
		{
			chart.Title.Font.Brush.Color = Color.Blue;
			chart.Title.Font.Bold = true;
			chart.Title.Font.Brush.Color = Color.Black;
			chart.Title.Font.Name = FONT;
			chart.Title.Font.Shadow.Visible = false;
			chart.Title.Font.Size = 18;
			chart.Title.AutoSize = true;
			chart.Title.TextAlign = TextAlignment.Center;
			chart.Title.Alignment = TextAlignment.Center;
			chart.Title.TextFormat = TextFormat.Normal;
			chart.Title.ShapeStyle = TextShapeStyle.RoundRectangle;
			chart.Title.Visible = true;
			chart.Title.Text = title;
		}
		public static void Legend( this Chart chart )
		{
			chart.Legend.Alignment = LegendAlignments.Bottom;
			chart.Legend.Visible = false;
			chart.Legend.Font.Brush.Color = Color.Gray;
			chart.Legend.Font.Name = FONT;
			chart.Legend.Font.Size = 15;
			chart.Legend.Pen.Visible = false;
			chart.Legend.Shadow.Visible = true;
		}
		public static void Walls( this Chart chart )
		{
			chart.Panel.Gradient.Visible = false;
			chart.Panel.Bevel.Outer = BevelStyles.None;
			chart.Panel.Bevel.Width = 2;
			chart.Panel.BevelWidth = 2;
			chart.Panel.Brush.Color = Color.White;

			chart.Walls.Back.Transparent = false;
			chart.Walls.Back.Brush.Gradient.EndColor = Color.White;
			chart.Walls.Back.Pen.Visible = false;
			chart.Walls.Back.Visible = false;

			chart.Walls.Bottom.Brush.Gradient.EndColor = Color.Silver;
			chart.Walls.Bottom.Brush.Gradient.StartColor = Color.Gray;
			chart.Walls.Bottom.Brush.Gradient.Visible = true;
			chart.Walls.Bottom.Pen.Color = Color.Gray;
			chart.Walls.Bottom.Transparent = true;

			chart.Walls.Left.Brush.Color = Color.White;
			chart.Walls.Left.Brush.Gradient.EndColor = Color.Silver;
			chart.Walls.Left.Brush.Gradient.StartColor = Color.Gray;
			chart.Walls.Left.Brush.Gradient.Visible = true;
			chart.Walls.Left.Pen.Color = Color.Gray;
			chart.Walls.Left.Transparent = true;
			chart.Walls.Right.Transparent = true;
		}


		public static Line CreateLine( this Chart chart )
		{
			var line = new Line(chart)
					   {
						   ColorEach = false,
						   ColorEachPoint = true,
						   ColorEachLine = false,
						   TreatNaNAsNull = true,
						   TreatNulls = TreatNullsStyle.Skip,
						   Color = Color.Black,
						   Visible = true,
						   Active = true,
						   ClickableLine = false,
						   ShowInLegend = false
					   };

			line.Marks.ConfigureMarks();
			line.Pointer.ConfigurePointer();

			line.Brush.Color = Color.Black;
			line.LinePen.Color = Color.Black;

			line.ConfigureLineSize();

			line.DateTimeFormat = "MMM dd, yyyy";
			line.XValues.DateTime = true;
			line.XValues.Order = ValueListOrder.Ascending;

			chart.Series.Add(line);

			return line;
		}
		public static void ConfigureLineSize( this Line line )
		{
			switch ( Device.RuntimePlatform )
			{
				case Device.Android:
				{
					line.LinePen.Width = 4;
					line.Pointer.VertSize = 16;

					break;
				}

				case Device.iOS:
				{
					line.LinePen.Width = 2;
					line.Pointer.VertSize = 6;

					break;
				}

				default:
				{
					line.LinePen.Width = 4;
					line.Pointer.VertSize = 8;

					break;
				}
			}
		}
		public static void ConfigurePointer( this SeriesPointer pointer )
		{
			pointer.Gradient.Transparency = 2;
			pointer.HorizSize = 6;
			pointer.VertSize = 6;
			pointer.Pen.Width = 3;
			pointer.SizeDouble = 0D;
			pointer.SizeUnits = PointerSizeUnits.Pixels;
			pointer.Style = PointerStyles.Circle;
			pointer.Visible = true;
		}
		public static void ConfigureMarks( this SeriesMarks marks )
		{
			marks.Arrow.Visible = false;
			marks.ArrowLength = 0;
			marks.Font.Name = FONT;
			marks.Font.Size = 8;
			marks.Color = Color.GhostWhite;
			marks.BackColor = Color.SteelBlue;
			marks.Transparent = false;
			marks.CheckMarkArrowColor = true;
			marks.AutoSize = true;
			marks.AutoPosition = true;
			marks.FollowSeriesColor = true;
			marks.Visible = false;
			marks.ClipText = true;
		}


		public static void PanningZoom( this Chart chart, bool allow )
		{
			chart.Aspect.Orthogonal = false;
			chart.Aspect.ClipPoints = true;
			chart.Aspect.ZoomText = true;
			chart.Aspect.View3D = false;
			chart.Aspect.ExtendAxes = false;
			chart.Aspect.ZoomStyle = SAspect.ZoomStyles.FullChart;
			chart.Aspect.ZoomScrollStyle = SAspect.ZoomScrollStyles.Auto;
			chart.Aspect.ClipPoints = true;
			chart.Zoom.Direction = ZoomDirections.Both;
			chart.Zoom.Active = true;
			chart.Zoom.History = true;
			chart.Zoom.Allow = allow;

			chart.Panning.Active = allow;
			chart.Panning.Allow = allow
									  ? ScrollModes.Horizontal
									  : ScrollModes.None;
			chart.Panning.InsideBounds = true;

			chart.AutoRepaint = true;
		}


		public static double GetLocalMaximum( this Line line, bool isVertical )
		{
			var max = double.MinValue;

			//for (int i = 0; i < line.Count; i++)
			for ( int i = line.FirstVisibleIndex; i <= line.LastVisibleIndex; i++ )
			{
				double v = isVertical
							   ? line.CalcYPos(i)
							   : line.CalcXPos(i);

				max = Math.Max(max, v);
			}

			return max;
		}
		public static double GetLocalMinimum( this Line line, bool isVertical )
		{
			var min = double.MaxValue;

			//for (int i = 0; i < line.Count; i++)
			for ( int i = line.FirstVisibleIndex; i <= line.LastVisibleIndex; i++ )
			{
				double v = isVertical
							   ? line.CalcYPos(i)
							   : line.CalcXPos(i);

				min = Math.Min(min, v);
			}

			return min;
		}
		public static double LocalAveragePos( this Line line, out double min, out double max, bool vertical )
		{
			min = line.GetLocalMinimum(vertical);
			max = line.GetLocalMaximum(vertical);

			return Average(min, max);
		}

		public static double AverageWidth( this Rectangle bounds ) => Average(bounds.Left, bounds.Right);
		public static double AverageHeight( this Rectangle bounds ) => Average(bounds.Top, bounds.Bottom);
		public static double Average( params double[] values ) => values.Sum() / values.Length;


		public static void ConfigureAnnotations( this AnnotationTool tool, bool floating ) => tool.ConfigureAnnotations(floating, string.Empty);
		public static void ConfigureAnnotations( this AnnotationTool tool, bool floating, string text )
		{
			tool.ClipText = false;
			tool.Shape.TextFormat = TextFormat.Html;
			tool.Shape.Font.Size = 15;

			tool.Position = floating
								? AnnotationPositions.Custom
								: AnnotationPositions.RightTop;

			tool.Text = text;
			// tool.AutoSize = true;
			tool.Active = !string.IsNullOrWhiteSpace(text);
			if ( tool.Active )
			{
				tool.Width = tool.Chart.Graphics3D.TextWidth(text);
				tool.Height = tool.Chart.Graphics3D.TextHeight(text);
			}

			tool.Invalidate();
		}


		/*
			    (x, y)
	(0,0)
		______________________
		|                    |
		|                    |
		|                    |
		|                    |
		|                    |
		|                    |
		|                    |
		|                    |
		----------------------

		 */
		public static Point GetPoint( this AnnotationTool tool,
									  in Chart chart,
									  in double offset,
									  in double x,
									  in double y,
									  in double minVerticalValueHeight,
									  in double maxVerticalValueHeight,
									  in bool IsPhone,
									  in Quadrant quadrant )
		{
			Rectangle bounds = tool.GetBounds();

			return tool.GetPoint(chart.ChartBounds,
								 chart.Title.Top + chart.Title.Height,
								 chart.Axes.Bottom.Position,
								 offset,
								 ANNOTATION_OFFSET,
								 bounds,
								 x,
								 y,
								 minVerticalValueHeight,
								 maxVerticalValueHeight,
								 IsPhone,
								 quadrant);
			// leftBound,
			// topBound);
		}
		public static Point GetPoint( this AnnotationTool tool,
									  in Rectangle chartBounds,
									  in double titleBottomY,
									  in double bottomAxisY,
									  in double offset,
									  in double constant,
									  in Rectangle annotationBounds,
									  in double x,
									  in double y,
									  in double minVerticalValueHeight,
									  in double maxVerticalValueHeight,
									  in bool IsPhone,
									  in Quadrant quadrant,
									  in int iteration = 0 )
		{
			double top;
			double left;

			if ( IsPhone )
			{
				// phone mode: keep out of the way as much as possible
				if ( quadrant.HasFlag(Quadrant.Top) )
				{
					top = minVerticalValueHeight - annotationBounds.Height - offset;
					if ( top <= titleBottomY + constant ) { top = titleBottomY + offset; }
				}
				else
				{
					top = maxVerticalValueHeight + offset;
					if ( top + annotationBounds.Height >= bottomAxisY ) { top = bottomAxisY - annotationBounds.Height - constant; }
				}

				if ( quadrant.HasFlag(Quadrant.Left) ) { left = chartBounds.Left + constant; }
				else { left = chartBounds.Right - annotationBounds.Width - constant; }
			}
			else
			{
				// tablet mode: track position as closely as possible
				if ( quadrant.HasFlag(Quadrant.Top) )
				{
					top = y - annotationBounds.Height - offset;
					if ( top <= titleBottomY + constant ) { top = titleBottomY + constant + offset; }
				}
				else
				{
					top = y + offset;
					if ( top + annotationBounds.Height >= bottomAxisY ) { top = bottomAxisY - annotationBounds.Height - offset; }
				}

				if ( quadrant.HasFlag(Quadrant.Left) ) { left = x - offset; }
				else { left = x + offset; }
			}

			tool.Bounds = new Rectangle(left, top, annotationBounds.Width, annotationBounds.Height);

			if ( iteration >= 3 || !tool.Bounds.Contains(x, y) ) return quadrant.GetSourcePoint(IsPhone, left, left + annotationBounds.Width, top, top + annotationBounds.Height);

			Quadrant newQuadrant;
			if ( iteration < 2 )
			{
				newQuadrant = quadrant & Quadrant.Top;
				if ( !quadrant.HasFlag(Quadrant.Left) ) { newQuadrant |= Quadrant.Left; }
			}
			else { newQuadrant = quadrant | Quadrant.Left; }

			// ReSharper disable once TailRecursiveCall
			return tool.GetPoint(chartBounds,
								 titleBottomY,
								 bottomAxisY,
								 offset,
								 constant,
								 annotationBounds,
								 x,
								 y,
								 minVerticalValueHeight,
								 maxVerticalValueHeight,
								 IsPhone,
								 newQuadrant,
								 iteration + 1);
		}
		public static Point GetSourcePoint( this Quadrant quadrant,
											in bool IsPhone,
											in double left,
											in double right,
											in double top,
											in double bottom )
		{
			if ( IsPhone )
			{
				if ( quadrant.HasFlag(Quadrant.Top) )
				{
					return quadrant.HasFlag(Quadrant.Left)
							   ? new Point(right, bottom)
							   : new Point(left, bottom);
				}

				return quadrant.HasFlag(Quadrant.Left)
						   ? new Point(right, top)
						   : new Point(left, top);
			}

			if ( quadrant.HasFlag(Quadrant.Top) )
			{
				return quadrant.HasFlag(Quadrant.Left)
						   ? new Point(left, bottom)
						   : new Point(right, bottom);
			}

			return quadrant.HasFlag(Quadrant.Left)
					   ? new Point(left, top)
					   : new Point(right, top);
		}


		public static void CoerceVisibleAnnotation( this AnnotationTool tool, in Rectangle chart, in Rectangle title, in double offset )
		{
			Rectangle startBounds = tool.Bounds;
			double width = tool.Width;
			double height = tool.Height;
			if ( tool.Bounds.Right > chart.Right ) { tool.Left = chart.Right - width - offset; }

			if ( tool.Left <= chart.Left ) { tool.Left = offset; }

			if ( tool.Top <= title.Bottom ) { tool.Top = title.Bottom + offset; }

			if ( tool.Bounds.Bottom >= chart.Bottom ) { tool.Top = chart.Bottom - height - offset; }

			Console.WriteLine($"\n\n		startBounds: {startBounds}  |  final: {tool.Bounds}  |  chart.bounds: {chart}  |  title.bounds: {title}		\n\n");
		}


		public static Point GetSourcePoint( this AnnotationTool tool, in Quadrant quadrant )
		{
			Rectangle bounds = tool.Bounds; // forces a refresh.
			double width = Math.Max(bounds.Width, tool.Width);
			double height = Math.Max(bounds.Height, tool.Height);
			return tool.GetSourcePoint(width, height, quadrant);
		}
		public static Point GetSourcePoint( this AnnotationTool tool, in double widthAnnotation, in double heightAnnotation, in Quadrant quadrant )
		{
			if ( quadrant.HasFlag(Quadrant.Top) )
			{
				return quadrant.HasFlag(Quadrant.Left)
						   ? new Point(tool.Left, tool.Top + heightAnnotation)
						   : new Point(tool.Left - widthAnnotation, tool.Top + heightAnnotation);
			}

			return quadrant.HasFlag(Quadrant.Left)
					   ? new Point(tool.Left, tool.Top)
					   : new Point(tool.Left + widthAnnotation, tool.Top);
		}


		public static Quadrant ToQuadrant( in bool topBound, in bool leftBound )
		{
			return topBound switch
				   {
					   // true when leftBound => Quadrant.TopLeft,
					   // true => Quadrant.TopRight,
					   // false when leftBound => Quadrant.BottomLeft,
					   // false => Quadrant.BottomRight
					   true when leftBound => Quadrant.Top | Quadrant.Left,
					   true => Quadrant.Top,
					   false when leftBound => Quadrant.Left,
					   false => 0
				   };
		}


		public static void DrawArrow( this Graphics3D g, in Point source, in Point target ) => g.DrawArrow(source, target, Color.LightSkyBlue);
		public static void DrawArrow( this Graphics3D g, in Point source, in Point target, in Color color )
		{
			g.Brush.Color = color;

			if ( Device.RuntimePlatform == Device.Android )
			{
				g.Pen.Width = 2;
				g.Arrow(true, source, target, 15, 15, 0);
			}
			else
			{
				g.Pen.Width = 1;
				g.Arrow(true, source, target, 10, 10, 0);
			}
		}


		// public static void DrawRectangle( this Graphics3D g, in Rectangle bounds, in Color color )
		// {
		// 	g.Brush.Color = color;
		// 	g.Rectangle(bounds);
		// }
		// public static void DrawRectangle( this Graphics3D g, in AnnotationTool tool, in Color color ) => g.DrawRectangle(tool.Bounds, color);
		// public static void DrawRectangle( this Graphics3D g, in AnnotationTool tool ) => g.DrawRectangle(tool, Color.SlateBlue);
		//
		//
		// public static void DrawAnnotation( this Graphics3D g, in Rectangle bounds, in ChartFont font, in string text )
		// {
		// 	g.SetTextAlign(TextAlignment.Start);
		// 	g.Brush.Color = font.Color;
		// 	g.Font = font;
		//
		// 	double height = bounds.Height - g.TextHeight(text);   // find leftover space
		// 	double y = ( bounds.Height - height ) / 2 + bounds.Y; // get middle, add start
		//
		// 	double width = bounds.Width - g.TextWidth(text);     // find leftover space
		// 	double x = ( bounds.Height - width ) / 2 + bounds.X; // get middle, add start
		//
		// 	g.DoDrawString(x, y, text, g.Brush, Guid.NewGuid());
		// 	g.TextOut(font, x, y, text, Guid.NewGuid(), true);
		// }
		// public static void DrawAnnotation( this Graphics3D g, in AnnotationTool tool, in string text )
		// {
		// 	g.DrawRectangle(tool);
		// 	g.DrawAnnotation(tool.Bounds, tool.Shape.Font, text);
		// }


		public static double VerticalOffsetInRange( this Point point, in double value, in double min = MIN_ANNOTATION_OFFSET, in double max = ANNOTATION_OFFSET ) => Math.Min(point.VerticalOffsetAtLeast(value, min), Math.Abs(max));
		public static double VerticalOffsetAtLeast( this Point point, in double value, in double min ) => Math.Max(point.VerticalOffset(value), Math.Abs(min));
		public static double VerticalOffset( this Point point, in double value ) => point.Y.Offset(value);


		public static double HorizontalOffsetInRange( this Point point, in double value, in double min = MIN_ANNOTATION_OFFSET, in double max = ANNOTATION_OFFSET ) => Math.Min(point.HorizontalOffsetAtLeast(value, min), Math.Abs(max));
		public static double HorizontalOffsetAtLeast( this Point point, in double value, in double min ) => Math.Max(point.HorizontalOffset(value), Math.Abs(min));
		public static double HorizontalOffset( this Point point, in double value ) => point.X.Offset(value);


		public static double Horizontal( this Point point, in double value ) => value.Adjust(point.X, point.HorizontalOffsetInRange(value));
		public static double Vertical( this Point point, in double value ) => value.Adjust(point.Y, point.VerticalOffsetInRange(value));
		public static double Vertical( this Point point, in double value, in Quadrant quadrant ) => value.Adjust(quadrant, point.VerticalOffsetInRange(value));


		public static double Adjust( this double value, in Quadrant quadrant, in double offset ) => value.Adjust(quadrant.HasFlag(Quadrant.Top), offset);
		public static double Adjust( this double value, in double other, in double offset ) => value.Adjust(value > other, offset);
		public static double Adjust( this double value, in bool check, in double offset ) =>
			check
				? value - offset
				: value + offset;


		public static double Offset( this double value, in double other ) => value.Offset(other, ANNOTATION_OFFSET);
		public static double Offset( this double value, in double other, in double constant ) => Math.Abs(Math.Abs(value - other) / value * constant);
	}



	[Flags]
	public enum Quadrant
	{
		Top = 0x01,
		Left = 0x10,
	}
}