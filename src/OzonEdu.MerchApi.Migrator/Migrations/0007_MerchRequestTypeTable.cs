using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(7)]
    public class MerchRequestTypeTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("MerchRequestType")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("MerchRequestType");
        }
    }
}