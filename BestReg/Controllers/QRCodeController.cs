﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BestReg.Data;
using System;
using System.Threading.Tasks;

namespace BestReg.Controllers
{
    public class QrCodeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
      

        public QrCodeController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
           
        }

        // displaying the QR code for the logged-in student
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

        //allowing the student to download their QR code
        public async Task<IActionResult> DownloadQrCode()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.QrCodeBase64))
            {
                return NotFound("User not found or QR code not available.");
            }

            var qrCodeBytes = Convert.FromBase64String(user.QrCodeBase64);
            return File(qrCodeBytes, "image/png", "QRCode.png");
        }

  
    }
}
