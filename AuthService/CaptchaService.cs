using ModelService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Dynamic;
using System.IO;
using System.Security.Cryptography.Xml;
using System.Text;
namespace AuthService
{
    public class CaptchaService : ICaptchaService
    {
        private ApplicationAPSWCCDbContext _context;
        public CaptchaService(ApplicationAPSWCCDbContext appcontext)
        {
            { _context = appcontext; }
        }
        public dynamic check_s_captch(string value)
        {
            Captch cap = new Captch();
            
            dynamic data = new ExpandoObject();

            var ids = "";
            ids = DateTime.Now.Ticks.ToString();
            Random r = new Random();
            int random_num = r.Next(0, 99999);
            var serila = random_num;

            Bitmap objBitmap = new Bitmap(180, 100);
            Graphics objGraphics = Graphics.FromImage(objBitmap);
            objGraphics.Clear(Color.White);
            Random objRandom = new Random();
            objGraphics.DrawLine(Pens.White, objRandom.Next(0, 50), objRandom.Next(10, 30), objRandom.Next(0, 200), objRandom.Next(0, 50));
            objGraphics.DrawRectangle(Pens.White, objRandom.Next(0, 20), objRandom.Next(0, 20), objRandom.Next(50, 80), objRandom.Next(0, 20));
            objGraphics.DrawLine(Pens.White, objRandom.Next(0, 20), objRandom.Next(10, 50), objRandom.Next(100, 200), objRandom.Next(0, 80));
            Brush objBrush =
                default(Brush);
            HatchStyle[] aHatchStyles = new HatchStyle[]
            {
               HatchStyle.LargeGrid, HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal
            };
            RectangleF oRectangleF = new RectangleF(0, 0, 300, 300);
            objBrush = new HatchBrush(aHatchStyles[objRandom.Next(aHatchStyles.Length - 3)], Color.FromArgb((objRandom.Next(100, 255)), (objRandom.Next(100, 255)), (objRandom.Next(100, 255))), Color.White);
            objGraphics.FillRectangle(objBrush, oRectangleF);
            string captchaText = objRandom.Next(100000, 999999).ToString();
            Font objFont = new Font("Courier New", 28, FontStyle.Bold);
            objGraphics.DrawString(captchaText, objFont, Brushes.Black, 15, 30);
            objGraphics.Flush();
            objGraphics.Dispose();
            string fileName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".Gif";
            objBitmap.Save(Path.Combine("capth", ids + serila.ToString() + fileName), System.Drawing.Imaging.ImageFormat.Gif);


            data.Capchid = captchaText.ToString();
            data.id = ids.ToString().Trim();

            bool captchgen = true;

            if (captchgen == true)
            {
                cap.Capchid = data.Capchid;
                cap.Id = data.id;
                cap.IsActive = 1;
                _context.captcha.Add(cap);
                _context.SaveChanges();
                byte[] imageBytes = System.IO.File.ReadAllBytes(Path.Combine("capth", ids + serila + fileName));
                string base64String = Convert.ToBase64String(imageBytes);
                data.idval = ids;

                data.code = "100";
                data.imgurl = base64String;
                DirectoryInfo diInfo = new DirectoryInfo(Path.Combine("capth"));
                FileInfo[] files = diInfo.GetFiles();
                for (int i = 0; i < files.Length; i++)
                {
                    string filePath = Path.Combine("capth/" + files[i].ToString());
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }
                return data;

            }
            else
            {
                data.code = "101";
                data.message = "";
                return data;
            }
        }

       

    }

    public interface ICaptchaService
    {
        dynamic check_s_captch(string value);
    }
}
