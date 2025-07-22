using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Rendering.HighDefinition;

namespace Series.Core
{

    public class PlayerLightSystem : MonoBehaviour
    {
        [Header("Light Control")] [SerializeField]
        private float _LightIntensity = 10f;

        [SerializeField] private float _LightRange = 10f;
        [SerializeField] private float _LightIntensitySpeed = 5f;
        [SerializeField] private float _LightRangeSpeed = 5f;

        [Header("Light Detection")] [SerializeField]
        private bool _autoDetectLight = true;

        [SerializeField] private string _lightGameObjectName = ""; // Optional: specify light name

        [Header("Manual Light Assignment")] [SerializeField]
        private HDAdditionalLightData _manualLightData; // Direct component reference

        [Header("Lamp Mat")] [SerializeField] private GameObject _HeadLampMat;
        [SerializeField] private float _HeadLampEmmis = 1f;

        [Header("Light Usage")] [SerializeField]
        private float _LightUsage = 5f; // How much power is used per second when the light is on

        [Header("Health Usage")] 
        [SerializeField] private float _HealthRegenPerSecond = 10f;


        Material _HeadLampMaterial;
        HDAdditionalLightData lightData;

        private PlayerInputManager inputs;
        private Power power;
        private Health _health;
        private Player player;

        void Start()
        {
            inputs = GetComponent<PlayerInputManager>();
            if (inputs == null)
            {
                Debug.LogError("PlayerInputManager not found on this GameObject!");
                return;
            }

            player = GetComponent<Player>();
            if (player != null)
            {
                power = player.power;
                _health = player.health;
            }
            else
            {
                _health = GetComponent<Health>();
                power = GetComponent<Power>();
            }

            // Initialize light data
            InitializeLightData();

            // Initialize material
            if (_HeadLampMat != null)
            {
                Renderer renderer = _HeadLampMat.GetComponent<Renderer>();
                if (renderer != null)
                {
                    _HeadLampMaterial = renderer.material;
                    Debug.Log($"HeadLamp material initialized: {_HeadLampMaterial.name}");
                }
                else
                {
                    Debug.LogWarning("Renderer component not found on HeadLamp GameObject!");
                }
            }
        }

        void InitializeLightData()
        {
            // First, try manual assignment
            if (_manualLightData != null)
            {
                lightData = _manualLightData;
                Debug.Log($"Using manually assigned light: {lightData.gameObject.name}");
            }
            // Then try auto-detection
            else if (_autoDetectLight)
            {
                // Try to find by name first if specified
                if (!string.IsNullOrEmpty(_lightGameObjectName))
                {
                    GameObject namedLight = GameObject.Find(_lightGameObjectName);
                    if (namedLight != null)
                    {
                        lightData = namedLight.GetComponent<HDAdditionalLightData>();
                        if (lightData != null)
                        {
                            Debug.Log($"Found light by name: {_lightGameObjectName}");
                        }
                    }
                }

                // If still no light found, search for any HDRP light
                if (lightData == null)
                {
                    // First check children
                    HDAdditionalLightData[] childLights = GetComponentsInChildren<HDAdditionalLightData>();
                    if (childLights.Length > 0)
                    {
                        lightData = childLights[0];
                        Debug.Log($"Found child light: {lightData.gameObject.name}");
                    }
                    else
                    {
                        // Check parent
                        HDAdditionalLightData parentLight = GetComponentInParent<HDAdditionalLightData>();
                        if (parentLight != null)
                        {
                            lightData = parentLight;
                            Debug.Log($"Found parent light: {lightData.gameObject.name}");
                        }
                        else
                        {
                            // Last resort: find any in scene
                            HDAdditionalLightData sceneLight = FindObjectOfType<HDAdditionalLightData>();
                            if (sceneLight != null)
                            {
                                lightData = sceneLight;
                                Debug.Log($"Found scene light: {lightData.gameObject.name}");
                            }
                        }
                    }
                }
            }

            if (lightData == null)
            {
                Debug.LogError("No HDAdditionalLightData found! Please either:\n" +
                               "1. Assign it manually in the 'Manual Light Assignment' field\n" +
                               "2. Make sure there's an HDRP light as a child/parent/in scene\n" +
                               "3. Specify the light's GameObject name in 'Light GameObject Name'");
                return;
            }

            // Initialize light state
            Debug.Log($"Successfully initialized light: {lightData.gameObject.name}");
            lightData.intensity = 0;
        }

        void Update()
        {
            if (inputs == null || power == null || lightData == null) return;

            float powerPercent = power.max > 0 ? power.currentFloat / power.max : 0f;

            bool lightPressed = inputs.GetLight();
            bool canUseLight = lightPressed && !power.isEmpty && !_health.isEmpty;

            if (canUseLight)
            {
                float targetIntensity = _LightIntensity * powerPercent;
                float targetRange = _LightRange * powerPercent;

                lightData.intensity = Mathf.Lerp(lightData.intensity, targetIntensity, Time.deltaTime * _LightIntensitySpeed);
                lightData.range = Mathf.Lerp(lightData.range, targetRange, Time.deltaTime * _LightRangeSpeed);

                if (_HeadLampMaterial != null)
                {
                    Color emissionColor = Color.white * _HeadLampEmmis * powerPercent;
                    _HeadLampMaterial.SetColor("_EmissionColor",
                        Color.Lerp(_HeadLampMaterial.GetColor("_EmissionColor"), emissionColor, Time.deltaTime * 5));
                }

                float powerToUse = _LightUsage * Time.deltaTime;
                power.UseFloat(powerToUse);

                if (Time.frameCount % 60 == 0)
                {
                    Debug.Log($"[Light ON] Power -{powerToUse:F2} (Health increases only)");
                }
            }
            else
            {
                lightData.intensity = Mathf.Lerp(lightData.intensity, 0, Time.deltaTime * _LightIntensitySpeed);
                lightData.range = Mathf.Lerp(lightData.range, 0, Time.deltaTime * _LightRangeSpeed);

                if (_HeadLampMaterial != null)
                {
                    _HeadLampMaterial.SetColor("_EmissionColor",
                        Color.Lerp(_HeadLampMaterial.GetColor("_EmissionColor"), Color.black, Time.deltaTime * 15));
                }

                if (Time.frameCount % 60 == 0 && lightPressed)
                {
                    Debug.Log($"[Light Blocked] Power empty: {power.isEmpty}, Health empty: {_health.isEmpty}");
                }
            }

            // ✅ Always increase health if not full
            if (!inputs.GetLight() &&_health.currentFloat < _health.max)
            {
                float regenAmount = _HealthRegenPerSecond * Time.deltaTime;
                _health.SetFloat(_health.currentFloat + regenAmount);

                if (Time.frameCount % 60 == 0)
                {
                    Debug.Log($"[Health Regen] +{regenAmount:F2} (Light ON or OFF)");
                }
            }
        }


     // Runtime methods to change light reference
        public void SetLightData(HDAdditionalLightData newLightData)
        {
            lightData = newLightData;
            Debug.Log($"Light data changed to: {(lightData != null ? lightData.gameObject.name : "null")}");
        }

        public void RefreshLightData()
        {
            lightData = null;
            InitializeLightData();
        }
    }
}