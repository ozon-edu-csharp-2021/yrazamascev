using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(7)]
    public class MerchRequestTypeTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("merch_request_type")
                .WithColumn("id").AsInt32().PrimaryKey()
                .WithColumn("name").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("merch_request_type");
        }
    }
}