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
        public bool AttachToOwner;

        [Header("Sounds")]
        public string SoundKey;

        [Header("Camera")]
        public float CameraShakeIntensity;
        public float CameraShakeDuration;


        public virtual void Play(FieldObject _owner)
        {
            Debug.Log($"Cue Àç»ý : {EffectKey}");

            if (!string.IsNullOrEmpty(EffectKey))
            {
                EffectManager.Instance.PlayEffect(
                    EffectKey,
                    _owner.transform.position + EffectOffset,
                    _owner.transform.rotation
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