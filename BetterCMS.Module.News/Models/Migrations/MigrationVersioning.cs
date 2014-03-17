using FluentMigrator.VersionTableInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Models.Migrations
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
            get { return "bcms_" + NewsModuleDescriptor.ModuleName; }
        }

        public string TableName
        {
            get { return "VersionInfo"; }
        }

        public string UniqueIndexName
        {
            get { return "uc_VersionInfo_Version_" + NewsModuleDescriptor.ModuleName; }
        }
    }
}