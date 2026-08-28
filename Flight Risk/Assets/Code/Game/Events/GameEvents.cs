using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace FlightRisk.Game
{
    public static class GameEvents
    {
        public enum Game : uint { Start, Fail, End }
        public enum Plane : uint { Start = 10, Update, Crash, End }
        public enum Passengers : uint { Start = 20, Update, End }
        public enum Encounters : uint { Start = 30, Spawn, Update, Success, Fail, End }
        public enum Interactions : uint { Enter = 40, Exit, TakeItem, GiveItem, OpenDialog, FireEffect }

        private static readonly Dictionary<uint, UnityEvent<object>> eventPool = new()
        {
            { (uint)Game.Start, new UnityEvent<object>() },
            { (uint)Game.Fail, new UnityEvent<object>() },
            { (uint)Game.End, new UnityEvent<object>() },

            { (uint)Plane.Start, new UnityEvent<object>() },
            { (uint)Plane.Update, new UnityEvent<object>() },
            { (uint)Plane.Crash, new UnityEvent<object>() },
            { (uint)Plane.End, new UnityEvent<object>() },

            { (uint)Passengers.Start, new UnityEvent<object>() },
            { (uint)Passengers.Update, new UnityEvent<object>() },
            { (uint)Passengers.End, new UnityEvent<object>() },

            { (uint)Encounters.Spawn, new UnityEvent<object>() },
            { (uint)Encounters.Update, new UnityEvent<object>() },
            { (uint)Encounters.Success, new UnityEvent<object>() },
            { (uint)Encounters.Fail, new UnityEvent<object>() },

            { (uint)Interactions.Enter, new UnityEvent<object>() },
            { (uint)Interactions.Exit, new UnityEvent<object>() },
            { (uint)Interactions.TakeItem, new UnityEvent<object>() },
            { (uint)Interactions.GiveItem, new UnityEvent<object>() },
            { (uint)Interactions.OpenDialog, new UnityEvent<object>() },
            { (uint)Interactions.FireEffect, new UnityEvent<object>() },
        };

        public static bool TrySubscribe(uint eventID, UnityAction<object> action)
        {
            if (action == null) return false;
            if (eventPool == null) return false;
            if (!eventPool.TryGetValue(eventID, out var gameEvent)) return false;
            gameEvent.AddListener(action);
            return true;
        }

        public static bool TryUnsubscribe(uint eventID, UnityAction<object> action)
        {
            if (action == null) return false;
            if (eventPool == null) return false;
            if (!eventPool.TryGetValue(eventID, out var gameEvent)) return false;
            gameEvent.RemoveListener(action);
            return true;
        }

        public static bool TryInvoke(uint eventID, object eventPackage = null)
        {
            if (eventPool == null) return false;
            if (!eventPool.TryGetValue(eventID, out var gameEvent)) return false;
            gameEvent?.Invoke(eventPackage);
            return true;
        }
    }
}