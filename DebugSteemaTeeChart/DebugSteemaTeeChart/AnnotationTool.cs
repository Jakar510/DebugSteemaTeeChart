// unset

using Steema.TeeChart;
using Steema.TeeChart.Drawing;
using Steema.TeeChart.Tools;
using Xamarin.Forms;

namespace DebugSteemaTeeChart
{
	public class AnnotationTool : Annotation
	{
		public AnnotationTool( Chart chart, string text ) : this(chart) => Text = text;
		public AnnotationTool( Chart chart ) : base(chart)
		{
			Active = true;
			Position = AnnotationPositions.Custom;
			TextAlign = TextAlignment.Start;
			AutoSize = true;
			AllowEdit = false;
		}


		public new Rectangle Bounds
		{
			get => base.Bounds;
			set
			{
				Shape.CustomPosition = true;
				base.Bounds = value;
			}
		}
		public Rectangle GetBounds()
		{
			// forces a refresh.
			AutoSize = true;
			return Bounds;
		}


		public void Draw( Graphics3D g ) { DrawText(g); }
	}
}