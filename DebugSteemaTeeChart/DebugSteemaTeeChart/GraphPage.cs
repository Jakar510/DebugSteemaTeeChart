using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Steema.TeeChart;
using Steema.TeeChart.Drawing;
using Steema.TeeChart.Styles;
using Steema.TeeChart.Tools;
using Xamarin.Essentials;
using Xamarin.Forms;

#nullable enable
namespace DebugSteemaTeeChart
{
	public class GraphPage : ContentPage
	{
		private NearestPoint? _nearestPointTool;

		internal NearestPoint NearestPointTool
		{
			get => _nearestPointTool ?? throw new NullReferenceException(nameof(_nearestPointTool));
			set
			{
				if ( _nearestPointTool != null ) { _nearestPointTool.Change -= NearestPointToolOnChange; }

				_nearestPointTool = value;

				if ( _nearestPointTool is null ) return;
				_nearestPointTool.Change += NearestPointToolOnChange;
			}
		}

		private Chart? _chart1;

		internal Chart Chart1
		{
			get => _chart1 ?? throw new NullReferenceException(nameof(_chart1));
			set
			{
				if ( _chart1 != null )
				{
					_chart1.AfterDraw -= TChart1_Annotations_AfterDraw;
					_chart1.BeforeDraw -= Chart1_BeforeDrawSeries;
				}

				_chart1 = value;

				if ( _chart1 is null ) return;
				_chart1.AfterDraw += TChart1_Annotations_AfterDraw;
				_chart1.BeforeDraw += Chart1_BeforeDrawSeries;
			}
		}

		public bool FloatingAnnotations { get; set; }
		public string SelectedItemDescription { get; set; } = string.Empty;
		public int CurrentPointIndex { get; set; }
		public double AnnotationHeightOffset { get; set; } = 30;

		internal Point TargetPoint { get; set; }
		internal Point SourcePoint { get; set; }


		public static bool IsPhone { get; } = DeviceInfo.Idiom == DeviceIdiom.Phone;
		internal Line DataLine { get; }
		internal ChartView ChartViewer { get; }
		internal AnnotationTool AnnotationTool { get; set; }


		public GraphPage() : this(true) { }
		public GraphPage( bool floatingAnnotations )
		{
			FloatingAnnotations = floatingAnnotations;
			Content = ChartViewer = new ChartView()
									{
										VerticalOptions = LayoutOptions.FillAndExpand,
										HorizontalOptions = LayoutOptions.FillAndExpand,
										Visual = new VisualMarker.DefaultVisual()
									};

			Chart1 = ChartViewer.Chart;
			Chart1.AutoRepaint = true;
			Chart1.PanningZoom(true);
			Chart1.ConfigureAxes();
			Chart1.Title("Chart Title");
			Chart1.Legend();
			Chart1.Walls();
			DataLine = Chart1.CreateLine();
			DataLine.FillSampleValues(25);


			NearestPointTool = new NearestPoint(Chart1)
							   {
								   Active = true,
								   DrawLine = true,
								   FullRepaint = true,
								   Style = NearestPointStyles.Circle,
								   Size = 5,
								   Series = DataLine,
								   Direction = NearestPointDirection.Both,
							   };

			AnnotationTool = new AnnotationTool(Chart1, SelectedItemDescription);

			AnnotationTool.ConfigureAnnotations(FloatingAnnotations);
		}


		private void NearestPointToolOnChange( object sender, NearestPointEventArgs e )
		{
			try
			{
				CurrentPointIndex = e.Point;

				string result = Math.Round(DataLine.YValues[CurrentPointIndex], 3).ToString("n", CultureInfo.CurrentCulture);


				SelectedItemDescription = $"<b>  {DateTime.Now}: {result}</b>{Utils.NewLine}  Value: {result}";
			}
			catch ( IndexOutOfRangeException )
			{
				CurrentPointIndex = -1;
				SelectedItemDescription = string.Empty;
			}
			catch ( Exception ex )
			{
				Console.WriteLine(ex);

				CurrentPointIndex = -1;
				SelectedItemDescription = string.Empty;
			}
			finally { AnnotationTool.Active = false; }
		}


		private void Chart1_BeforeDrawSeries( object sender, Graphics3D g )
		{
			try
			{
				if ( CurrentPointIndex < 0 ||
					 DataLine.Count <= 0 )
				{
					AnnotationTool.Active = false;
					return;
				}

				SetPoints();
			}
			catch ( Exception e ) { Console.WriteLine(e); }
		}

		private void SetPoints() => SetPoints(DataLine.CalcXPos(CurrentPointIndex), DataLine.CalcYPos(CurrentPointIndex));
		private void SetPoints( in double x, in double y )
		{
			AnnotationTool.ConfigureAnnotations(FloatingAnnotations, SelectedItemDescription);
			if ( !AnnotationTool.Active ) { return; }

			Chart1.Title.Visible = FloatingAnnotations;

			if ( FloatingAnnotations )
			{
				// Floating Annotations
				Quadrant quadrant = 0;
				if ( x <= Chart1.ChartBounds.AverageWidth() ) { quadrant |= Quadrant.Left; }

				if ( y <= DataLine.LocalAveragePos(out double minVerticalValueHeight, out double maxVerticalValueHeight, true) ) { quadrant |= Quadrant.Top; }

				SourcePoint = AnnotationTool.GetPoint(Chart1,
													  AnnotationHeightOffset,
													  x,
													  y,
													  minVerticalValueHeight,
													  maxVerticalValueHeight,
													  IsPhone,
													  quadrant
													 );

				double targetY = SourcePoint.Vertical(y, quadrant);
				double targetX = SourcePoint.Horizontal(x);
				TargetPoint = new Point(targetX, targetY);
			}
			else
			{
				// Static Annotations
				SourcePoint = new Point(AnnotationTool.Left, AnnotationTool.GetBounds().Bottom);

				double targetX = SourcePoint.Horizontal(x);
				double targetY = y - SourcePoint.VerticalOffsetInRange(y);

				TargetPoint = new Point(targetX, targetY);
			}
		}


		private void TChart1_Annotations_AfterDraw( object sender, Graphics3D g )
		{
			if ( CurrentPointIndex < 0 ||
				 !AnnotationTool.Active ) return;

			// comment out to make annotation not render
			// uncomment out to make annotation render
			// AnnotationTool.Draw(g);
			
			
			g.DrawArrow(SourcePoint, TargetPoint);
		}
	}
}