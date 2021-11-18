using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(5)]
    public class MerchOrderTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("MerchOrder")
                .WithColumn("Id").AsInt64().Identity().PrimaryKey()
                .WithColumn("MerchPackType_id").AsInt32().NotNullable()
                .WithColumn("MerchOrderStatus_id").AsInt32().NotNullable()
                .WithColumn("MerchRequestType_id").AsInt32().NotNullable()
                .WithColumn("InWorkAt").AsDateTimeOffset(0).NotNullable()
                .WithColumn("ReserveAt").AsDateTimeOffset(0).Nullable()
                .WithColumn("DoneAt").AsDateTimeOffset(0).Nullable()
                .WithColumn("Employee_id").AsInt64().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("MerchOrder");
        }
    }
}