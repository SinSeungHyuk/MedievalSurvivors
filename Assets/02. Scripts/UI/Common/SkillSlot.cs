using MS.Data;
using MS.Manager;
using MS.Utils;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace MS.UI
{
    public class SkillSlot : MonoBehaviour
    {
        public event Action<string> OnSkillSlotClicked;

        private Button btnSkillSlot;
        private Image imgSkillIcon;
        private Image overlayCooltime;
        private Image imgCooltime;

        private string skillKey;


        public void InitSkillSlot(string _key, SkillSettingData _skillData)
        {
            imgSkillIcon = transform.FindChildComponentDeep<Image>("ImgSkillIcon");
            overlayCooltime = transform.FindChildComponentDeep<Image>("OverlayCooltime");
            imgCooltime = transform.FindChildComponentDeep<Image>("ImgCooltime");
            btnSkillSlot = GetComponent<Button>();

            btnSkillSlot.onClick.AddListener(OnSkillSlotClickedCallback);

            skillKey = _key;
            Sprite skillIcon = AddressableManager.Instance.LoadResource<Sprite>(_skillData.IconKey);
            imgSkillIcon.sprite = skillIcon;
        }

        public void UpdateExpBar(float _ratio)
        {
            imgCooltime.fillAmount = _ratio;
        }

        private void OnSkillSlotClickedCallback()
        {
            OnSkillSlotClicked?.Invoke(skillKey);
        }
    }
}