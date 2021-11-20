using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(6)]
    public class MerchOrderStatusTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("merch_order_status")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("merch_order_status");
        }
    }
}