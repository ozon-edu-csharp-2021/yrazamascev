using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(2)]
    public class SkuPackTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("sku_pack")
                .WithColumn("id").AsInt64().Identity().PrimaryKey()
                .WithColumn("merch_order_id").AsInt64().NotNullable()
                .WithColumn("sku_id").AsInt64().NotNullable()
                .WithColumn("quantity").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("sku_pack");
        }
    }
}