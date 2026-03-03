# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**MedievalSurvivors** is a Unity 3D Android roguelite survival game (Vampire Survivors-style). Single scene (`Assets/01. Scenes/MainScene.unity`), targeting Android via Google Play.

## Build

Build is done via Unity Editor or Jenkins:
- **Jenkins script**: `Assets/02. Scripts/Editor/JenkinsBuild.cs`
- Output: `Builds/Windows/MedievalSurvival.apk` (Android target)
- There are no automated tests; testing is done by running the game in the Unity Editor.

## Code Architecture

### Namespace & Folder Structure

```
Assets/02. Scripts/
├── Core/           → MS.Core, Core namespaces — base patterns
├── Data/           → MS.Data — config data structs, GlobalDefine, enums
├── Editor/         → Editor-only scripts (JenkinsBuild)
├── FieldObject/    → MS.Field — all runtime game objects
├── Manager/        → MS.Manager — singleton manager classes
├── Mode/           → MS.Mode — game mode state machines
├── Skill/          → MS.Skill — skill system, attribute sets, status effects
├── UI/             → MS.UI — all UI panels and popups
└── Utils/          → MS.Utils — Settings constants, math helpers
```

### Core Patterns

**Singleton**: Pure C# singletons extend `Core.Singleton<T>`. MonoBehaviour singletons extend `Core.MonoSingleton<T>` (`Core/Singleton.cs`).

**State Machine**: `MSStateMachine<OwnerType>` (`Core/StateMachine/`) — generic, data-driven. States registered by `int` ID with `onEnter/onUpdate/onExit` callbacks. Transitions are deferred (applied at start of next `OnUpdate`). Used by `GameModeBase` and individual characters.

**Reactive Properties**: `MSReactProp<T>` (`Core/MSReactProp.cs`) — observable wrapper with `Subscribe(Action<T,T>)`. Used for UI data binding (e.g., `KillCount`, `CurLevel`).

**Stat System**: `Stat` class (`Core/Stat.cs`) — base value + keyed bonus modifiers (Flat or Percentage). `AttributeSet` classes aggregate stats and expose them by `EStatType` enum.

### Field Object Hierarchy

```
FieldObject (abstract, MS.Field)
├── FieldCharacter (abstract) — holds SkillSystemComponent + Animator
│   ├── PlayerCharacter — adds PlayerController, LevelSystem, PlayerArtifact
│   └── MonsterCharacter
├── SkillObject (abstract) — duration/hitcount lifecycle, pool key
│   ├── ProjectileObject
│   ├── AreaObject
│   └── IndicatorObject
└── FieldItem
    └── ResourceItem
```

`FieldCharacter.Awake` dynamically adds `SkillSystemComponent` via `AddComponent`.

### Skill System

- `SkillSystemComponent` (MonoBehaviour) — attached at runtime; holds the character's `BaseAttributeSet` and list of `BaseSkill` instances.
- `BaseSkill` (abstract) — manages cooltime, DPS tracking; `ActivateSkill(CancellationToken)` is async (UniTask).
- Player and monster skills live in `Skill/PlayerSkill/` and `Skill/MonsterSkill/`.
- Attribute sets: `BaseAttributeSet` → `PlayerAttributeSet` / `MonsterAttributeSet`.

### Manager Layer

All managers are singletons accessed via `.Instance`:

| Manager | Type | Responsibility |
|---|---|---|
| `AddressableManager` | Singleton | Wraps Unity Addressables; caches handles |
| `DataManager` | Singleton | Loads all JSON config at startup via Addressables |
| `UIManager` | MonoSingleton | View/Popup/System canvas layers; stack-based popup |
| `ObjectPoolManager` | MonoSingleton | Addressable-backed object pool |
| `MonsterManager` | Singleton | Tracks live monsters; spatial queries |
| `FieldItemManager` | Singleton | Tracks field items |
| `PlayerManager` | Singleton | Tracks the player |
| `SkillObjectManager` | Singleton | Tracks active skill objects |
| `EffectManager` | MonoSingleton | Particle effect pool |
| `SoundManager` | MonoSingleton | BGM + SFX |
| `CameraManager` | MonoSingleton | Follow + shake |
| `GameplayCueManager` | Singleton | ScriptableObject-based cue system |

### Game Mode Flow

`GameModeBase` → `SurvivalMode` (partial class across two files).

`SurvivalMode` state machine: `Load → BattleStart → LastWave → (BattleEnd)`.

`SurvivalMode.OnUpdate` manually ticks `SkillObjectManager`, `MonsterManager`, and `EffectManager` — these systems do **not** use Unity's `Update()`.

`SurvivalMode.EndMode` must clean up all manager state and call `ObjectPoolManager.ClearAllPools()`.

### Asset Loading Pattern

All assets (prefabs, audio, JSON data) are loaded via **Addressables**:
- Individual assets by string key: `AddressableManager.Instance.LoadResourceAsync<T>(key)`
- Groups by label: `AddressableManager.Instance.LoadResourcesLabelAsync<T>(label)`
- Labels in use: `"UI"`, `"GameplayCue"`, and per-asset keys for JSON config

Object pools must be created with `ObjectPoolManager.Instance.CreatePoolAsync(key, count)` before any `Get()` calls. Monsters are pooled in batches during the Load state.

### UI System

- `BaseUI` → `Panel` or `Popup`
- `UIManager.ShowView<T>(key)` — replaces current view (ViewCanvas)
- `UIManager.ShowPopup<T>(key)` — stacks on PopupCanvas with sorted `sortingOrder`
- `UIManager.ShowSystemUI<T>(key)` — overlaid on SystemCanvas (damage text, notifications)
- UI prefabs loaded by label `"UI"` into `uiPrefabDict`; instances cached in `cachedUIDict`

### Config & Balance

- All game balance is JSON, loaded by `DataManager` at startup:
  `SkillSettingData`, `CharacterSettingData`, `MonsterSettingData`, `StageSettingData`, `ItemSettingData`, `StatRewardSettingData`, `SoundSettingData`, `ArtifactSettingData`
- Runtime constants live in `Utils/Settings.cs` (animation hashes, layer masks, wave timer, spawn distances)
- Grade system (Normal/Rare/Unique/Legendary) defined in `Data/GlobalDefine.cs`

### Third-Party Libraries

- **UniTask** (`Cysharp.Threading.Tasks`) — all async operations; `CancellationToken` passed to skill activation
- **DOTween** (`DG.Tweening`) — tweens (e.g., artifact jump on boss death)
- **Newtonsoft.Json** — JSON deserialization for config data
- **Firebase** — `Assets/Firebase/`
- **GPGS** — `Assets/GooglePlayGames/` (leaderboards, achievements)
