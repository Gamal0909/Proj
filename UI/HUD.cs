using UnityEngine;
using UnityEngine.UI;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/UI/HUD")]
    public class HUD : MonoBehaviour
    {
        
        [Header("UI Elements")]
        public Slider health;
        public Slider stamina;
        public Slider power;
       
        protected Player m_player;

        protected float timerStep;
        protected static float timerRefreshRate = .1f;

        /// <summary>
        /// Set the stamina value to a given value.
        /// </summary>
        protected virtual void UpdateStamina()
        {
            stamina.value = m_player.stamina.current;
        }

        /// <summary>
        /// Set the power value to a given value.
        /// </summary>
        protected virtual void UpdatePower()
        {
            power.value = m_player.power.current;
        }

        /// <summary>
        /// Set the health value to a given value.
        /// </summary>
        protected virtual void UpdateHealth()
        {
            health.value = m_player.health.current;
        }

        /// <summary>
        /// Called to force an updated on the HUD.
        /// </summary>
        public virtual void Refresh()
        {
            UpdateStamina();
            UpdatePower();
            UpdateHealth();
        }

        protected virtual void Awake()
        {
                m_player = FindObjectOfType<Player>();
                m_player.health.onChange.AddListener(UpdateHealth);
                m_player.power.onChange.AddListener(UpdatePower);
                m_player.stamina.onChange.AddListener(UpdateStamina);
                Refresh();
        }
    }
}
