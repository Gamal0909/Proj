using UnityEngine;
using UnityEngine.Events;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/Misc/Power")]
    public class Power : MonoBehaviour
    {
        public int initial = 100;
        public int max = 100;
        public float coolDown = 1f;
        
        /// <summary>
        /// Called when the Power count changed.
        /// </summary>
        public UnityEvent onChange;
        /// <summary>
        /// Called when it Use.
        /// </summary>
        public UnityEvent onUse;
        
        protected float m_currentPowerFloat; // Internal float value for smooth calculations
        protected int m_currentPower; // Public integer value
        protected float m_lastUseTime;
        
        /// <summary>
        /// Returns the current amount of Power.
        /// </summary>
        public int current
        {
            get { return m_currentPower; }
            protected set
            {
                var last = m_currentPower;
                var clampedValue = Mathf.Clamp(value, 0, max);
                if (clampedValue != last)
                {
                    m_currentPower = clampedValue;
                    m_currentPowerFloat = clampedValue; // Keep float in sync
                    onChange?.Invoke();
                }
            }
        }
        
        /// <summary>
        /// Returns the current amount of Power as a float for smooth calculations.
        /// </summary>
        public float currentFloat
        {
            get { return m_currentPowerFloat; }
        }
        
        /// <summary>
        /// Returns true if the Power is empty.
        /// </summary>
        public virtual bool isEmpty => m_currentPowerFloat <= 0f;
        
        /// <summary>
        /// Returns true if it's still recovering from the last Use of Power.
        /// </summary>
        public virtual bool recovering => Time.time < m_lastUseTime + coolDown;
        
        /// <summary>
        /// Sets the current Power to a given amount.
        /// </summary>
        /// <param name="amount">The total Power you want to set.</param>
        public virtual void Set(int amount) => current = amount;
        
        /// <summary>
        /// Sets the current Power to a given amount (float version).
        /// </summary>
        /// <param name="amount">The total Power you want to set.</param>
        public virtual void SetFloat(float amount)
        {
            var last = m_currentPower;
            m_currentPowerFloat = Mathf.Clamp(amount, 0f, max);
            var newIntValue = Mathf.RoundToInt(m_currentPowerFloat);
            
            if (newIntValue != last)
            {
                m_currentPower = newIntValue;
                onChange?.Invoke();
            }
        }
        
        /// <summary>
        /// Increases the amount of Power.
        /// </summary>
        /// <param name="amount">The amount you want to increase.</param>
        public virtual void Increase(int amount) => current += amount;
        
        /// <summary>
        /// Increases the amount of Power (float version).
        /// </summary>
        /// <param name="amount">The amount you want to increase.</param>
        public virtual void IncreaseFloat(float amount)
        {
            SetFloat(m_currentPowerFloat + amount);
        }
        
        /// <summary>
        /// Decreases the amount of Power.
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
        /// Decreases the amount of Power smoothly (float version).
        /// </summary>
        /// <param name="amount">The amount you want to decrease.</param>
        public virtual void UseFloat(float amount)
        {
            var last = m_currentPower;
            m_currentPowerFloat = Mathf.Clamp(m_currentPowerFloat - Mathf.Abs(amount), 0f, max);
            var newIntValue = Mathf.RoundToInt(m_currentPowerFloat);
            
            if (newIntValue != last)
            {
                m_currentPower = newIntValue;
                onChange?.Invoke();
            }
            
            // Update last use time for any usage
            if (amount > 0)
            {
                m_lastUseTime = Time.time;
                onUse?.Invoke();
            }
        }
        
        /// <summary>
        /// Set the current Power back to its initial value.
        /// </summary>
        public virtual void ResetPower() => current = initial;
        
        protected virtual void Awake() 
        {
            current = initial;
            m_currentPowerFloat = initial;
        }
    }
}