using Core;
using Unity.Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;


namespace MS.Manager
{
    public class CameraManager : MonoSingleton<CameraManager>
    {
        [SerializeField] private CinemachineCamera mainCamera;
        private CinemachineBasicMultiChannelPerlin noiseComponent;

        public CinemachineCamera MainCamera => mainCamera;


        protected override void Awake()
        {
            base.Awake();
            noiseComponent = mainCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void InitMainCamera(Transform _target)
        {
            mainCamera.Follow = _target;
        }

        public void ShakeCamera(float _intensity, float _duration)
        {
            CameraShakeAsync(_intensity, _duration).Forget();
        }

        private async UniTaskVoid CameraShakeAsync(float _intensity, float _duration)
        {
            if (noiseComponent == null) return;

            // 1. 진동 시작
            noiseComponent.AmplitudeGain = _intensity;

            // 2. 시간 대기 (UniTask 활용)
            await UniTask.WaitForSeconds(_duration);

            // 3. 진동 서서히 감소 (Linear하게 0으로)
            float elapsed = 0f;
            float fadeOutTime = 0.2f; // 부드럽게 멈추기 위한 페이드 아웃 시간

            while (elapsed < fadeOutTime)
            {
                elapsed += Time.deltaTime;
                noiseComponent.AmplitudeGain = Mathf.Lerp(_intensity, 0f, elapsed / fadeOutTime);
                await UniTask.Yield();
            }

            noiseComponent.AmplitudeGain = 0f;
        }
    }
}