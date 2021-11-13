using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.Enumerations
{
    public class MerchPackType : Enumeration
    {
        public static MerchPackType Welcome = new(10, nameof(Welcome));
        public static MerchPackType Starter = new(20, nameof(Starter));
        public static MerchPackType ConferenceListener = new(30, nameof(ConferenceListener));
        public static MerchPackType ConferenceSpeaker = new(40, nameof(ConferenceSpeaker));
        public static MerchPackType Veteran = new(50, nameof(Veteran));

        public MerchPackType(int id, string name) : base(id, name)
        {
        }
    }
}