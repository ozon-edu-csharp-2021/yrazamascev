using CSharpCourse.Core.Lib.Enums;

using OzonEdu.MerchApi.Domain.Models;

namespace OzonEdu.MerchApi.Domain.AggregationModels.Enumerations
{
    public class MerchPackType : Enumeration
    {
        public static MerchPackType Welcome = new((int)MerchType.WelcomePack, nameof(Welcome));
        public static MerchPackType Starter = new((int)MerchType.ProbationPeriodEndingPack, nameof(Starter));
        public static MerchPackType ConferenceListener = new((int)MerchType.ConferenceListenerPack, nameof(ConferenceListener));
        public static MerchPackType ConferenceSpeaker = new((int)MerchType.ConferenceSpeakerPack, nameof(ConferenceSpeaker));
        public static MerchPackType Veteran = new((int)MerchType.VeteranPack, nameof(Veteran));

        public MerchPackType(int id, string name) : base(id, name)
        {
        }
    }
}