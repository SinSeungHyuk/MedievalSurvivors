using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Field;
using MS.Manager;
using MS.Mode;
using MS.Skill;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using static MS.Field.FieldObject;

public class TestUIController : MonoBehaviour
{
    [SerializeField] private Button BtnModeStart;
    [SerializeField] private Button BtnClear;
    [SerializeField] private Button BtnTP;
    [SerializeField] private Button BtnSpawn;


    void Start()
    {
        BtnModeStart.onClick.AddListener(()
            =>
        {
            GameManager.Instance.ChangeMode(new SurvivalMode(DataManager.Instance.StageSettingDataDict["Stage1"]));
        });

        BtnClear.onClick.AddListener(()
             =>
        {
            GameManager.Instance.CurGameMode.EndMode();
        });

        BtnTP.onClick.AddListener(()
            =>
        {
            //var StatRewardDict = DataManager.Instance.StatRewardSettingDataDict;
            //var rewards = new List<StatRewardSettingData>();
            //var statValues = Enum.GetValues(typeof(EStatType));
            //var gradeValues = Enum.GetValues(typeof(EGrade));

            //// 등급 가중치 총합 계산

            //while (rewards.Count < 3)
            //{
            //    // 1. 스탯 랜덤
            //    var rndStat = (EStatType)statValues.GetValue(UnityEngine.Random.Range(0, statValues.Length));

            //    // 2. 등급 가중치 랜덤
            //    var rndGrade = EGrade.Normal;
            //    int rndPoint = UnityEngine.Random.Range(0, 100);
            //    int currentSum = 0;
            //    foreach (EGrade g in gradeValues)
            //    {
            //        currentSum += (int)g;
            //        if (rndPoint < currentSum) { rndGrade = g; break; }
            //    }

            //    // 3. 리스트 내 중복 검사 (스탯과 등급이 모두 같은지 확인)
            //    if (rewards.Any(x => x.StatType == rndStat && x.Grade == rndGrade)) continue;

            //    // 4. 데이터 조회 (Key = 스탯이름 + 등급이름)
            //    string key = rndStat.ToString() + rndGrade.ToString();
            //    if (StatRewardDict.TryGetValue(key, out var data))
            //    {
            //        rewards.Add(data);
            //    }
            //}


            var SkillDict = DataManager.Instance.SkillSettingDataDict;
            var rewards = new List<string>();

            var playerSkillKeys = SkillDict
                .Where(x => x.Value.OwnerType == FieldObjectType.Player)
                .Select(x => x.Key)
                .ToList();

            while (rewards.Count < 3 && playerSkillKeys.Count > 0)
            {
                int randomIndex = UnityEngine.Random.Range(0, playerSkillKeys.Count);
                rewards.Add(playerSkillKeys[randomIndex]);
                playerSkillKeys.RemoveAt(randomIndex);
            }
        });

        BtnSpawn.onClick.AddListener(()
            =>
        {
            PlayerManager.Instance.Player.ApplyStatEffect("test", EStatType.CoinMultiple, 1, EBonusType.Flat);
            //PlayerManager.Instance.Player.ApplyStatEffect("test", EStatType.AreaRangeMultiple, 1, EBonusType.Flat);
        });
    }
}
