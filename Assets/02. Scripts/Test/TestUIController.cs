using MS.Data;
using MS.Manager;
using MS.Mode;
using UnityEngine;
using UnityEngine.UI;

public class TestUIController : MonoBehaviour
{
    [SerializeField] private Button BtnModeStart;
    [SerializeField] private Button BtnClear;


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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
