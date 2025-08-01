using System;
using UnityEngine;
using UnityEngine.Events;

namespace Series.Core
{
    [Serializable]
    public class EntityEvents
    {
        /// <summary>
        /// Called when the Entity lands on the ground.
        /// </summary>
        public UnityEvent OnGroundEnter;

        /// <summary>
        /// Called when the Entity leaves the ground.
        /// </summary>
        public UnityEvent OnGroundExit;

        /// <summary>
        /// Called when the Entity enters a rail.
        /// </summary>
        public UnityEvent OnRailsEnter;

        /// <summary>
        /// Called when the Entity exists a rail.
        /// </summary>
        public UnityEvent OnRailsExit;
    }
}