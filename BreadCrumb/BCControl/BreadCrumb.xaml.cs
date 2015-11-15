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
	/// Interaction logic for BreadCrumb.xaml
	/// </summary>
	public partial class BreadCrumb : UserControl
	{
		List<IEnumerable<string>> _dataSets = new List<IEnumerable<string>>();
		List<BreadCrumbItem> _ctrls = new List<BreadCrumbItem>();
		List<object> _path = new List<object>();
		TextBox _tbLast = new TextBox();

		public BreadCrumb()
		{
			InitializeComponent();

			_tbLast.Text = "";
			_tbLast.VerticalAlignment = VerticalAlignment.Top;
			_tbLast.BorderThickness = new Thickness(0);

			mainPanel.Children.Add(_tbLast);
		}

		public delegate void ChildSelectedDelegate(object sender, BCSelectedEventArgs e);
		public event ChildSelectedDelegate ChildSelected;

		public void AddDataSet(IEnumerable<string> newDataSet)
		{
			if (null != newDataSet)
			{
				int nId = _dataSets.Count;

				_dataSets.Add(newDataSet);

				//
				// also add a new BCI control

				BreadCrumbItem bci = new BreadCrumbItem();
				bci.Tag = nId;
				bci.Items = newDataSet;
				bci.ChildSelected += Bci_ChildSelected;
				bci.ItemExpanded += Bci_ItemExpanded;
				if (nId == 0)
					bci.Text = "";
				else
					bci.Text = _dataSets[nId-1].First();

				mainPanel.Children.Insert(nId, bci);
				_tbLast.Text = newDataSet.First();
				_path.Add(newDataSet.First());
				_ctrls.Add(bci);
			}
    }
		public void Collapse()
		{
			Bci_ItemExpanded(null);
    }
		public void DropItemsFromIndex(int nIndex)
		{

		}

		private void Bci_ItemExpanded(object sender)
		{
			//
			// collapse all the rest
			foreach (BreadCrumbItem item in _ctrls)
			{
				if (item != sender)
					item.Expanded = false;
			}
    }

		private void Bci_ChildSelected(object sender, BCIChildSelectedEventArgs e)
		{
			BreadCrumbItem bci = sender as BreadCrumbItem;
			if(null != bci)
			{
				int nId = (int)bci.Tag;

				//
				// time to set the next bci to the value of the previous selection
				if (nId + 1 < _ctrls.Count)
				{
					_ctrls[nId + 1].Text = e.Value.ToString();
        }
				else
				{
					_tbLast.Text = e.Value.ToString();
				}

				_path[nId] = e.Value;

				if(null != ChildSelected)
				{
					ChildSelected(this, new BCSelectedEventArgs() { SelectionStep = nId, SelectedIndex = e.Index, SelectedValue = e.Value, StablePath = _path.Take(nId+1), FullPath = _path });
				}
			}
    }
	}

	public class BCSelectedEventArgs : EventArgs
	{
		public int SelectionStep { get; set; }
		public int SelectedIndex { get; set; }
		public object SelectedValue { get; set; }
		public IEnumerable<object> StablePath { get; set; }
		public IEnumerable<object> FullPath { get; set; }
	}
}
