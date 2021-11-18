using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(6)]
    public class MerchOrderStatusTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("MerchOrderStatus")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("MerchOrderStatus");
        }
    }
}