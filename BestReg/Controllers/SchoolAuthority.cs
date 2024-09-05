using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BestReg.Controllers
{
    [Authorize(Roles = "SchoolSecurity")]
    public class SchoolAuthorityController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ScanIn(string qrCode)
        {
            // Optionally process the QR code here if needed
            // For example, you might want to store or validate the QR code

            // Redirect to the ScanQRCode page
            return RedirectToAction("ScanQRCode");
        }

        public IActionResult ScanOut()
        {
            // Logic for scanning learners out
            return View();
        }
    }
}
