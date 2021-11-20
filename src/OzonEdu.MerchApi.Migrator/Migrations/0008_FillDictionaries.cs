using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Temp
{
    [Migration(8)]
    public class FillDictionaries : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO merch_pack_type (id, name)
                VALUES
                    (10, 'Welcome'),
                    (20, 'Starter'),
                    (30, 'ConferenceListener'),
                    (40, 'ConferenceSpeaker'),
                    (50, 'Veteran')
                ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO merch_order_status (id, name)
                VALUES
                    (10, 'InWork'),
                    (20, 'IsReserved'),
                    (30, 'IsDone')
                ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO merch_request_type (id, name)
                VALUES
                    (10, 'Auto'),
                    (20, 'Manual')
                ON CONFLICT DO NOTHING
            ");
        }
    }
}