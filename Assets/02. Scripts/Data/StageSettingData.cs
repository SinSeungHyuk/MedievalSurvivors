using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MS.Data
{
    [Serializable]
    public class StageSettingData
    {
        public string MapKey { get; set; }
        public List<MonsterSpawnInfo> MonsterSpawnInfoPerWave { get; set; }
        public List<string> BossMonsterSpawnInfoPerWave { get; set; }
    }


    [Serializable]
    public class MonsterSpawnInfo
    {
        public string MonsterKey { get; set; }
        public int MonsterSpawnRate { get; set; }
    }
}