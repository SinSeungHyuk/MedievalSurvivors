using MS.Core;
using MS.Field;
using MS.Mode;

namespace MS.Data
{
    public class BattlePanelData
    {
        public MSReactProp<int> KillCount { get; private set; }
        public MSReactProp<int> WaveCount { get; private set; }
        public MSReactProp<float> WaveTimer { get; private set; }
        public MSReactProp<int> PlayerGold { get; private set; }
        public MSReactProp<int> PlayerLevel { get; private set; }
        public MSReactProp<float> PlayerMaxExp { get; private set; }
        public MSReactProp<float> PlayerCurExp { get; private set; }


        public BattlePanelData(SurvivalMode _mode, PlayerCharacter _player)
        {
            KillCount = _mode.KillCount;
            WaveCount = _mode.CurWaveCount;
            WaveTimer = _mode.CurWaveTimer;

            PlayerGold = _player.LevelSystem.Gold;
            PlayerLevel = _player.LevelSystem.CurLevel;
            PlayerMaxExp = _player.LevelSystem.MaxExp;
            PlayerCurExp = _player.LevelSystem.CurExp;
        }
    }
}