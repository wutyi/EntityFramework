﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data.SqlClient;
using Microsoft.Data.Entity.DependencyInjection;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.Data.Entity.Tests
{
    public class SqlServerTestHelpers : TestHelpers
    {
        protected SqlServerTestHelpers()
        {
        }

        public new static SqlServerTestHelpers Instance { get; } = new SqlServerTestHelpers();
        
        protected override EntityFrameworkServicesBuilder AddProviderServices(EntityFrameworkServicesBuilder builder)
        {
            return builder.AddSqlServer();
        }

        protected override DbContextOptions UseProviderOptions(DbContextOptions options)
        {
            options.UseSqlServer(new SqlConnection("Database=DummyDatabase"));

            return options;
        }
    }
}
