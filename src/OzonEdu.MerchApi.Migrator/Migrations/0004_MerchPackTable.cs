using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(4)]
    public class MerchPackTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("merch_pack")
                .WithColumn("id").AsInt64().Identity().PrimaryKey()
                .WithColumn("merch_pack_type_id").AsInt32().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("merch_pack");
        }
    }
}