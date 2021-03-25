using DebugSteemaTeeChart.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace DebugSteemaTeeChart.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}