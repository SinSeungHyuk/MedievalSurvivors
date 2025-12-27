using Core;
using MS.UI;
using UnityEngine;


namespace MS.Manager
{
    public class UIManager : Singleton<UIManager>
    {




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