namespace OzonEdu.MerchApi.Domain.Infrastructure.Configuration
{
    public class KafkaConfiguration
    {
        public string BootstrapServers { get; set; }
        public string EmployeeTopic { get; set; }
        public string GroupId { get; set; }
        public string StockTopic { get; set; }
    }
}