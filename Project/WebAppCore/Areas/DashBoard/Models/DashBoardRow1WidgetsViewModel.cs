﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.DashBoard.Models
{
    public class DashBoardRow1WidgetsViewModel
    {
        public string TotalNoOfEmployees { get; set; }
        public string NoOfEmployeesCreatedToday { get; set; }
        public string NoOfEmployeesPendingAuth { get; set; }

        public string TodayNoOfApplicationErrors { get; set; }
        public string ErrorLastLoggedOn { get; set; }
        public string TotalNoOfApplicationErrors { get; set; }

        public string TotalNoOfServicesScheduled { get; set; }
        public string ServicesLastChecked { get; set; }
        public string TotalNoOfServicesDown { get; set; }

        public string TotalNoOfApplicationUsers { get; set; }
        public string NoOfActiveUsers { get; set; }
        public string NoOfActiveSessions { get; set; }
    }
}
