using BCControl;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace TestCtrls
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();

			BCITest.Text = "test_main";
			BCITest.Items = new string[] { "bau", "hau", "miau" };
			BCITest.ChildSelected += BCITest_ChildSelected;
			
			//
			// simplest crumb case - add data ready and independent
			/*
			BCSimpleTest.AddDataSet(new string[] { "aa", "bb", "cc", "dd" });
			BCSimpleTest.AddDataSet(new string[] { "11", "22", "33", "44", "55" });
			BCSimpleTest.AddDataSet(new string[] { "miau", "hau", "bau" });
			BCSimpleTest.ChildSelected += BCTest_ChildSelected;*/

			//
			// test crumb with dependent data:
			// maybe test it realy on folders navigation? would be good test

			//
			// start with a known folder like c:, populate only with that,
			// and as the user chnages stuff react

			IEnumerable<string> foldersHere = Directory.GetDirectories("d:\\", "*", SearchOption.TopDirectoryOnly).Select(u => GetLastFolderNameFromPath(u));
			BCDependencyTest.AddDataSet(foldersHere);
			BCDependencyTest.ChildSelected += BCDependencyTest_ChildSelected;

		}

		private void BCDependencyTest_ChildSelected(object sender, BCControl.BCSelectedEventArgs e)
		{
			//
			// in here I want to simulate a HDD folder browser, so whenever a new child is selected
			//
			// 1. I will drop all data sets after the current modified item
			//   (I rely on the fact that the control is optimized to do nothing in case the very last item was changed)
			// 2. list the subfolders of the new path
			// 3. update the control with a new data set about the current path

			
			//
			// 1. 
			BreadCrumb bcc = sender as BreadCrumb;
			if (null != bcc)
			{
				bcc.DropDataSetsFromIndex(e.SelectionStep + 1);

				string strPath = "d:\\";
				foreach (string str in e.StablePath)
					strPath += str + "\\";

				//
				// 2.
				IEnumerable<string> foldersHere = Directory.GetDirectories(strPath).Select(u => GetLastFolderNameFromPath(u));

				//
				// 3.
				bcc.AddDataSet(foldersHere); 
			}
		}

		string GetLastFolderNameFromPath(string strFullPath)
		{
			int nIdx = strFullPath.LastIndexOf("\\");
			if (nIdx > 0)
				return strFullPath.Substring(nIdx+1, strFullPath.Length - nIdx - 1);
			else
				return strFullPath;
    }

		private void BCTest_ChildSelected(object sender, BCControl.BCSelectedEventArgs e)
		{
			
		}

		private void BCITest_ChildSelected(object sender, BCControl.BCIChildSelectedEventArgs e)
		{
			MessageBox.Show("Index: " + e.Index + " Value: " + e.Value.ToString());
		}

		private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			//BCSimpleTest.Collapse();
			BCDependencyTest.Collapse();
		}
	}
}
