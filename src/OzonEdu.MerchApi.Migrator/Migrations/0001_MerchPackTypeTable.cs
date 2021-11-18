using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(1)]
    public class MerchPackTypeTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("MerchPackType")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("MerchPackType");
        }
    }
}