# Unity 2D 横版动作游戏：代码恢复与工程化整理

这是本科毕业设计《一款基于 Unity 的横版过关游戏的设计与实现》的代码展示仓库。原始 Unity 工程遗失后，我从自己保留的 Windows 构建包中恢复了 C# 业务脚本，并对核心逻辑进行了中文注释、缺陷修复和结构整理。

> 本仓库是“可审阅的代码作品集”，不是完整可运行的 Unity 工程。场景、贴图、动画、音频和第三方素材未上传，原因见[素材来源与匹配报告](Documentation/素材来源与匹配报告.md)。

## 项目展示内容

- 敌人有限状态机：待机、巡逻、警觉、追击、攻击、受击、死亡七种状态。
- 玩家控制：横向移动、跳跃、朝向、三段连击与攻击位移补偿。
- 战斗反馈：顿帧、摄像机震动、击退、生命值和死亡动画。
- 关卡交互：出口切场景、告示牌、宝箱、暂停菜单和 UI 焦点恢复。
- 恢复后的 28 个 C# 文件已通过 `netstandard2.1 / C# 9` 独立编译验证：0 个错误。

## 重点目录

```text
RecoveredUnityProject/Assets/Scripts/
├─ StateMachine/PlayerMovement.cs  # 移动、跳跃、连击、命中反馈
├─ FSM.cs                          # 状态注册、状态转换和受击入口
├─ *State.cs                       # 七种敌人状态
├─ StateGuards.cs                  # 状态共享的边界/空引用判定
├─ Enemy.cs / PlayerHealth.cs      # 双方受击、击退与生命流程
└─ UI 与交互脚本                   # 菜单、血条、告示牌、宝箱等
```

## 本次整理修复的典型问题

- 将 `health >= 0` 就销毁怪物的反向判断改为生命归零时销毁。
- 修复玩家 `GetHit` 只有在已经受击时才扣血、且没有保存击退方向的问题。
- 将敌人受击从“按攻击键让全部 FSM 受伤”改成由实际命中对象调用。
- FSM 进入初始状态前先初始化 Animator，并阻止同一帧连续执行多个转换。
- 死亡状态只安排一次延迟销毁，避免每帧重复调用。
- 摄像机跟随时保留 Z 坐标，避免摄像机逐渐移动到角色平面。
- 用类型判断替代脆弱的 `GetType().ToString()` 字符串比较。
- 为数组、目标、Animator、摄像机和 UI 引用增加空值与边界保护。

## 恢复与验证

恢复源为本人持有的 Unity 2021.3.11f1c2 Mono 构建。原 `Assembly-CSharp.dll` 的 SHA-256：

```text
5E7C4DE36EDEF80FF0C4E9FF5221C56A742C56AF7E2EB88A1A1EA0AF6A65A103
```

脚本先反编译恢复，再在不依赖完整 Unity 编辑器工程的条件下引用构建包内 Unity 程序集进行编译检查。详细过程见[代码恢复说明](README_代码恢复说明.md)。

## 来源声明

早期 Demo 参考了 [RedFF0000/Finite-state-machine](https://github.com/RedFF0000/Finite-state-machine) 的动画素材和 FSM 教学实现。本仓库已经明确标注来源，并对 FSM 代码重新组织和改写；没有把参考仓库的 UnityPackage 或第三方美术文件重新上传。代码归属边界见[代码来源与改写说明](Documentation/代码来源与改写说明.md)。

## 展示

<img width="1471" height="748" alt="image" src="https://github.com/user-attachments/assets/c78df21d-b24a-4e07-8f46-fec962b7bb14" />
<img width="1471" height="748" alt="image" src="https://github.com/user-attachments/assets/d984ec8f-79b7-409a-b188-4593825e6c34" />



## 许可

本仓库未授予第三方复制、修改或再分发许可。第三方素材分别受其原作者条款约束。

