using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		List<string> _path = new List<string>();
		TextBox _tbLast = new TextBox();
		const string _defaultEndText = "";


		public BreadCrumb()
		{
			InitializeComponent();

			_tbLast.Text = _defaultEndText;
			_tbLast.VerticalAlignment = VerticalAlignment.Top;
			_tbLast.BorderThickness = new Thickness(0);

			mainPanel.Children.Add(_tbLast);
		}

		public delegate void ChildSelectedDelegate(object sender, BCSelectedEventArgs e);
		public event ChildSelectedDelegate ChildSelected;

		public void AddDataSet(IEnumerable<string> newDataSet)
		{
			if ((null != newDataSet) && (newDataSet.Count() > 0))
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
				{
					bci.Text = _path.Last();

					//Debug.Assert(newDataSet.Contains(_path.Last()));
					//
					// TODO: I shoudl enforce that the new data set that arrived has in it's list the 
					// last path object... - no need - text is from previous, list is unrelated and for next

					//
					// also maybe the path should stop holding objects...
					// I am not ready to differentiate between display string and an internal value..
				}

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
		public void DropDataSetsFromIndex(int nIndex)
		{
			int nDataCount = _dataSets.Count;
      if (nIndex < nDataCount)
			{
				IEnumerable<Control> ctrlsRemove = _ctrls.Skip(nIndex);
				foreach (Control ctrl in ctrlsRemove)
					mainPanel.Children.Remove(ctrl);

				_dataSets = _dataSets.Take(nIndex).ToList();
				_ctrls = _ctrls.Take(nIndex).ToList();
				_path = _path.Take(nIndex).ToList();
				if (nIndex > 0)
					_tbLast.Text = _path.Last();
				else
					_tbLast.Text = _defaultEndText;
			}
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

				_path[nId] = e.Value.ToString(); // TODO: perhaps this value shoudl be a string as well - for simplicity, for now - can be extended at any time...

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
