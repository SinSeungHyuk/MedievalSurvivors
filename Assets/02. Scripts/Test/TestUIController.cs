using Cysharp.Threading.Tasks;
using MS.Data;
using MS.Manager;
using MS.Mode;
using UnityEngine;
using UnityEngine.UI;

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
            PlayerManager.Instance.Player.SSC.UseSkill("Teleport").Forget();
        });

        BtnSpawn.onClick.AddListener(()
            =>
        {
            // todo test
            Vector3 spawnPos = GameManager.Instance.CurGameMode.CurFieldMap.GetRandomSpawnPoint(PlayerManager.Instance.Player.Position);

            MonsterManager.Instance.SpawnMonster("Skeleton_Tier1", spawnPos, Quaternion.identity);
        });
    }
}
