using MS.Data;
using MS.Field;
using MS.Mode;
using MS.Utils;
using TMPro;
using UnityEngine;

namespace MS.UI
{
    public class BattlePanel : BaseUI
    {
        private ExpBar expBar;
        private TextMeshProUGUI txtGold;
        private TextMeshProUGUI txtKillCount;
        private TextMeshProUGUI txtTimer; 
        private TextMeshProUGUI txtWaveCount; 
        private TextMeshProUGUI txtLevel;


        public void InitBattlePanel(BattlePanelData _data)
        {
            if (expBar == null) FindUIComponents();

            _data.KillCount.Subscribe(Test);
        }


        private void FindUIComponents()
        {
            expBar = transform.FindChildComponentDeep<ExpBar>("ExpBar");
            txtGold = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtGold");
            txtKillCount = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtKillCount");
            txtTimer = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtTimer");
            txtWaveCount = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtWaveCount");
            txtLevel = transform.FindChildComponentDeep<TextMeshProUGUI>("TxtLevel");
        }

        private void Test(int _prev, int _next)
        {
            txtKillCount.text = _next.ToString();
        }

        public override void Close()
        {
            base.Close();


        }
    }
}