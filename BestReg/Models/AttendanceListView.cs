using System;
using System.Collections.Generic;

namespace BestReg.Models
{
    public class AttendanceListViewModel
    {
        public List<AttendanceRecordViewModel> CheckedInUsers { get; set; }
        public List<AttendanceRecordViewModel> CheckedOutUsers { get; set; }
    }

    public class AttendanceRecordViewModel
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IDNumber { get; set; }
        public string Role { get; set; }
        public DateTime CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
    }
}
