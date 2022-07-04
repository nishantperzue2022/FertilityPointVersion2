using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FertilityPoint.DAL.Enumerations
{
    public class Enumerations
    {
        public enum AppointmentStatus
        {
            [Description("Approved")]
            Approved,
            [Description("Pending")]
            Pending,

        }
    }
}
