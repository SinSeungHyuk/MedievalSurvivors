using MS.Field;
using MS.Manager;
using UnityEngine;


namespace MS.Core
{
    [CreateAssetMenu(fileName = "GC_", menuName = "GameplayCue")]
    public class GameplayCue : ScriptableObject
    {
        [Header("Effects")]
        public string EffectKey; 
        public Vector3 EffectOffset;
        public Vector3 EffectRotation;
        public bool AttachToOwner;

        [Header("Sounds")]
        public string SoundKey;

        [Header("Camera")]
        public float CameraShakeIntensity;
        public float CameraShakeDuration;


        public virtual void Play(FieldObject _owner)
        {
            if (!string.IsNullOrEmpty(EffectKey))
            {
                Vector3 spawnPosition =
                    _owner.Position +
                    _owner.transform.TransformDirection(EffectOffset);
                Quaternion spawnRotation =
                    _owner.Rotation * Quaternion.Euler(EffectRotation);

                EffectManager.Instance.PlayEffect(
                    EffectKey,
                    spawnPosition,
                    spawnRotation
                );
            }

            if (!string.IsNullOrEmpty(SoundKey))
            {
                // SoundManager.Instance.PlaySound(SoundKey);
            }

            if (CameraShakeIntensity > 0)
            {
                // CameraManager.Instance.Shake(CameraShakeIntensity, CameraShakeDuration);
            }
        }
    }
}