using Unity.Scripting.LifecycleManagement;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace FireAlt.MonoSystems
{
    /// <summary>
    /// Contains methods and properties related to event buses and event types in the Unity application.
    /// </summary>
    [NoAutoStaticsCleanup]
    public static partial class EventBusUtil {
        public static IReadOnlyList<Type> EventTypes { get; set; }
        public static IReadOnlyList<Type> EventBusTypes { get; set; }
    
        [OnEnteringPlayMode]
        public static void Initialize() {
            EventTypes = PredefinedAssemblyUtil.GetTypes(typeof(IEvent));
            EventBusTypes = InitializeAllBuses();
        }

        [OnExitingPlayMode]
        private static void OnExitingPlayMode() => ClearAllBuses();

        static List<Type> InitializeAllBuses() {
            List<Type> eventBusTypes = new List<Type>();
        
            var typedef = typeof(EventBus<>);
            foreach (var eventType in EventTypes) {
                var busType = typedef.MakeGenericType(eventType);
                eventBusTypes.Add(busType);
            }
        
            return eventBusTypes;
        }

        /// <summary>
        /// Clears (removes all listeners from) all event buses in the application.
        /// </summary>
        public static void ClearAllBuses() {
            if (EventBusTypes == null) return;

            for (int i = 0; i < EventBusTypes.Count; i++) {
                var busType = EventBusTypes[i];
                var clearMethod = busType.GetMethod("Clear", BindingFlags.Static | BindingFlags.NonPublic);
                clearMethod?.Invoke(null, null);
            }
        }
    }
}
