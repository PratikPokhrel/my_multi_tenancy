﻿using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Tenancy
{
    public interface ITenantProvider
    {
        Tenant GetTenant();
    }
}
