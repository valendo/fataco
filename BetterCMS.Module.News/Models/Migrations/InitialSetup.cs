using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.News.Models.Migrations
{
    [Migration(201403141615)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup() : base(NewsModuleDescriptor.ModuleName) { }
        public override void Up()
        {
            Create.Table("Categories").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Name_en").AsString(MaxLength.Name).NotNullable()
                .WithColumn("ParentId").AsGuid().NotNullable()
                .WithColumn("SortOrder").AsInt32().Nullable();

            Create.Table("News").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("CategoryId").AsGuid().Nullable()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Title_en").AsString().Nullable()
                .WithColumn("Summary").AsString().Nullable()
                .WithColumn("Summary_en").AsString().Nullable()
                .WithColumn("Content").AsString().Nullable()
                .WithColumn("Content_en").AsString().Nullable()
                .WithColumn("PublishDate").AsDateTime().Nullable()
                .WithColumn("ImageId").AsGuid().Nullable()
                .WithColumn("SortOrder").AsInt32().Nullable();
        }
    }
}