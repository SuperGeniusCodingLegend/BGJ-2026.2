using UnityEngine;

namespace FlightRisk
{
    public interface IServiceProvider<T> where T : Component
    {
        public static T Service { get; set; }
        public T OfferService();
    }
}