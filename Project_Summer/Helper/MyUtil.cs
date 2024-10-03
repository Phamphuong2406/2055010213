using System.Reflection.Metadata;
using System.Text;

namespace Project_Summer.Helper
{
    public class MyUtil
    {
        public static String UploadHinh(IFormFile Hinh, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "img", folder, Hinh.FileName);
                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Hinh.CopyTo(myfile);

                }
                return Hinh.FileName;
            }
            catch ( Exception ex)
            {
                return string.Empty;
            }
        }
        // thuật toán random chuỗi 
        public static string GenerateRandomKey(int lenght = 5)
        {
            var pattern = @"qazwsxedcrfvtgbyhnujmikolpQAZWSXEDCRFVTGBYHNUJMIKOLP!";
            var sb = new StringBuilder();
            var random = new Random();
            for (int i = 0; i < lenght; i++)
            {
                sb.Append(pattern[random.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}
