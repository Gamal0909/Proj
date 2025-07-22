using UnityEngine;

namespace Series.Core
{
    [AddComponentMenu("Series/Platformer/Misc/Power Regeneration")]
    public class PowerRegeneration : MonoBehaviour
    {
        [Header("Regeneration Settings")]
        [SerializeField] private bool _autoRegenerate = true;
        [SerializeField] private float _regenerationRate = 2f; // Power per second
        [SerializeField] private float _regenerationDelay = 3f; // Delay after last use before regeneration starts
        [SerializeField] private bool _onlyRegenerateWhenNotEmpty = false; // Some games only regen if you have some power left
        [SerializeField] private bool _onlyRegenerateWhenLightOff = true; // Only regenerate when light is not being used

        private Power _power;
        private PlayerLightSystem _lightSystem; // Reference to check if light is being used
        private PlayerInputManager _inputManager; // Reference to check light input
        private float _lastRegenerationTime;
        private float _regenerationTimer;

        void Start()
        {
            _power = GetComponent<Power>();
            if (_power == null)
            {
                Debug.LogError("PowerRegeneration requires a Power component on the same GameObject!");
                enabled = false;
                return;
            }

            // Try to find the light system and input manager
            _lightSystem = GetComponent<PlayerLightSystem>();
            _inputManager = GetComponent<PlayerInputManager>();

            if (_onlyRegenerateWhenLightOff && (_lightSystem == null || _inputManager == null))
            {
                Debug.LogWarning("PowerRegeneration: Light system or input manager not found. Will regenerate regardless of light state.");
                _onlyRegenerateWhenLightOff = false;
            }
        }

        void Update()
        {
            if (!_autoRegenerate || _power == null) return;

            // Check if we should regenerate
            bool canRegenerate = _power.currentFloat < _power.max;

            // Optional: only regenerate if not completely empty
            if (_onlyRegenerateWhenNotEmpty && _power.isEmpty)
                canRegenerate = false;

            // Check if light is currently being used
            bool lightIsOn = false;
            if (_onlyRegenerateWhenLightOff && _inputManager != null)
            {
                lightIsOn = _inputManager.GetLight() && !_power.isEmpty;
            }

            // Don't regenerate if light is on and we have that restriction enabled
            if (_onlyRegenerateWhenLightOff && lightIsOn)
            {
                canRegenerate = false;

                // Log occasionally for debugging
                if (Time.frameCount % 120 == 0) // Every 2 seconds
                {
                    Debug.Log("Power regeneration paused - light is on");
                }
            }

            if (canRegenerate)
            {
                // Check if enough time has passed since last power use
                float timeSinceLastUse = Time.time - GetLastUseTime();

                if (timeSinceLastUse >= _regenerationDelay)
                {
                    // Smooth regeneration every frame
                    float powerToAdd = _regenerationRate * Time.deltaTime;
                    _power.IncreaseFloat(powerToAdd);

                    // Log occasionally for debugging
                    if (Time.frameCount % 120 == 0) // Every 2 seconds
                    {
                        Debug.Log($"Power Regenerating: +{powerToAdd:F4}/frame (+{_regenerationRate}/sec), Current: {_power.current} ({_power.currentFloat:F2})/{_power.max}");
                    }
                }
            }
        }

        /// <summary>
        /// Gets the last use time from the Power component using reflection
        /// since m_lastUseTime is protected
        /// </summary>
        private float GetLastUseTime()
        {
            var field = typeof(Power).GetField("m_lastUseTime",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            if (field != null)
            {
                return (float)field.GetValue(_power);
            }

            // Fallback: assume regeneration can always happen if we can't access the field
            return 0f;
        }

        /// <summary>
        /// Manually start regeneration (ignores delay and light state)
        /// </summary>
        public void ForceRegeneration()
        {
            if (_power != null && _power.currentFloat < _power.max)
            {
                float powerToAdd = _regenerationRate;
                _power.IncreaseFloat(powerToAdd);
                Debug.Log($"Force Regenerated: +{powerToAdd}, Current: {_power.current} ({_power.currentFloat:F2})/{_power.max}");
            }
        }

        /// <summary>
        /// Enable/disable auto regeneration
        /// </summary>
        public void SetAutoRegeneration(bool enabled)
        {
            _autoRegenerate = enabled;
        }

        /// <summary>
        /// Enable/disable light-dependent regeneration
        /// </summary>
        public void SetLightDependentRegeneration(bool enabled)
        {
            _onlyRegenerateWhenLightOff = enabled;
        }
    }
}