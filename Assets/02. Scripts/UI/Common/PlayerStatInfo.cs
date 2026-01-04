using MS.Manager;
using MS.Utils;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


namespace MS.UI
{
    public class PlayerStatInfo : MonoBehaviour
    {
        private RectTransform statRowContainer;
        private PlayerStatInfoRow statInfoRowTemplate;


        public void InitPlayerStatInfo(BaseAttributeSet _attributeSet)
        {
            FindComponents();

            foreach (Transform child in statRowContainer)
            {
                if (child.gameObject == statInfoRowTemplate.gameObject) continue; // 템플릿은 파괴하지 말고 냅둬야함
                Destroy(child.gameObject);
            }

            foreach (EStatType statType in Enum.GetValues(typeof(EStatType)))
            {
                Stat targetStat = _attributeSet.GetStatByType(statType);
                if (targetStat == null) continue;

                // 몬스터 전용 스탯 등 표시하지 않을 예외 처리
                if (statType == EStatType.AttackRange) continue;

                PlayerStatInfoRow newRowObj = Instantiate(statInfoRowTemplate, statRowContainer);
                newRowObj.gameObject.SetActive(true);

                // StringTable을 이용해 스탯 이름을 로컬라이징하여 전달
                string statName = StringTable.Instance.Get("StatType", statType.ToString());
                if (string.IsNullOrEmpty(statName)) statName = statType.ToString();
                float statValue = targetStat.Value;

                newRowObj.InitPlayerStatInfoRow(statName, statValue);
            }
        }

        private void FindComponents()
        {
            if (statRowContainer != null) return;

            statRowContainer = transform.FindChildComponentDeep<RectTransform>("StatRowContainer");
            statInfoRowTemplate = transform.GetOrAddComponent<PlayerStatInfoRow>("PlayerStatInfoRowTemplate");
            statInfoRowTemplate.gameObject.SetActive(false);
        }
    }

    public class PlayerStatInfoRow : MonoBehaviour
    {
        private TextMeshProUGUI txtStatType;
        private TextMeshProUGUI txtStatValue;


        private void Awake()
        {
            txtStatType = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtStatType");
            txtStatValue = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtStatValue");
        }

        public void InitPlayerStatInfoRow(string _statName, float _statValue)
        {
            txtStatType.text = _statName;
            txtStatValue.text = _statValue.ToString("0.#");
        }
    }
}