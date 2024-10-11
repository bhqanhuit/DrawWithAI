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
        string CredentialsPath = @"C:\DrawWithAI\DrawApi\Resources\GoogleAuth\client_secret_161921845702-5lv0eh2t5vl5t6d1jaftu2340ok1idai.apps.googleusercontent.com.json";
        string FolderImageInputId = @"1U2_qm_kLVY-wXb70k0fe-0WzB1Ivzkgm";
        string FolderImageOutputId = @"14--Tk9eNr2n4E43QYeZ7SKajR4aHOXag";
        string[] Scopes = { DriveService.Scope.DriveFile };
        string TokenPath = @"Resources\GoogleAuth\token.json";
        public ImageDriveService()
        {
            

        }  

        public string DownloadImage(string driveImageName, string destFolderPath)
        {
            string imagePath = Path.Combine(destFolderPath, driveImageName);
            Console.Write("Images Downloaded");

            return imagePath;
            // input driveImageName, destination folder path --> download image from drive --> output imagePath @"..\Images\<<name>>"
            // create name pattern for image (to avoid overwriting)

        }

        public string UploadImage(string localImagePath)
        {
            Console.WriteLine("Images Uploaded");
            // Load OAuth credentials
            UserCredential credential;
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
            var service = new DriveService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "DrawWithAI",
            });

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



            return localImagePath;

            // input imagePath --> upload image to drive --> output namePath
            // create name pattern for image (to avoid overwriting)
        }
    }
}

