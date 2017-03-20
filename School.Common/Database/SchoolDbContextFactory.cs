﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace School.Core.Database
{
    public class SchoolDbContextFactory : ISchoolDbContextFactory
    {
        public ISchoolDbContext Create()
        {
            return new SchoolDbContext();
        }
    }
}