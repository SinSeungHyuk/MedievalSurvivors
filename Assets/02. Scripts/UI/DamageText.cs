using Cysharp.Threading.Tasks;
using MS.Manager;
using MS.Utils;
using System;
using System.Threading;
using TMPro;
using UnityEngine;


namespace MS.UI
{
    public class DamageText : MonoBehaviour
    {
        private TextMeshProUGUI txtDamage;
        private float moveSpeed = 2.0f;
        private float duration = 0.8f;
        private Vector3 baseScale = new Vector3(0.1f, 0.1f, 0.1f);
        private static Camera mainCam;


        private void Awake()
        {
            if (txtDamage == null) txtDamage = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtDamage");
            if (mainCam == null) mainCam = Camera.main;
        }

        public void InitDamageText(int _damage, bool _isCritic)
        {
            transform.localScale = _isCritic ? baseScale * 1.5f : baseScale;

            if (mainCam != null)
                transform.rotation = mainCam.transform.rotation;

            txtDamage.text = _damage.ToString();

            txtDamage.color = _isCritic ? Settings.Critical : Color.white;

            AnimateAndReturn().Forget();
        }

        private async UniTaskVoid AnimateAndReturn()
        {
            float elapsedTime = 0f;
            CancellationToken token = this.GetCancellationTokenOnDestroy();

            while (elapsedTime < duration)
            {
                if (token.IsCancellationRequested || !gameObject.activeInHierarchy) return;

                float dt = Time.deltaTime;
                elapsedTime += dt;

                transform.Translate(Vector3.up * moveSpeed * dt, Space.World);

                await UniTask.Yield(PlayerLoopTiming.Update, token);
            }

            ReturnToPool();
        }

        private void ReturnToPool()
        {
            ObjectPoolManager.Instance.Return("DamageText", this.gameObject);
        }
    }
}