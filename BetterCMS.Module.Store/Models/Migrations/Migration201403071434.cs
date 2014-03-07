using BetterCms.Core.DataAccess.DataContext.Migrations;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models.Migrations
{
    [Migration(201403071434)]
    public class Migration201403071434 : DefaultMigration
    {
        public Migration201403071434() : base(StoreModuleDescriptor.ModuleName) { }
        public override void Up()
        {
            Create.Table("Products").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("CategoryId").AsGuid().Nullable()
                .WithColumn("Code").AsString().NotNullable()
                .WithColumn("Size").AsString().Nullable()
                .WithColumn("Color").AsString().Nullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Description_en").AsString().Nullable()
                .WithColumn("ImageId").AsGuid().Nullable()
                .WithColumn("IsFeature").AsBoolean()
                .WithColumn("SortOrder").AsInt32().Nullable();
        }
    }
}