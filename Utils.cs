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
    }
}
