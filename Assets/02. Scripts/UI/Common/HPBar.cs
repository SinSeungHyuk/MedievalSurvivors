using MS.Field;
using MS.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace MS.UI
{
    public class HPBar : MonoBehaviour
    {
        private Image imgBar;


        public void InitHPBar(FieldCharacter _owner)
        {
            imgBar = transform.FindChildComponentDeep<Image>("ResourceBar");
            imgBar.fillAmount = 1;
            _owner.SSC.AttributeSet.OnHealthChanged += UpdateHPBar;
        }

        private void UpdateHPBar(float _curHp, float _maxHp)
        {
            imgBar.fillAmount = _curHp / _maxHp;
        }
    }
}