// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using Microsoft.Data.Entity.Relational.Update;

namespace Microsoft.Data.Entity.Relational.Tests
{
    public class SqlGeneratorTest : SqlGeneratorTestBase
    {
        protected override SqlGenerator CreateSqlGenerator()
        {
            return new ConcreteSqlGenerator();
        }

        protected override string RowsAffected
        {
            get { return "provider_specific_rowcount()"; }
        }

        protected override string Identity
        {
            get { return "provider_specific_identity()"; }
        }

        private class ConcreteSqlGenerator : SqlGenerator
        {
            protected override void AppendIdentityWhereCondition(StringBuilder commandStringBuilder, ColumnModification columnModification)
            {
                commandStringBuilder
                    .Append(DelimitIdentifier(columnModification.ColumnName))
                    .Append(" = ")
                    .Append("provider_specific_identity()");
            }

            public override void AppendSelectAffectedCountCommand(StringBuilder commandStringBuilder, string tableName, string schemaName)
            {
                commandStringBuilder
                    .Append("SELECT provider_specific_rowcount();" + Environment.NewLine);
            }

            protected override void AppendRowsAffectedWhereCondition(StringBuilder commandStringBuilder, int expectedRowsAffected)
            {
                commandStringBuilder
                    .Append("provider_specific_rowcount() = " + expectedRowsAffected);
            }
        }
    }
}
