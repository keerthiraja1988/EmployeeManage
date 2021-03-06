﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppCore.Areas.EmployeeManage.Models
{
    public class EmployeeSearchViewModel
    {
        public string SearchAllEmployee { get; set; }

        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string TIN { get; set; }
        public string Passport { get; set; }

        public string SearchText { get; set; }
        public string SearchColumn { get; set; }

        public DateTime? DateOfBirthStart { get; set; }
        public DateTime? DateOfBirthEnd { get; set; }

        public DateTime? DateOfJoiningStart { get; set; }
        public DateTime? DateOfJoiningEnd { get; set; }
    }
}
