using MS.Data;
using MS.Field;
using MS.Mode;
using MS.Utils;
using TMPro;
using UnityEngine;

namespace MS.UI
{
    using TMPro;

    public class BattlePanel : BaseUI
    {
        private ExpBar expBar;
        private TextMeshProUGUI txtGold;
        private TextMeshProUGUI txtKillCount;
        private TextMeshProUGUI txtTimer;
        private TextMeshProUGUI txtWaveCount;
        private TextMeshProUGUI txtLevel;

        private BattlePanelData curData;


        public void InitBattlePanel(BattlePanelData _data)
        {
            if (expBar == null) FindUIComponents();

            expBar.InitExpBar();

            curData = _data;
            curData.KillCount.Subscribe(OnKillCountChanged);
            curData.WaveTimer.Subscribe(OnTimerChanged);
            curData.WaveCount.Subscribe(OnWaveCountChanged);
            curData.PlayerGold.Subscribe(OnGoldChanged);
            curData.PlayerLevel.Subscribe(OnLevelChanged);
            curData.PlayerCurExp.Subscribe(OnExpChanged);

            curData.OnBossSpawned += OnBossSpawnedCallback;
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

        #region Bind
        private void OnKillCountChanged(int _prev, int _cur)
        {
            txtKillCount.text = _cur.ToString("N0");
        }

        private void OnGoldChanged(int _prev, int _cur)
        {
            txtGold.text = _cur.ToString("N0");
        }

        private void OnTimerChanged(float _prev, float _cur)
        {
            int minutes = (int)_cur / 60;
            int seconds = (int)_cur % 60;
            txtTimer.text = string.Format("{0}:{1:D2}", minutes, seconds);
        }

        private void OnWaveCountChanged(int _prev, int _cur)
        {
            txtWaveCount.text = "Wave "+ _cur.ToString();
        }

        private void OnLevelChanged(int _prev, int _cur)
        {
            txtLevel.text = "Lv. " + _cur.ToString();
        }

        private void OnExpChanged(float _prev, float _cur)
        {
            float ratio = _cur / curData.PlayerMaxExp.Value;
            expBar.UpdateExpBar(ratio);
        }

        private void OnBossSpawnedCallback()
        {
            txtTimer.text = "Boss";
        }
        #endregion

        public override void Close()
        {
            base.Close();

            curData.KillCount.Unsubscribe(OnKillCountChanged);
            curData.WaveTimer.Unsubscribe(OnTimerChanged);
            curData.WaveCount.Unsubscribe(OnWaveCountChanged);
            curData.PlayerGold.Unsubscribe(OnGoldChanged);
            curData.PlayerLevel.Unsubscribe(OnLevelChanged);
            curData.PlayerCurExp.Unsubscribe(OnExpChanged);
        }
    }
}