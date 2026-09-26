# RoundStartItems（开局发卡）V1.0.0 By 真乃美 Manami

SCP:SL（SCP 秘密实验室）服务器的 LabAPI 插件：回合开始后，为逃生方（D 级、科学家、研究主管）延迟发放门禁卡与药品，并在开局向玩家公示各物品发放概率。

## 功能

- 回合开始后延迟 `DelaySeconds` 秒自动发卡发药
- 开局 2 秒后向 D 级 / 科学家 / 研究主管公示本轮物品概率（HUD 提示）
- 物品与概率全部可在配置文件中调整
- 支持 HintServiceMeow 提示（无该插件时自动降级为广播）

## 发放规则（默认）

| 物品 | 概率 | 说明 |
|---|---|---|
| 清洁工卡 | 40% | 门禁卡 |
| 科学家卡 | 50% | 门禁卡 |
| 研究主管卡 | 10% | 门禁卡 |
| 止痛药 | 40% | 药品 |
| 急救包 | 40% | 药品 |
| 肾上腺素 | 20% | 药品 |

发放对象：D 级、科学家、研究主管（三种逃生方角色）。

## 配置文件（PluginConfig）

首次加载后自动生成 JSON 配置文件，字段如下：

| 字段 | 默认值 | 说明 |
|---|---|---|
| IsEnabled | true | 是否启用 |
| Debug | false | 调试日志（打印每个玩家获得物品） |
| DelaySeconds | 5 | 回合开始后延迟发放秒数 |
| AnnounceDuration | 20 | 概率公示 HUD 显示时长（秒） |
| JanitorCardChance | 40 | 清洁工卡概率 |
| ScientistCardChance | 50 | 科学家卡概率 |
| ResearchSupervisorCardChance | 10 | 研究主管卡概率 |
| PainkillersChance | 40 | 止痛药概率 |
| MedkitChance | 40 | 急救包概率 |
| AdrenalineChance | 20 | 肾上腺素概率 |

## 环境要求

- SCP:SL 服务器，已安装 LabAPI 加载器（`RequiredApiVersion` = 1.1.7）
- 可选依赖 HintServiceMeow（缺失时自动降级为 `SendBroadcast` 广播）
- 目标框架：.NET Framework 4.8（net48）

## 安装

将 `RoundStartItems.dll` 放入服务器插件目录后重启服务器：

- Windows（客户端开服）：`%APPDATA%\SCP Secret Laboratory\PluginAPI\plugins\<端口>\`
- Linux 专服：`~/.config/SCP Secret Laboratory/PluginAPI/plugins/<端口>/`

## 从源码编译

```
dotnet build -c Release src/RoundStartItems.csproj
```

- 需要 .NET SDK（自动还原 `Microsoft.NETFramework.ReferenceAssemblies`）
- 需要 `lib/` 下的游戏程序集引用（见下文依赖说明）
- 产物输出到 `bin/RoundStartItems.dll`

### 依赖 DLL 说明

`lib/` 目录需要以下程序集（本包不含游戏本体程序集，请从你自己的服务器 / 游戏安装目录的 `Managed` 文件夹获取）：

`LabApi.dll`、`Assembly-CSharp.dll`、`Assembly-CSharp-firstpass.dll`、`CommandSystem.Core.dll`、`Mirror.dll`、`HintServiceMeow.dll`、`UnityEngine.CoreModule.dll`、`0Harmony.dll`、`Newtonsoft.Json.dll`

## 署名

- 原插件作者：真奈美（插件元数据 Author）
- 本源码由原「赛尔号纯净插件（服务器已关闭）」DLL 反编译整理而成，公开分发请保留原作者署名并自行确认原作者许可。
