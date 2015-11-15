using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BCControl
{
	/// <summary>
	/// Interaction logic for BreadCrumbItem.xaml
	/// </summary>
	public partial class BreadCrumbItem : UserControl
	{
		public BreadCrumbItem()
		{
			InitializeComponent();
		}

		public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable<string>), typeof(BreadCrumbItem), new PropertyMetadata(new string[0]));
		public IEnumerable<string> Items
		{
			get { return (IEnumerable<string>)this.GetValue(ItemsProperty); }
			set { this.SetValue(ItemsProperty, value); }
		}

		public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BreadCrumbItem), new PropertyMetadata("Unnamed"));
		public string Text
		{
			get { return (string)this.GetValue(TextProperty); }
			set
			{
				this.SetValue(TextProperty, value);
				tbText.Text = value;
			}
		}

		public delegate void ChildSelectedDelegate(object sender, BCIChildSelectedEventArgs e);
		public event ChildSelectedDelegate ChildSelected;

		public delegate void ItemExpandedDelegate(object sender);
		public event ItemExpandedDelegate ItemExpanded;

		public bool Expanded
		{
      get { return (Trsf.Angle > 1); }
			set
			{
				if (value != Expanded)
				{
					//
					// someone wants me to change this object expansion state
					Btn_Click(this, null);
				}
			}
		}

		private void Btn_Click(object sender, RoutedEventArgs e)
		{
			if (Trsf.Angle > 1)
			{
				//
				// collapse the item - reset arrow, hide the list
				Trsf.Angle = 0;
				lbItems.Visibility = Visibility.Collapsed;
			}
			else
			{
				//
				// expand the item, rotate arrow, show the list
				Trsf.Angle = 90;

				//_lbItems.Items.Clear();
				lbItems.ItemsSource = Items;
				lbItems.Visibility = Visibility.Visible;
				lbItems.Focus();

				if (null != ItemExpanded)
					ItemExpanded(this);
      }
		}

		private void lbItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (null != ChildSelected)
			{
				int nSelIdx = ((ListBox)sender).SelectedIndex;
				object selValue = ((ListBox)sender).SelectedValue; // TODO: test a doubel value here or something cooler

				ChildSelected(this, new BCIChildSelectedEventArgs() {Index=nSelIdx, Value=selValue });
			}

			Btn_Click(sender, null);
    }
	}

	public class BCIChildSelectedEventArgs : EventArgs
	{
		public int Index { get; set; }
		public object Value { get; set; }
	}
}
