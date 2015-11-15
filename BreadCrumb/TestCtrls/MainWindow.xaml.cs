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
			BCSimpleTest.AddDataSet(new string[] { "aa", "bb", "cc", "dd" });
			BCSimpleTest.AddDataSet(new string[] { "11", "22", "33", "44", "55" });
			BCSimpleTest.AddDataSet(new string[] { "miau", "hau", "bau" });
			BCSimpleTest.ChildSelected += BCTest_ChildSelected;

			//
			// test crumb with dependent data:
			// maybe test it realy on folders navigation? would be good test

			//
			// start with a known folder like c:, populate only with that,
			// and as the user chnages stuff react

			string[] foldersHere = Directory.GetDirectories("c:");
			BCDependencyTest.AddDataSet(foldersHere);
			BCDependencyTest.ChildSelected += BCDependencyTest_ChildSelected;

		}

		private void BCDependencyTest_ChildSelected(object sender, BCControl.BCSelectedEventArgs e)
		{
			IEnumerable<object> stablePath = e.StablePath;
			string strStablePath = "c:\\";
			foreach (object obj in stablePath)
				strStablePath += obj.ToString() + "\\";

			string[] foldersHere = Directory.GetDirectories(strStablePath);

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
			BCSimpleTest.Collapse();
		}
	}
}
