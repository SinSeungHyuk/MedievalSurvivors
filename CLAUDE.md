# CLAUDE.md

이 파일은 Claude Code(claude.ai/code)가 이 저장소의 코드를 다룰 때 참고하는 가이드입니다.

## 프로젝트 개요

**MedievalSurvivors**는 Unity 3D Android 로그라이트 생존 게임(뱀파이어 서바이버즈 스타일)입니다. 단일 씬(`Assets/01. Scenes/MainScene.unity`) 구성이며, Google Play Android를 타겟으로 합니다. 코드 주석은 **한국어**로 작성합니다.

## 빌드

빌드는 Unity Editor 또는 Jenkins를 통해 수행합니다:
- **Jenkins 스크립트**: `Assets/02. Scripts/Editor/JenkinsBuild.cs`
- 출력 경로: `Builds/Windows/MedievalSurvival.apk` (Android 타겟이지만 "Windows" 폴더명은 의도된 것)
- 자동화 테스트는 없으며, Unity Editor에서 게임을 실행하여 테스트합니다.

## 코드 아키텍처

### 네임스페이스 & 폴더 구조

```
Assets/02. Scripts/
├── Core/           → Core 네임스페이스 — 기반 패턴 (Singleton, StateMachine 등)
├── Data/           → MS.Data — 설정 데이터 구조체, GlobalDefine, 열거형
├── Editor/         → 에디터 전용 스크립트 (JenkinsBuild)
├── FieldObject/    → MS.Field — 모든 런타임 게임 오브젝트
├── Manager/        → MS.Manager — 싱글턴 매니저 클래스
│   ├── CoreManager/        → ObjectPoolManager, EffectManager, GameplayCueManager
│   └── FieldObjectManager/ → MonsterManager, FieldItemManager, PlayerManager, SkillObjectManager
├── Mode/           → MS.Mode — 게임 모드 상태 머신
├── Skill/          → MS.Skill — 스킬 시스템, 어트리뷰트 셋, 상태 효과
├── UI/             → MS.UI — 모든 UI 패널 및 팝업
└── Utils/          → MS.Utils — Settings 상수, 수학 헬퍼
```

네임스페이스 규칙: 기반 인프라는 `Core` 네임스페이스, 모든 게임 로직은 `MS.*` 네임스페이스를 사용합니다.

### 핵심 패턴

**싱글턴**: 순수 C# 싱글턴은 `Core.Singleton<T>`를 상속합니다. MonoBehaviour 싱글턴은 `Core.MonoSingleton<T>`를 상속합니다(`Core/Singleton.cs`).

**상태 머신**: `MSStateMachine<OwnerType>`(`Core/StateMachine/`) — 제네릭, 데이터 기반. 상태는 `int` ID로 등록하며 `onEnter/onUpdate/onExit` 콜백을 가집니다. 전환은 지연 처리됩니다(다음 `OnUpdate` 시작 시 적용). `GameModeBase`와 개별 캐릭터에서 사용합니다.

**반응형 프로퍼티**: `MSReactProp<T>`(`Core/MSReactProp.cs`) — `Subscribe(Action<T,T>)`를 가진 옵저버블 래퍼. UI 데이터 바인딩에 사용합니다(예: `KillCount`, `CurLevel`). 

**스탯 시스템**: `Stat` 클래스(`Core/Stat.cs`) — 기본값 + 키 기반 보너스 수정자(Flat 또는 Percentage). `AttributeSet` 클래스가 스탯을 집계하고 `EStatType` 열거형으로 노출합니다(`Data/`가 아닌 `Skill/AttributeSet/BaseAttributeSet.cs`에 정의됨).

### 부분 클래스 규칙

대형 클래스는 부분 클래스(partial class)로 여러 파일에 분할합니다:
- `SurvivalMode` → 4개 파일: `SurvivalMode.cs`, `SurvivalMode_Load.cs`, `SurvivalMode_BattleStart.cs`, `SurvivalMode_LastWave.cs`
- `GameManager` → 2개 파일: `GameManager.cs`(코어), `GameManager_SDK.cs`(Firebase/GPGS 연동)

### 필드 오브젝트 계층

```
FieldObject (추상, MS.Field)
├── FieldCharacter (추상) — SkillSystemComponent + Animator 보유
│   ├── PlayerCharacter — PlayerController, LevelSystem, PlayerArtifact 추가
│   └── MonsterCharacter
├── SkillObject (추상) — 지속시간/히트횟수 생명주기, 풀 키
│   ├── ProjectileObject
│   ├── AreaObject
│   └── IndicatorObject
└── FieldItem
    └── ResourceItem
```

`FieldCharacter.Awake`에서 `AddComponent`로 `SkillSystemComponent`를 동적 추가합니다.

### 스킬 시스템

- `SkillSystemComponent`(MonoBehaviour) — 런타임에 부착됨. 캐릭터의 `BaseAttributeSet`과 `BaseSkill` 인스턴스 목록을 보유합니다.
- `BaseSkill`(추상) — 쿨타임 및 DPS 추적 관리. `ActivateSkill(CancellationToken)`은 async(UniTask)입니다.
- 플레이어/몬스터 스킬은 각각 `Skill/PlayerSkill/`, `Skill/MonsterSkill/`에 위치합니다.
- 어트리뷰트 셋: `BaseAttributeSet` → `PlayerAttributeSet` / `MonsterAttributeSet`.
- **리플렉션 기반 인스턴스화**: `SkillSystemComponent`는 `Type.GetType("MS.Skill." + skillKey)`로 스킬 인스턴스를 생성합니다. 스킬 클래스명은 JSON 데이터의 `skillKey`와 정확히 일치해야 하며 `MS.Skill` 네임스페이스에 있어야 합니다.

### 매니저 레이어

모든 매니저는 `.Instance`로 접근하는 싱글턴입니다:

| 매니저 | 타입 | 역할 |
|---|---|---|
| `GameManager` | MonoSingleton | 진입점. targetFrameRate=60 설정, SDK 초기화 |
| `AddressableManager` | Singleton | Unity Addressables 래핑, 핸들 캐싱 |
| `DataManager` | Singleton | 시작 시 Addressables로 모든 JSON 설정 로드 |
| `StringTable` | Singleton | 로컬라이제이션. Addressables로 JSON에서 문자열 로드 |
| `UIManager` | MonoSingleton | View/Popup/System 캔버스 레이어, 스택 기반 팝업 |
| `ObjectPoolManager` | MonoSingleton | Addressable 기반 오브젝트 풀 |
| `MonsterManager` | Singleton | 활성 몬스터 추적, 공간 쿼리(`GetNearestMonster`) |
| `FieldItemManager` | Singleton | 필드 아이템 추적 |
| `PlayerManager` | Singleton | 플레이어 추적 |
| `SkillObjectManager` | Singleton | 활성 스킬 오브젝트 추적 |
| `EffectManager` | MonoSingleton | 파티클 이펙트 풀 |
| `SoundManager` | MonoSingleton | BGM + SFX |
| `CameraManager` | MonoSingleton | 추적 + 흔들림 |
| `GameplayCueManager` | Singleton | ScriptableObject 기반 큐 시스템 |

### 게임 모드 흐름

두 가지 게임 모드가 있습니다: `LobbyMode`와 `SurvivalMode`, 둘 다 `GameModeBase`를 상속합니다.

`SurvivalMode` 상태 머신: `Load → BattleStart → LastWave → (BattleEnd)`.

`SurvivalMode.OnUpdate`는 `SkillObjectManager`, `MonsterManager`, `EffectManager`, `battlePanel`을 수동으로 틱합니다 — 이 시스템들은 Unity의 `Update()`를 사용하지 **않습니다**.

`SurvivalMode.EndMode`는 모든 매니저 상태를 정리하고 `ObjectPoolManager.ClearAllPools()`를 호출해야 합니다.

### 에셋 로드 패턴

모든 에셋(프리팹, 오디오, JSON 데이터)은 **Addressables**로 로드합니다:
- 문자열 키로 개별 에셋 로드: `AddressableManager.Instance.LoadResourceAsync<T>(key)`
- 레이블로 그룹 로드: `AddressableManager.Instance.LoadResourcesLabelAsync<T>(label)`
- 사용 중인 레이블: `"UI"`, `"GameplayCue"`, `"StringTable"`, JSON 설정별 개별 키

오브젝트 풀은 `Get()` 호출 전에 반드시 `ObjectPoolManager.Instance.CreatePoolAsync(key, count)`로 생성해야 합니다. 몬스터는 Load 상태에서 일괄 풀링됩니다.

### UI 시스템

- `BaseUI` → `Panel` 또는 `Popup`
- `UIManager.ShowView<T>(key)` — 현재 뷰 교체(ViewCanvas)
- `UIManager.ShowPopup<T>(key)` — PopupCanvas에 스택 추가. sortingOrder = `1 + (popupStack.Count * 10)`
- `UIManager.ShowSystemUI<T>(key)` — SystemCanvas에 매번 새로 인스턴스화(View/Popup과 달리 캐싱 안 함)
- UI 프리팹은 `"UI"` 레이블로 `uiPrefabDict`에 로드, View/Popup 인스턴스는 `cachedUIDict`에 캐싱

### 설정 & 밸런스

- 모든 게임 밸런스는 JSON으로 관리하며, `DataManager`가 시작 시 로드합니다:
  `SkillSettingData`, `CharacterSettingData`, `MonsterSettingData`, `StageSettingData`, `ItemSettingData`, `StatRewardSettingData`, `SoundSettingData`, `ArtifactSettingData`
- 런타임 상수는 `Utils/Settings.cs`에 있습니다(애니메이션 해시, 레이어 마스크, 웨이브 타이머, 스폰 거리)
- 등급 시스템(일반/희귀/고유/전설)은 `Data/GlobalDefine.cs`에 정의됩니다.

### 서드파티 라이브러리

- **UniTask** (`Cysharp.Threading.Tasks`) — 모든 비동기 작업. `CancellationToken`을 스킬 활성화에 전달합니다. SDK 콜백 API는 `UniTaskCompletionSource<T>`로 래핑합니다.
- **DOTween** (`DG.Tweening`) — 트윈 (예: 보스 처치 시 아티팩트 점프)
- **Newtonsoft.Json** — 설정 데이터 JSON 역직렬화
- **Firebase** — `Assets/Firebase/` — 인증, 시작 시 자동 로그인
- **GPGS** — `Assets/GooglePlayGames/` — 리더보드, 업적, 시작 시 자동 로그인


## 코드 스타일
- 함수의 매개변수는 앞에 '_'를 붙입니다.
- private,protected 지역변수는 소문자로, public 변수는 대문자로 시작합니다.
- 함수명은 충분히 알아볼 수 있도록 작성하며, UniTask를 사용하는 함수는 뒤에 Async를 붙이고, bool을 반환하는 함수는 Is를 앞에 붙이고, 이벤트 콜백 함수는 대체적으로 앞에 On을 붙입니다.