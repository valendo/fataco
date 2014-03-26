using FluentMigrator.VersionTableInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Models.Migrations
{
    [VersionTableMetaData]
    public class MigrationVersioning : IVersionTableMetaData
    {
        public string ColumnName
        {
            get { return "Version"; }
        }

        public string SchemaName
        {
            get { return "bcms_" + FaqModuleDescriptor.ModuleName; }
        }

        public string TableName
        {
            get { return "VersionInfo"; }
        }

        public string UniqueIndexName
        {
            get { return "uc_VersionInfo_Version_" + FaqModuleDescriptor.ModuleName; }
        }
    }
}