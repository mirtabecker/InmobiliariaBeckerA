﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmoBecker.Models
{
    public class RepositorioBase
    {
        protected readonly IConfiguration configuration;
        protected readonly string connectionString;

        protected RepositorioBase(IConfiguration configuration)
        {
            this.configuration = configuration;
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }
    }
}
