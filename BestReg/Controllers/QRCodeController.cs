using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BestReg.Data;
using System;
using System.Threading.Tasks;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using IronSoftware;

namespace BestReg.Controllers
{
    public class QrCodeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public QrCodeController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        // This action displays the QR code for the logged-in student
        public async Task<IActionResult> ShowQrCode()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.QrCodeBase64))
            {
                return NotFound("User not found or QR code not available.");
            }

            ViewBag.QrCodeBase64 = user.QrCodeBase64;
            return View();
        }

        // This action allows the student to download their QR code in different formats
        [HttpPost]
        public async Task<IActionResult> DownloadQrCode(string format = "png")
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.QrCodeBase64))
            {
                return NotFound("User not found or QR code not available.");
            }

            byte[] qrCodeBytes = Convert.FromBase64String(user.QrCodeBase64);
            using (MemoryStream ms = new MemoryStream(qrCodeBytes))
            {
                if (format.ToLower() == "pdf")
                {
                    using (MemoryStream pdfStream = new MemoryStream())
                    {
                        PdfWriter writer = new PdfWriter(pdfStream);
                        PdfDocument pdf = new PdfDocument(writer);
                        Document document = new Document(pdf);

                        ImageData imageData = ImageDataFactory.Create(qrCodeBytes);
                        iText.Layout.Element.Image pdfImage = new iText.Layout.Element.Image(imageData);
                        document.Add(pdfImage);

                        document.Close();
                        return File(pdfStream.ToArray(), "application/pdf", "QRCode.pdf");
                    }
                }
                else
                {
                    ImageFormat imageFormat;
                    string mimeType;

                    switch (format.ToLower())
                    {
                        case "jpeg":
                            imageFormat = ImageFormat.Jpeg;
                            mimeType = "image/jpeg";
                            break;
                        case "bmp":
                            imageFormat = ImageFormat.Bmp;
                            mimeType = "image/bmp";
                            break;
                        case "png":
                        default:
                            imageFormat = ImageFormat.Png;
                            mimeType = "image/png";
                            break;
                    }

                    using (Bitmap bitmap = new Bitmap(ms))
                    {
                        using (MemoryStream outStream = new MemoryStream())
                        {
                            bitmap.Save(outStream, imageFormat);
                            return File(outStream.ToArray(), mimeType, $"QRCode.{format}");
                        }
                    }
                }
            }
        }
    }
}
