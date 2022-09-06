using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace DataGenerator.App
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Data data = new Data();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = data;
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            data.export();
        }
    }

    class Data :Data_Generator.Datalayer.ViewModelBase
    {

        private string _strSQliteDB;

        public string strSQliteDB
        {
            get { return _strSQliteDB; }
            set
            {
                _strSQliteDB = value;

                Data_Generator.Datalayer.RepositoryMaker.setdatabaseSQLite(value);
                var lstTables =Data_Generator.Datalayer.RepositoryMaker.setTables();

                allTables = lstTables.Select(p => new Table() { isChecked = isSelectAll, title = p }).ToList();
            }
        }

        private bool _isSelectAll;

        public bool isSelectAll
        {
            get { return _isSelectAll; }
            set
            {
                _isSelectAll = value;
                foreach (var item in tables)
                    item.isChecked = value;
            }
        }

        private string _strSearch = "";

        public string strSearch
        {
            get { return _strSearch; }
            set
            {
                _strSearch = value;
                setTables();
            }
        }

        List<Table> allTables = new List<Table>();

        public List<string> DATABASES { get; set; }

        public Data()
        {
            var lst =Data_Generator.Datalayer.RepositoryMaker.getDATABASES();
            DATABASES = lst.OrderBy(p => p).ToList();
            tables = new ObservableCollection<Table>();
        }
        private string _dbName;

        public string dbName
        {
            get { return _dbName; }
            set
            {
                _dbName = value;
                setdb(value);
            }
        }

        public ObservableCollection<Table> tables { get; set; }

        public void setdb(string dbname)
        {
            allTables =Data_Generator.Datalayer.RepositoryMaker.getTables(dbname).Select(p => new Table() { isChecked = isSelectAll, title = p }).ToList();
            setTables();
        }

        private void setTables()
        {
            tables.Clear();
            var lst = allTables.ToList();
            if (lst != null)
            {
                var sp = _strSearch.Split(' ');
                foreach (var w in sp)
                {
                    lst = lst.Where(p => p.title.Contains(w)).ToList();
                }

                foreach (var item in lst)
                    tables.Add(item);
            }
            OnPropertyChanged(nameof(tables));
        }

        public List<string> lstSelected { get { return tables.Where(p => p.isChecked).Select(p => p.title).ToList(); } }

        public void export()
        {
            try
            {
               Data_Generator.Datalayer.RepositoryMaker.exportData(lstSelected);
                System.Windows.MessageBox.Show("دیتا با موفقیت درست شد!");
            }
            catch { }
        }

        public class Table :Data_Generator.Datalayer.ViewModelBase
        {
            public string title { get; set; }
            private bool _isChecked;

            public bool isChecked
            {
                get { return _isChecked; }
                set
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(isChecked));
                }
            }

            //
            //OnPropertyChanged(nameof(tables));

        }

    }

}
