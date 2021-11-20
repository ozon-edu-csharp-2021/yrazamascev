using FluentMigrator;

namespace OzonEdu.MerchApi.Migrator.Migrations
{
    [Migration(5)]
    public class MerchOrderTable : Migration
    {
        public override void Up()
        {
            Execute.Sql(@"
                CREATE TABLE IF NOT EXISTS merch_order (
	                id BIGSERIAL NOT NULL,
	                merch_pack_type_id INT NOT NULL,
	                merch_order_status_id INT NOT NULL,
	                merch_request_type_id INT NOT NULL,
	                in_work_at TIMESTAMPTZ NOT NULL,
	                reserve_at TIMESTAMPTZ NULL,
	                done_at TIMESTAMPTZ NULL,
	                employee_id BIGSERIAL NOT NULL,
                    CONSTRAINT ""PK_merch_order"" PRIMARY KEY (id));");
        }

        public override void Down()
        {
            Execute.Sql("DROP TABLE IF EXISTS merch_order;");
        }
    }
}