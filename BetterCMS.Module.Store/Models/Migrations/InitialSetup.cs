﻿using BetterCms.Core.DataAccess.DataContext.Migrations;
using BetterCms.Core.Models;
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetterCMS.Module.Store.Models.Migrations
{
    [Migration(201402261030)]
    public class InitialSetup : DefaultMigration
    {
        public InitialSetup() : base(StoreModuleDescriptor.ModuleName) { }
        public override void Up()
        {
            Create.Table("ProductCategories").InSchema(SchemaName)
                .WithCmsBaseColumns()
                .WithColumn("Name").AsString(MaxLength.Name).NotNullable()
                .WithColumn("ParentId").AsGuid().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("ProductCategories").InSchema(SchemaName);
        }
    }
}