using BetterCms.Core.DataAccess.DataContext.Migrations;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models.Migrations
{
    [Migration(201403041555)]
    public class Migration201403041555 : DefaultMigration
    {
        public Migration201403041555() : base(StoreModuleDescriptor.ModuleName) { }

        public override void Up()
        {
            Create.Column("SortOrder")
                .OnTable("ProductCategories")
                .InSchema(SchemaName)
                .AsInt32().Nullable();
        }
    }
}