using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.FAQ.Models.Migrations
{
    [Migration(201404041400)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup() : base(FaqModuleDescriptor.ModuleName) { }
        public override void Up()
        {
            Create.Table("Categories").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("Name_en").AsString(MaxLength.Name).NotNullable()
                .WithColumn("SortOrder").AsInt32().Nullable();

            Create.Table("Faqs").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("CategoryId").AsGuid().Nullable()
                .WithColumn("Question").AsString(MaxLength.Text).NotNullable()
                .WithColumn("Question_en").AsString(MaxLength.Text).Nullable()
                .WithColumn("Answer").AsString(MaxLength.Max).Nullable()
                .WithColumn("Answer_en").AsString(MaxLength.Max).Nullable()
                .WithColumn("SortOrder").AsInt32().Nullable();
        }
    }
}