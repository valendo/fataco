using BetterCms.Core.DataAccess.DataContext.Migrations;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models.Migrations
{
    [Migration(201403031010)]
    public class Migration201403031010 : DefaultMigration
    {
        public Migration201403031010() : base(StoreModuleDescriptor.ModuleName) { }

        public override void Up()
        {
            Create.Column("Lang")
                .OnTable("ProductCategories")
                .InSchema(SchemaName)
                .AsString().Nullable();
        }
    }
}