using FluentMigrator;

namespace OzonEdu.StockApi.Migrator.Temp
{
    [Migration(8)]
    public class FillDictionaries : ForwardOnlyMigration
    {
        public override void Up()
        {
            Execute.Sql(@"
                INSERT INTO MerchPackType (Id, Name)
                VALUES
                    (10, 'Welcome'),
                    (20, 'Starter'),
                    (30, 'ConferenceListener'),
                    (40, 'ConferenceSpeaker'),
                    (50, 'Veteran')
                ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO MerchOrderStatus (Id, Name)
                VALUES
                    (10, 'InWork'),
                    (20, 'IsReserved'),
                    (30, 'IsDone')
                ON CONFLICT DO NOTHING
            ");

            Execute.Sql(@"
                INSERT INTO MerchRequestType (Id, Name)
                VALUES
                    (10, 'Auto'),
                    (20, 'Manual')
                ON CONFLICT DO NOTHING
            ");
        }
    }
}