using Unity.Cinemachine;
using UnityEngine;

namespace Series.Core
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    [AddComponentMenu("Series/Platformer/Player/Player Camera")]
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Camera Settings")]
        public Player player;
        public float maxDistance = 15f;
        public float heightOffset = 1f;
        public float upwardRotationSpeed = 90f;

        protected float m_cameraDistance;
        protected float m_cameraTargetYaw;

        protected Vector3 m_cameraTargetPosition;
        protected Quaternion m_currentUpRotation;

        protected Camera m_camera;
        protected CinemachineVirtualCamera m_virtualCamera;
        protected Cinemachine3rdPersonFollow m_cameraBody;
        protected CinemachineBrain m_brain;

        protected Transform m_target;
        protected const string k_targetName = "Player Follower Camera Target";

        public bool freeze { get; set; }

        // Variables to store initial Y and Z positions
        private float initialYPosition;
        private float initialZPosition;

        protected virtual void InitializeComponents()
        {
            if (!player)
            {
                player = FindObjectOfType<Player>();
            }

            m_camera = Camera.main;
            m_virtualCamera = GetComponent<CinemachineVirtualCamera>();
            m_cameraBody = m_virtualCamera.AddCinemachineComponent<Cinemachine3rdPersonFollow>();
            m_brain = m_camera.GetComponent<CinemachineBrain>();
        }

        protected virtual void InitializeFollower()
        {
            m_target = new GameObject(k_targetName).transform;
            m_target.position = player.transform.position;

            // Store the initial Y and Z positions
            initialYPosition = m_target.position.y;
            initialZPosition = m_target.position.z;
        }

        protected virtual void InitializeCamera()
        {
            m_virtualCamera.Follow = m_target;
            m_virtualCamera.LookAt = player.transform;

            Reset();
        }

        public virtual void Reset()
        {
            m_cameraDistance = maxDistance;
            m_cameraTargetYaw = player.transform.rotation.eulerAngles.y;
            m_cameraTargetPosition = player.transform.position + player.transform.up * heightOffset;
            m_currentUpRotation = Quaternion.FromToRotation(Vector3.up, player.transform.up);
            MoveTarget();
            m_brain.ManualUpdate();
        }

        protected virtual void HandleOffset()
        {
            // Update X position to match the player's X position
            m_cameraTargetPosition.x = player.transform.position.x;

            // Reset Y and Z positions to their initial values to lock movement along those axes
            m_cameraTargetPosition.y = initialYPosition;
            m_cameraTargetPosition.z = initialZPosition;
        }

        protected virtual void MoveTarget()
        {
            float upRotationDelta = upwardRotationSpeed * Time.deltaTime;
            Quaternion upRotation = Quaternion.FromToRotation(Vector3.up, player.transform.up);

            m_target.position = m_cameraTargetPosition;

            m_currentUpRotation = Quaternion.RotateTowards(m_currentUpRotation, upRotation, upRotationDelta);
            m_target.rotation = m_currentUpRotation * Quaternion.Euler(0f, m_cameraTargetYaw, 0f);

            m_cameraBody.CameraDistance = m_cameraDistance;
        }

        protected virtual void Start()
        {
            InitializeComponents();
            InitializeFollower();
            InitializeCamera();
        }

        protected virtual void LateUpdate()
        {
            if (freeze) return;

            HandleOffset();
            MoveTarget();
        }
    }
}