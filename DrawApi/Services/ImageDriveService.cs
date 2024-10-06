using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;


namespace DrawWithAI.DrawApi.Services
{
    public class ImageDriveService
    {
        string CredentialsPath = @"Resources\GoogleAuth\client_secret_161921845702-5lv0eh2t5vl5t6d1jaftu2340ok1idai.apps.googleusercontent.com.json";
        static string FolderImageInputId = @"1U2_qm_kLVY-wXb70k0fe-0WzB1Ivzkgm";
        static string FolderImageOutputId = @"14--Tk9eNr2n4E43QYeZ7SKajR4aHOXag";
        string[] Scopes = { DriveService.Scope.DriveFile };
        string TokenPath = @"Resources\GoogleAuth\token.json";
        DriveService service;
        UserCredential credential;
        public ImageDriveService()
        {
            Console.WriteLine("Drive Controller creating...");
            // Load OAuth credentials
            using (var stream = new FileStream(CredentialsPath, FileMode.Open, FileAccess.Read))
            {
                string credPath = TokenPath;
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Drive API service.
            service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DrawWithAI",
            });

        }

        public static string GetFileId(DriveService service, string TargetName)
        {

            // Define the request
            FilesResource.ListRequest request = service.Files.List();
            request.PageSize = 1000; // Maximum files per request (can be changed)
            request.Fields = "nextPageToken, files(id, name)"; // Specify to only return file IDs and names
            request.Q = $"'{FolderImageOutputId}' in parents";

            // Execute request and iterate through all pages
            do
            {
                var result = request.Execute();
                if (result.Files != null && result.Files.Count > 0)
                {
                    foreach (var file in result.Files)
                    {
                        if (file.Name == TargetName) 
                        { 
                            return file.Id;
                        }
                        Console.WriteLine($"Found file: {file.Name} (ID: {file.Id})");
                    }
                }

                request.PageToken = result.NextPageToken; // Set next page token
            } while (!string.IsNullOrEmpty(request.PageToken));

            return "";
        }

        private static void SaveStreamToFile(string filePath, MemoryStream stream)
        {
            using (var file = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                stream.WriteTo(file);
            }
        }

        public string DownloadImage(string driveImageName, string destFolderPath)
        {
            string imagePath = Path.Combine(destFolderPath, driveImageName);
            
            string fileId = GetFileId(service, driveImageName);
            Console.WriteLine(fileId);

            var request = service.Files.Get(fileId);
            var stream = new MemoryStream();

            // Execute the download
            request.MediaDownloader.ProgressChanged += (Google.Apis.Download.IDownloadProgress progress) =>
            {
                switch (progress.Status)
                {
                    case Google.Apis.Download.DownloadStatus.Downloading:
                        Console.WriteLine(progress.BytesDownloaded + " bytes downloaded.");
                        break;

                    case Google.Apis.Download.DownloadStatus.Completed:
                        Console.WriteLine("Download completed.");
                        SaveStreamToFile(destFolderPath + @"\" + driveImageName, stream);
                        break;

                    case Google.Apis.Download.DownloadStatus.Failed:
                        Console.WriteLine("Download failed.");
                        break;
                }
            };

            request.Download(stream);

            Console.Write("Images Downloaded");

            return imagePath;
            // input driveImageName, destination folder path --> download image from drive --> output imagePath @"..\Images\<<name>>"
            // create name pattern for image (to avoid overwriting)

        }

        public string UploadImage(string localImagePath)
        {
            // Specify the image file to upload
            var filePath = localImagePath;
            var fileMetadata = new Google.Apis.Drive.v3.Data.File()
            {
                Name = Path.GetFileName(filePath),
                Parents = [FolderImageInputId]
            };

            FilesResource.CreateMediaUpload request;
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                request = service.Files.Create(
                    fileMetadata, stream, "image/jpeg");
                request.Fields = "id";
                request.Upload();
            }

            var file = request.ResponseBody;
            Console.WriteLine("File ID: " + file.Id);
            var Name = Path.GetFileName(localImagePath);

            return @"ImagesInput/" + Name;

            // input imagePath --> upload image to drive --> output namePath
            // create name pattern for image (to avoid overwriting)
        }
    }
}

