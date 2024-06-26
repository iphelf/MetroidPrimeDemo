using MetroidPrimeDemo.Scripts.General;
using UnityEngine;

namespace MetroidPrimeDemo.Scripts.Gameplay
{
    public class CrosshairCtrl : MonoBehaviour
    {
        [SerializeField] private RectTransform targetLock;
        [SerializeField] private float targetLockSmoothTime = 0.03f;
        [SerializeField] private RectTransform lockSeal;
        [SerializeField] private float lockingSmoothTime = 0.02f;
        [SerializeField] private float rotationSpeed = 120.0f;
        [SerializeField] private PlayerCharacterCtrl player;

        private Vector3 _screenCenter;
        private Vector3 _targetLockDampVelocity;
        private float _targetLockEulerDampVelocity;
        private float _pitch;
        private float _pitchDampVelocity;
        private float _yaw;
        private float _yawDampVelocity;

        private void Start()
        {
            _screenCenter = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f, 0.0f);
        }

        private void LateUpdate()
        {
            bool aimable = player.attributes.aimTarget is not null;
            bool locked = player.attributes.lockTarget is not null;
            if (locked)
            {
                lockSeal.gameObject.SetActive(true);
                UpdateLockedAim(player.attributes.lockTarget.transform.position);
            }
            else
            {
                lockSeal.gameObject.SetActive(false);
                lockSeal.position = _screenCenter;
                if (aimable)
                {
                    targetLock.gameObject.SetActive(true);
                    UpdateFreeAim(player.attributes.aimTarget.transform.position);
                }
                else
                {
                    targetLock.gameObject.SetActive(false);
                    targetLock.position = _screenCenter;
                }
            }
        }

        private void UpdateLockedAim(Vector3 worldPosition)
        {
            float targetLockRotationZ = targetLock.localEulerAngles.z;
            float targetLockRotationZTarget = Mathf.Round(targetLockRotationZ / 120.0f) * 120.0f;
            targetLockRotationZ = Mathf.SmoothDampAngle(
                targetLockRotationZ, targetLockRotationZTarget,
                ref _targetLockEulerDampVelocity, lockingSmoothTime
            );
            targetLock.localEulerAngles = new Vector3(0.0f, 0.0f, targetLockRotationZ);
            targetLock.position = player.camera.WorldToScreenPoint(worldPosition);

            GeometryHelpers.LookAt(player.camera.transform.position, worldPosition, out var pitch, out var yaw);
            GeometryHelpers.Normalize(ref pitch, ref yaw);
            player.GetRotation(out _pitch, out _yaw);
            _pitch = Mathf.SmoothDampAngle(_pitch, pitch, ref _pitchDampVelocity, lockingSmoothTime);
            _yaw = Mathf.SmoothDampAngle(_yaw, yaw, ref _yawDampVelocity, lockingSmoothTime);
            player.SetRotation(_pitch, _yaw);

            lockSeal.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime, Space.Self);
        }

        private void UpdateFreeAim(Vector3 worldPosition)
        {
            var targetPosition = player.camera.WorldToScreenPoint(worldPosition);
            targetLock.position = Vector3.SmoothDamp(
                targetLock.position, targetPosition,
                ref _targetLockDampVelocity, targetLockSmoothTime
            );
            targetLock.Rotate(0.0f, 0.0f, rotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}