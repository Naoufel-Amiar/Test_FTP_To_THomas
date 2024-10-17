using System;
using System.IO;
using System.Net;
using System.Windows;

namespace Test_FTP
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DownloadFileButton_Click(object sender, RoutedEventArgs e)
        {
            string ftpServer = ftpServerTextBox.Text.Trim();
            string username = usernameTextBox.Text.Trim();
            string password = passwordBox.Password.Trim();
            string remoteFilePath = remoteFilePathTextBox.Text.Trim();
            string localFilePath = localFilePathTextBox.Text.Trim();

            try
            {
                // Création de l'instance de FtpClient
                FtpClient ftpClient = new FtpClient(ftpServer, username, password);
                ftpClient.DownloadFile(remoteFilePath, localFilePath);
                MessageBox.Show("Fichier téléchargé avec succès !", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors du téléchargement : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
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
                // Créez l'URL FTP
                string ftpUrl = $"ftp://{_ftpServer}/{remoteFilePath}";

                // Créer la requête FTP
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(_ftpUsername, _ftpPassword);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create))
                {
                    responseStream.CopyTo(fileStream);
                }

                Console.WriteLine("Téléchargement terminé.");
            }
            catch (WebException ex)
            {
                // Récupération de l'erreur spécifique
                if (ex.Response is FtpWebResponse response)
                {
                    throw new Exception($"Erreur lors du téléchargement : {response.StatusDescription}");
                }
                else
                {
                    throw new Exception($"Erreur générale : {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du téléchargement : {ex.Message}");
                throw; // Re-throw the exception to be handled by the calling method
            }
        }
    }
}
