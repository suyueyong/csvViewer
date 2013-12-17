using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Threading;
using System.IO;


namespace csvViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                textBox1.Text = dlg.FileName;
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                textBox2.Text = dlg.FileName;
            }

        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                textBox3.Text = dlg.FileName;
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            string filename1 = textBox1.Text;
            string filename2 = textBox2.Text;
            string filename3 = textBox3.Text;
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Name");
            dt.Columns.Add("Age");
            dt.Columns.Add("Level");
            dt.Columns.Add("CX");
            dt.Columns.Add("CY");
            toDataTable(dt, filename1, filename2, filename3);
            DataView dv = dt.DefaultView;
            dv.Sort = "Level ASC, CX ASC, CY ASC";
            DataTable sortedDT = dv.ToTable();
            listView1.ItemsSource = sortedDT.DefaultView;
        }

        private void toDataTable(DataTable dt, string filename1, string filename2, string filename3)
        {
            try
            {
                string[] csvRows1 = { };
                string[] csvRows2 = { };
                string[] csvRows3 = { };
                Thread t1 = new Thread(() => readFile(filename1, ref csvRows1));
                Thread t2 = new Thread(() => readFile(filename2, ref csvRows2));
                Thread t3 = new Thread(() => readFile(filename3, ref csvRows3));
                t1.Start();
                t2.Start();
                t3.Start();
                t1.Join();
                t2.Join();
                t3.Join();
                var csvRows = csvRows1.Union(csvRows2).Union(csvRows3).ToArray<string>();
                string[] field = null;
                Array.Sort(csvRows, 1, csvRows.Length - 1);
                int r = 0;
                for (int i = 1; i < csvRows.Length; i++)
                {
                    if (csvRows[i] != csvRows[r])
                    {
                        csvRows[++r] = csvRows[i];
                    }
                }
                for (int i = 1; i < r + 1; i++)
                {
                    string temp = i.ToString() + ",";
                    field = (temp + csvRows[i]).Split(',');
                    DataRow row = dt.NewRow();
                    row.ItemArray = field;
                    dt.Rows.Add(row);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void readFile(string filename, ref string[] csvRows) {
            csvRows = System.IO.File.ReadAllLines(filename);
        }       
    }
}
