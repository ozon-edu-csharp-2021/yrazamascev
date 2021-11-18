using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(4)]
    public class MerchPackTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("MerchPack")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("MerchPackType_id").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("MerchPack");
        }
    }
}