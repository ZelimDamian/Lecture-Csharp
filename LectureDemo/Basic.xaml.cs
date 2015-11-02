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
using System.Windows.Shapes;

namespace LectureDemo
{
    /// <summary>
    /// Interaction logic for Basic.xaml
    /// </summary>
    public partial class Basic : Window
    {
        protected Basic()
        {
            InitializeComponent();
        }

        public static Basic Show()
        {
            if (instance == null)
                instance = new Basic();
            (instance as Window).Show();
            (instance as Window).Focus();
            return instance;
        }

        static Basic instance;
    }
}
