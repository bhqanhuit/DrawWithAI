namespace DrawWithAI.DrawApi.Services
{
    public class ImageDriveService
    {
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
            return localImagePath;

            // input imagePath --> upload image to drive --> output namePath
            // create name pattern for image (to avoid overwriting)
        }
    }
}

