using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models.Migrations
{
    [Migration(201404041400)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup() : base(StoreModuleDescriptor.ModuleName) { }
        public override void Up()
        {
            Create.Table("Categories").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Name_en").AsString(MaxLength.Name).NotNullable()
                .WithColumn("ParentId").AsGuid().NotNullable()
                .WithColumn("SortOrder").AsInt32().Nullable();

            Create.Table("Products").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("CategoryId").AsGuid().Nullable()
                .WithColumn("Code").AsString(MaxLength.Text).NotNullable()
                .WithColumn("Size").AsString(MaxLength.Text).Nullable()
                .WithColumn("Color").AsString(MaxLength.Text).Nullable()
                .WithColumn("Description").AsString(MaxLength.Max).Nullable()
                .WithColumn("Description_en").AsString(MaxLength.Max).Nullable()
                .WithColumn("ImageId").AsGuid().Nullable()
                .WithColumn("IsFeature").AsBoolean()
                .WithColumn("SortOrder").AsInt32().Nullable();
        }
    }
}