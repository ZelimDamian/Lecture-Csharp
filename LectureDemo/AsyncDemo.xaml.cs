using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
    public partial class AsyncDemo : Window
    {
        protected AsyncDemo()
        {
            InitializeComponent();
        }

        public static AsyncDemo Show()
        {
            var window = instance as Window;
            if (window == null)
                window = instance = new AsyncDemo();

            window.Show();
            window.Focus();
            return instance;
        }

        static AsyncDemo instance;

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            instance = null;
        }


        async private void Button_Click(object sender, RoutedEventArgs e)
        {
            string url = urlTextBox.Text;

            if (url == null || url == "")
                return;

            Task<BitmapImage> imageTask = GetImageFromUrlAsync(url);

            // do whatever parallel work here:

            statusLabel.Content = "STARTED downloading image.";

            // wait for the task to finish now
            var image = await imageTask;

            statusLabel.Content = "FINISHED downloading image.";

            imageControl.Source = image;

            statusLabel.Content = "Loaded image into GUI.";
        }

        async private Task<BitmapImage> GetImageFromUrlAsync(string url)
        {
            var client = new HttpClient();

            byte[] imageBytes = await client.GetByteArrayAsync(url); // async method!  

            return ToImage(imageBytes);
        }


        private BitmapImage GetImageFromUrl(string url)
        {
            var client = new WebClient();

            byte[] imageBytes = client.DownloadData(url); // async method!  

            return ToImage(imageBytes);
        }


        private void syncButton_Click(object sender, RoutedEventArgs e)
        {
            string url = urlTextBox.Text;

            if (url == null || url == "")
                return;

            // do whatever parallel work here:

            statusLabel.Content = "STARTED downloading image.";

            // wait for the task to finish now
            var image = GetImageFromUrl(url);

            statusLabel.Content = "FINISHED downloading image.";

            imageControl.Source = image;

            statusLabel.Content = "Loaded image into GUI.";
        }

        public BitmapImage ToImage(byte[] array)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new System.IO.MemoryStream(array);
            image.EndInit();
            return image;
        }
    }
}
