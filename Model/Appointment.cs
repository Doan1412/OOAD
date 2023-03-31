using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Appointment
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public Appointment() { }

        public Appointment(string title, DateTime startTime, DateTime endTime, string description, List<User> user)
        {
            Title = title;
            StartTime = startTime;
            EndTime = endTime;
            Description = description;
        }

        public override string ToString()
        {
            return $"Title: {Title}\nStart Time: {StartTime}\nEnd Time: {EndTime}\nDescription: {Description}\n";
        }
    }
}
