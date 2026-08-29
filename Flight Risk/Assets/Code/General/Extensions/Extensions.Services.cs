using System;
using UnityEngine;

namespace FlightRisk
{
    public static partial class Extensions
    {
        public static bool InjectService<T>(this IServiceProvider<T> provider, T service) where T : Component
        {
            if (IServiceProvider<T>.Service != null && IServiceProvider<T>.Service != service) return false;
            if (service == null) return false;

            IServiceProvider<T>.Service = service;
            Debug.Log($"Provider {provider} has successfully injected service: {service} from {service.gameObject.name}");
            return true;
        }

        public static void WaitForService<T>(this IRequireService<T> requester, Action<T> onServiceGet) where T : Component
        {
            requester.WaitUntill(() => IServiceProvider<T>.Service != null, () => onServiceGet?.Invoke(IServiceProvider<T>.Service));
        }
    }
}