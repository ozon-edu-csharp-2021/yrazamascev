using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(3)]
    public class ItemPackTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("ItemPack")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("MerchPack_id").AsInt64().NotNullable()
                .WithColumn("StockItem_id").AsInt64().NotNullable()
                .WithColumn("Quantity").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("ItemPack");
        }
    }
}