using System;
using System.IO;
using System.Net;
using System.Windows;

namespace Test_FTP
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

        private void DownloadFileButton_Click(object sender, RoutedEventArgs e)
        {
            string ftpServer = ftpServerTextBox.Text;
            string username = usernameTextBox.Text;
            string password = passwordBox.Password;
            string remoteFilePath = remoteFilePathTextBox.Text;
            string localFilePath = localFilePathTextBox.Text;

            try
            {
                FtpClient ftpClient = new FtpClient(ftpServer, username, password);
                ftpClient.DownloadFile(remoteFilePath, localFilePath);
                MessageBox.Show("File downloaded successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class FtpClient
    {
        private string _ftpServer;
        private string _ftpUsername;
        private string _ftpPassword;

        public FtpClient(string ftpServer, string ftpUsername, string ftpPassword)
        {
            _ftpServer = ftpServer;
            _ftpUsername = ftpUsername;
            _ftpPassword = ftpPassword;
        }

        public void DownloadFile(string remoteFilePath, string localFilePath)
        {
            try
            {
                string ftpUrl = $"ftp://{_ftpServer}/{remoteFilePath}";

                // Create the FTP request
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create))
                {
                    responseStream.CopyTo(fileStream);
                }

                Console.WriteLine("Download complete.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error downloading file: {ex.Message}");
                throw; // Re-throw the exception to be handled by the calling method
            }
        }
    }
}
