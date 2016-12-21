﻿using SpaUserControl.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaUserControl.Infraestructure.Data
{
    public class AppDataContext : DbContext
    {

        public AppDataContext():base("")
        {

        }

        public DbSet<User> Users { get; set; }
    }
}
