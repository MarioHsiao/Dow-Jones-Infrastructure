using System;
using System.Collections.Generic;
using System.Data;
using DowJonesSnapshot.Models;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;
using Orchard.Indexing;

namespace DowJonesSnapshot.DataMigrations
{
    public class Migrations : DataMigrationImpl
    {

        public int Create()
        {
            // Creating table MapRecord
            SchemaBuilder.CreateTable("ComponentRecord", table => table
                .ContentPartRecord()
                .Column("componentName", DbType.String)
                .Column("componentData", DbType.String)
            );

            ContentDefinitionManager.AlterPartDefinition(
                typeof(SnapshotPart).Name, cfg => cfg.Attachable());

            return 1;
        }


        //public int UpdateFrom2()
        //{
        //    ContentDefinitionManager.AlterTypeDefinition("ComponentRecord", cfg => cfg
        //      .WithPart("CommonPart")
        //      .WithPart("RoutePart")
        //      .WithPart("BodyPart")
        //      .WithPart("ProductPart")
        //      .WithPart("CommentsPart")
        //      .WithPart("TagsPart")
        //      .WithPart("LocalizationPart")
        //      .Creatable()
        //      .Indexed());
        //    return 3;
        //}

    }
}