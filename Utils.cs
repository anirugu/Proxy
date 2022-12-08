namespace Proxy
{
    public static class Utils
    {
        public static string SanitizeName(this string fileName)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
            {
                fileName = fileName.Replace(c, '_');
            }
            return fileName;
        }

        public static string GetFilePath(string url)
        {
            var dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), GetDirectory());
            if (!System.IO.Directory.Exists(dir))
                System.IO.Directory.CreateDirectory(dir);
            return System.IO.Path.Combine(dir, url.SanitizeName());
        }
        
        /// <summary>
        /// Update every 5 day
        /// </summary>
        /// <returns></returns>
        private static string GetDirectory()
        {
            var date = DateTime.Now;
            int month = date.Month;
            int h = date.Day % 5;
            return $"{month}{h}";
        }
    }
}
