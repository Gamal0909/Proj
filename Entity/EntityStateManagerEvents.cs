using System;
using UnityEngine.Events;

namespace Series.Core
{
    [Serializable]
    public class EntityStateManagerEvents
    {
        /// <summary>
        /// Called when there's a state change.
        /// </summary>
        public UnityEvent onChange;

        /// <summary>
        /// Called when entering a state.
        /// </summary>
        public UnityEvent<Type> onEnter;

        /// <summary>
        /// Called when exiting a state.
        /// </summary>
        public UnityEvent<Type> onExit;
    }
}