using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Threading.Tasks;
using APSWCWEBAPIAPP.DBConnection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace APSWCWEBAPIAPP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesUploadController : ControllerBase
    {
        [HttpPost, DisableRequestSizeLimit]
        [Route("UploadFileDetails")]
        public IActionResult Upload()
        {
            try
            {
                var supportedTypes = new[] { "jpg", "jpeg", "png", "pdf" };
                var file = Request.Form.Files[0];
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);

                if (!supportedTypes.Contains(fileExt))
                {
                    string ErrorMessage = "File Extension Is InValid - Only Upload jpg/png/pdf File";
                    return Ok(new { ErrorMessage });
                }
                var folderName = Path.Combine("Inspection", "Images");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, DateTime.Now.ToString("ddMMyyyy"), fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("GalleryUploadFileDetails")]
        public IActionResult GalleryUpload()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("WareHouse", "Documents");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        [HttpPost, DisableRequestSizeLimit]
        [Route("WHUploadFileDetails")]
        public IActionResult WHUploadFileDetails()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("WareHouse", "Documents");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        [HttpPost, DisableRequestSizeLimit]
        [Route("PQCUploadFile")]
        public IActionResult Upload_PQCDoc()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Periodic_QC", "Documents");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("RegUploadFileDetails")]
        public IActionResult RegUploadFileDetails()
        {
            try
            {
                var file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    var regID = Request.Form["regid"].ToString();
                    var category = Request.Form["Category"].ToString();

                    var folderName = Path.Combine("Registrations", regID);
                    var pathToSave = Path.Combine("wwwroot", folderName);
                    var fileExtension = Path.GetExtension(file.FileName);
                    //var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"')
                    var fileName = DateTime.Now.ToString("yyyyMMddhhmmssmmm") + "_"+ category + fileExtension;
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        #region DeadStock
        [HttpPost, DisableRequestSizeLimit]
        [Route("DeadStockUploadFileDetails")]
        public IActionResult DeadStockUploadFileDetails()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("DeadStock", "Documents");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpPost, DisableRequestSizeLimit]
        [Route("DeadStockUploadImageDetails")]
        public IActionResult DeadStockUploadImageDetails()
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("DeadStock", "Images");
                var pathToSave = Path.Combine("wwwroot", folderName);
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);
                    bool folderExists = Directory.Exists(pathToSave);
                    if (!folderExists)
                        Directory.CreateDirectory(pathToSave);
                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }
        #endregion

        #region EncryptFile
        [HttpPost, DisableRequestSizeLimit]
        [Route("EncryptFileUpload")]
        public ActionResult EncryptFileUpload()
        {
            try
            {
                var supportedTypes = new[] { "jpg", "jpeg", "png", "pdf" };

                var file = Request.Form.Files[0];

                string folderName = Request.Form["pagename"];

                string todayDate = DateTime.Now.ToString("dd/MM/yyyy");

                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1).ToLower();

                if (!supportedTypes.Contains(fileExt))
                {
                    string ErrorMessage = "File Extension Is InValid - Only Upload jpg/png/pdf File";

                    return Ok(new { ErrorMessage });
                }
                folderName = Path.Combine(folderName, todayDate);

                var pathToSave = Path.Combine("wwwroot", folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

                    var fileWExt = System.IO.Path.GetFileNameWithoutExtension(fileName);

                    var createdtime = DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss");

                    fileName = fileWExt + "_" + createdtime + "." + fileExt;

                    string fullPath = Path.Combine(pathToSave, fileName);

                    bool folderExists = Directory.Exists(pathToSave);

                    if (!folderExists)
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    string outfileName = fileWExt + "_" + createdtime + "." + "txt";

                    string outPath = Path.Combine(pathToSave, outfileName);

                    bool isEncrypted = EncryptFile(fullPath, outPath);

                    if (isEncrypted)
                    {
                        if (System.IO.File.Exists(fullPath))
                        {
                            System.IO.File.Delete(fullPath);
                        }

                        System.IO.FileInfo fi = new System.IO.FileInfo(outPath);

                        if (fi.Exists)
                        {
                            fi.MoveTo(fullPath);
                        }
                    }
                    return Ok(new { fullPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }


        private bool EncryptFile(string inputFilePath, string outputfilePath)
        {
            bool isEncrypted = false;
            string EncryptionKey = "MAKV2SPBNI99512";
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (FileStream fsOutput = new FileStream(outputfilePath, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fsOutput, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                        {
                            int data;
                            while ((data = fsInput.ReadByte()) != -1)
                            {
                                cs.WriteByte((byte)data);
                            }
                            isEncrypted = true;
                        }
                    }
                }
            }
            return isEncrypted;
        }


        [HttpPost, DisableRequestSizeLimit]
        [Route("DecryptFile")]
        public ActionResult<string> DecryptFile(FilePath filePath)
        {
            string inputFilePath = filePath.DBPath;
            byte[] byteArray = null;
            string EncryptionKey = "MAKV2SPBNI99512";
            string docBase64 = "";
            try
            {
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
                    {
                        using (CryptoStream cs = new CryptoStream(fsInput, encryptor.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                cs.CopyTo(memoryStream);
                                byteArray = memoryStream.ToArray();
                            }
                        }
                    }
                    string ext = System.IO.Path.GetExtension(inputFilePath).Substring(1).ToLower();

                    if (ext == "jpg" || ext == "jpeg")
                    {
                        docBase64 = "data:image/jpeg;base64," + Convert.ToBase64String(byteArray);
                        return Ok(new { docBase64 });
                    }
                    if (ext == "png")
                    {
                        docBase64 = "data:image/png;base64," + Convert.ToBase64String(byteArray);
                        return Ok(new { docBase64 });
                    }
                    else
                    {
                        docBase64 = "data:application/pdf;base64," + Convert.ToBase64String(byteArray);
                        return Ok(new { docBase64 });
                    }
                    //return byteArray;
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        #endregion
    }

    public class FilePath
    {
        public string DBPath { get; set; }
    }
}