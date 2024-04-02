﻿using SV20T1020610.DataLayers;
using SV20T1020610.DataLayers.SQLSever;
using SV20T1020610.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020610.BusinessLayers
{
    public static class UserAccountService
    {
        private static readonly IUserAccountDAL employeeAcountDB;
        static UserAccountService()
        {
            employeeAcountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
        }
        public static UserAccount? Authorize(string userName, string password)
        {
            return employeeAcountDB.Authorize(userName,password);
        }
        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {

            return employeeAcountDB.ChangePassword(userName, oldPassword, newPassword);

        }
    }
}
