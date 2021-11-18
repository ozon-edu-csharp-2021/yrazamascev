using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(2)]
    public class SkuPackTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("SkuPack")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("MerchOrder_id").AsInt64().NotNullable()
                .WithColumn("Sku_id").AsInt64().NotNullable()
                .WithColumn("Quantity").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("SkuPack");
        }
    }
}