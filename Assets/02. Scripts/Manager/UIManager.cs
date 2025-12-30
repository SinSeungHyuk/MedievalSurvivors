using Core;
using MS.UI;
using MS.Utils;
using System;
using UnityEngine;


namespace MS.Manager
{
    public class UIManager : MonoSingleton<UIManager>
    {
        private Transform viewCanvas;
        private Transform popupCanvas;
        private Transform systemCanvas;


        protected override void Awake()
        {
            base.Awake();

            viewCanvas = transform.FindChildDeep("ViewCanvas");
            popupCanvas = transform.FindChildDeep("PopupCanvas");
            systemCanvas = transform.FindChildDeep("SystemCanvas");
        }


        #region Damage Text
        public void ShowDamageText(Vector3 _pos, int _damage, bool _isCritic)
        {
            GameObject textObj = ObjectPoolManager.Instance.Get("DamageText", _pos + Vector3.up * 2.0f, Quaternion.identity);
            var damageText = textObj.GetComponent<DamageText>();
            damageText.InitDamageText(_damage, _isCritic);
        }
        public void ShowEvasionText(Vector3 _pos)
        {
            GameObject textObj = ObjectPoolManager.Instance.Get("DamageText", _pos + Vector3.up * 2.0f, Quaternion.identity);
            var damageText = textObj.GetComponent<DamageText>();
            damageText.InitEvasionText();
        }
        #endregion
    }
}