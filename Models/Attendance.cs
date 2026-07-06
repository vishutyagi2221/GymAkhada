using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymAkhada.Models
{
    public class Attendance
    {
        [Key]
        public int AttendanceId { get; set; }

        [Required]
        public int GymMember_ID { get; set; }

        [ForeignKey("GymMember_ID")]
        public GymMember? GymMember { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now.Date;

        [Required]
        public bool IsPresent { get; set; } = true;

        public bool IsApproved { get; set; } = false;
    }
}
