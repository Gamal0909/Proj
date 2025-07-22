using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/Misc/Stamina")]
    public class Stamina : MonoBehaviour
    {
        public int initial = 100;
        public int max = 100;
        public float coolDown = 1f;


        /// <summary>
        /// Called when the Stamina count changed.
        /// </summary>
        public UnityEvent onChange;

        /// <summary>
        /// Called when it Get Used.
        /// </summary>
        public UnityEvent onUse;

        protected int m_currentStamina;
        protected float m_lastUseTime;


        /// <summary>
        /// Returns the current amount of Stamina.
        /// </summary>
        public int current
        {
            get { return m_currentStamina; }

            protected set
            {
                var last = m_currentStamina;

                if (value != last)
                {
                    m_currentStamina = Mathf.Clamp(value, 0, max);
                    onChange?.Invoke();
                }
            }
        }

        /// <summary>
        /// Returns true if the Stamina is empty.
        /// </summary>
        public virtual bool isEmpty => current == 0;

        /// <summary>
        /// Returns true if it's still recovering from the last Use.
        /// </summary>
        public virtual bool recovering => Time.time < m_lastUseTime + coolDown;

        /// <summary>
        /// Sets the current Stamina to a given amount.
        /// </summary>
        /// <param name="amount">The total Stamina you want to set.</param>
        public virtual void Set(int amount) => current = amount;

        /// <summary>
        /// Increases the amount of Stamina.
        /// </summary>
        /// <param name="amount">The amount you want to increase.</param>
        public virtual void Increase(int amount) => current += amount;


        /// <summary>
        /// Decreases the amount of Stamina.
        /// </summary>
        /// <param name="amount">The amount you want to decrease.</param>
        public virtual void Use(int amount)
        {
            if (!recovering)
            {
                current -= Mathf.Abs(amount);
                m_lastUseTime = Time.time;
                onUse?.Invoke();
            }
        }

        /// <summary>
        /// Set the current Stamina back to its initial value.
        /// </summary>
        public virtual void ResetStamina() => current = initial;

        protected virtual void Awake() => current = initial;
    }
}

