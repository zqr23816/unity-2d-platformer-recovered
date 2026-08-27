# Unity 毕业设计 Demo 代码恢复说明

## 恢复结论

本次恢复成功。

原程序使用 Unity Mono 脚本后端，业务代码仍以 .NET 中间语言保存在：

`simple_Data/Managed/Assembly-CSharp.dll`

因此可以恢复出结构清晰、可再次编译的 C# 源代码。已经完成以下工作：

- 从 `Assembly-CSharp.dll` 反编译出 60 个 C# 文件；
- 分离 TextMeshPro 官方示例与项目业务脚本；
- 筛选出 26 个与毕业设计直接相关的脚本；
- 使用 Demo 自带的 Unity 程序集进行重新编译验证；
- 编译结果为 **0 个错误、10 个警告**。

原程序集 SHA-256：

`5E7C4DE36EDEF80FF0C4E9FF5221C56A742C56AF7E2EB88A1A1EA0AF6A65A103`

## 目录说明

### `RecoveredUnityProject/Assets/Scripts`

这是整理后的项目业务代码，共 26 个脚本。重新创建 Unity 工程时，优先使用这里的文件。

### `DecompiledScripts`

这是程序集的完整反编译结果，包含：

- 项目业务代码；
- TextMeshPro 官方示例代码；
- 反编译器生成的项目文件；
- 编译验证输出。

该目录用于留档和交叉检查，不建议整体复制到新的 Unity 工程。

## 已恢复的项目代码

### 玩家与战斗

- `StateMachine/PlayerMovement.cs`：玩家移动、跳跃、方向翻转、三段连击和命中反馈；
- `PlayerHealth.cs`：玩家生命值、受击和击退；
- `AttackSense.cs`：命中暂停和镜头震动；
- `CameraShake.cs`：镜头震动参数；
- `Enemy.cs`：敌人受击、击退及对玩家造成伤害。

### 敌人有限状态机

- `FSM.cs`：状态注册、切换和目标检测；
- `IState.cs`：状态接口；
- `StateType.cs`：状态枚举；
- `Parameter.cs`：状态机共享参数；
- `IdleState.cs`：待机状态；
- `PatrolState.cs`：巡逻状态；
- `ChaseState.cs`：追击状态；
- `ReactState.cs`：发现玩家后的反应状态；
- `AttackState.cs`：攻击状态；
- `HitState.cs`：受击状态；
- `DeathState.cs`：死亡状态。

### 关卡、交互和 UI

- `CamreaFollow.cs`：摄像机跟随；
- `DoortoNextScene.cs`：关卡门与场景切换；
- `MainMenu.cs`：主菜单；
- `PauseMenu.cs`：暂停菜单；
- `InitButton.cs`：按钮初始化；
- `HealthBar.cs`：生命条显示；
- `ChatController.cs`：对话控制；
- `Sign.cs`：告示牌交互；
- `TreasureBox.cs`：宝箱交互；
- `Monsterset.cs`：怪物相关设置。

## 能恢复什么，不能恢复什么

### 已经恢复

- 类名、方法名和字段名；
- `public`、`private`、`SerializeField`、`Header` 等声明；
- 玩家移动、连击、受击、状态机、UI 和关卡交互逻辑；
- 协程和大部分控制流程；
- 原程序集所引用的 Unity API。

### 无法百分之百恢复

- 原始注释；
- 原始代码排版和文件夹结构；
- 局部变量在编译优化后丢失的原始名字；
- `.meta` 文件及原始 GUID；
- Inspector 中保存的具体数值和对象引用；
- Prefab、Animator Controller、Tilemap、材质和场景的原始编辑结构；
- Git 提交历史。

当前警告主要是 `SerializeField` 字段在纯 C# 编译过程中显示“未赋值”。这些字段原本由 Unity Inspector 在场景或 Prefab 中赋值，并不表示反编译失败。

## 如何重新建立 Unity 工程

1. 安装与原项目接近的 Unity 版本，优先使用 Unity `2021.3.11f1`。
2. 创建一个新的 2D 项目。
3. 将 `RecoveredUnityProject/Assets/Scripts` 复制到新工程的 `Assets/Scripts`。
4. 安装或启用 TextMeshPro、2D Sprite、Tilemap 等原项目使用的包。
5. 根据 Demo 画面重新创建场景、Prefab、Animator Controller 和输入设置。
6. 将脚本重新挂载到对应 GameObject，并在 Inspector 中补齐速度、生命值、LayerMask、攻击范围、Transform 和 UI 引用。
7. 每恢复一个模块就单独运行测试，不要一次性连接所有对象。

## 推荐恢复顺序

1. 玩家移动与跳跃；
2. 玩家动画和三段连击；
3. 生命值、受击和镜头反馈；
4. 敌人状态机；
5. 摄像机跟随；
6. 关卡切换、宝箱和告示牌；
7. 主菜单、暂停菜单和 UI；
8. 最后恢复场景资源与美术资源。

## 下一步建议

代码已经具备继续整理的条件。下一阶段可以从构建包中的 `.assets`、`.resS`、`level0`、`level1` 和 `level2` 文件尝试提取 Sprite、音频、材质、Prefab 信息和场景对象，再以恢复的 C# 脚本为基础重建一个可在 Unity 编辑器中打开的工程。

