using Random = UnityEngine.Random;
using BroadcastFlags = Broadcast.BroadcastFlags;
using System;
using HintServiceMeow.Core.Enum;
using HintServiceMeow.Core.Models.Hints;
using HintServiceMeow.Core.Utilities;
using InventorySystem.Items;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;
using LabApi.Features.Wrappers;
using LabApi.Loader.Features.Plugins;
using MEC;
using UnityEngine;
using Object = UnityEngine.Object;
using Logger = LabApi.Features.Console.Logger;

namespace RoundStartItems;

public class MainPlugin : Plugin<PluginConfig>
{
	private RoundStartEventHandler _eventHandler;

	public static MainPlugin Instance { get; private set; }

	public override string Name => "赛尔号纯净插件";

	public override string Author => "FoundationRX";

	public override Version Version => new Version(1, 0, 0);

	public override string Description => "";

	public override Version RequiredApiVersion { get; } = new Version(1, 1, 7);
	public override bool IsTransparent => true;


	public override void Enable()
	{
		Instance = this;
		Logger.Info((object)"========== 开局发卡插件正在启用 ==========");
		_eventHandler = new RoundStartEventHandler();
		CustomHandlersManager.RegisterEventsHandler<RoundStartEventHandler>(_eventHandler);
		Logger.Info((object)"========== 开局发卡插件启用完成 ==========");
	}

	public override void Disable()
	{
		Logger.Info((object)"========== 开局发卡插件正在禁用 ==========");
		if (_eventHandler != null)
		{
			CustomHandlersManager.UnregisterEventsHandler<RoundStartEventHandler>(_eventHandler);
		}
		Logger.Info((object)"========== 开局发卡插件禁用完成 ==========");
	}

	public static void OnRoundStarted()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			PluginConfig config = ((Plugin<PluginConfig>)Instance).Config;
			Logger.Info((object)$"[开局发卡] 回合开始，{config.DelaySeconds}秒后发放物品");
			Timing.CallDelayed(2f, (Action)delegate
			{
				try
				{
					AnnounceProbabilities();
				}
				catch (Exception ex3)
				{
					Logger.Error((object)("[开局发卡] 公示概率错误: " + ex3.Message));
				}
			});
			Timing.CallDelayed(config.DelaySeconds, (Action)delegate
			{
				try
				{
					DistributeItems();
				}
				catch (Exception ex2)
				{
					Logger.Error((object)("[开局发卡] 发放物品错误: " + ex2.Message));
				}
			});
		}
		catch (Exception ex)
		{
			Logger.Error((object)("[开局发卡] 回合开始处理错误: " + ex.Message));
		}
	}

	private static void AnnounceProbabilities()
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Invalid comparison between Unknown and I4
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Invalid comparison between Unknown and I4
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Invalid comparison between Unknown and I4
		try
		{
			PluginConfig config = ((Plugin<PluginConfig>)Instance).Config;
			string message = "<size=18><color=#FFD700>★ 逃生人员开局奖励 ★</color></size>\n" + $"<size=14><color=#00BFFF>卡片:</color> <color=white>清洁工</color><color=#00FF00>{config.JanitorCardChance}%</color> <color=white>科学家</color><color=#00FF00>{config.ScientistCardChance}%</color> <color=white>研究主管</color><color=#00FF00>{config.ResearchSupervisorCardChance}%</color></size>\n" + $"<size=14><color=#FF69B4>药品:</color> <color=white>止痛药</color><color=#00FF00>{config.PainkillersChance}%</color> <color=white>急救包</color><color=#00FF00>{config.MedkitChance}%</color> <color=white>肾上腺素</color><color=#00FF00>{config.AdrenalineChance}%</color></size>";
			foreach (Player item in Player.List)
			{
				try
				{
					if (item != null && !item.IsHost && ((int)item.Role == 1 || (int)item.Role == 6 || (int)item.Role == 15))
					{
						ShowHint(item, message, config.AnnounceDuration);
					}
				}
				catch
				{
				}
			}
			Logger.Info((object)"[开局发卡] 已公示概率");
		}
		catch (Exception ex)
		{
			Logger.Error((object)("[开局发卡] 公示概率错误: " + ex.Message));
		}
	}

	private static void DistributeItems()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Invalid comparison between Unknown and I4
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Invalid comparison between Unknown and I4
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Invalid comparison between Unknown and I4
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			PluginConfig config = ((Plugin<PluginConfig>)Instance).Config;
			int num = 0;
			foreach (Player item in Player.List)
			{
				try
				{
					if (item != null && !item.IsHost && ((int)item.Role == 1 || (int)item.Role == 6 || (int)item.Role == 15))
					{
						ItemType val = RollCard(config);
						if ((int)val != -1)
						{
							item.AddItem(val, (ItemAddReason)2);
						}
						ItemType val2 = RollItem(config);
						if ((int)val2 != -1)
						{
							item.AddItem(val2, (ItemAddReason)2);
						}
						num++;
						if (config.Debug)
						{
							Logger.Info((object)$"[开局发卡] {item.Nickname} 获得: {val}, {val2}");
						}
					}
				}
				catch (Exception ex)
				{
					Logger.Error((object)("[开局发卡] 给 " + ((item != null) ? item.Nickname : null) + " 发放错误: " + ex.Message));
				}
			}
			Logger.Info((object)$"[开局发卡] 已向 {num} 名玩家发放物品");
		}
		catch (Exception ex2)
		{
			Logger.Error((object)("[开局发卡] 发放物品错误: " + ex2.Message));
		}
	}

	private static ItemType RollCard(PluginConfig config)
	{
		float num = Random.Range(0f, 100f);
		if (!(num < config.JanitorCardChance))
		{
			num -= config.JanitorCardChance;
			if (!(num < config.ScientistCardChance))
			{
				num -= config.ScientistCardChance;
				if (!(num < config.ResearchSupervisorCardChance))
				{
					return (ItemType)(-1);
				}
				return (ItemType)2;
			}
			return (ItemType)1;
		}
		return (ItemType)0;
	}

	private static ItemType RollItem(PluginConfig config)
	{
		float num = Random.Range(0f, 100f);
		if (!(num < config.PainkillersChance))
		{
			num -= config.PainkillersChance;
			if (!(num < config.MedkitChance))
			{
				num -= config.MedkitChance;
				if (!(num < config.AdrenalineChance))
				{
					return (ItemType)(-1);
				}
				return (ItemType)33;
			}
			return (ItemType)14;
		}
		return (ItemType)34;
	}

	private static void ShowHint(Player player, string message, float duration)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			PlayerDisplay playerDisplay = PlayerDisplay.Get(player);
			if (playerDisplay == null)
			{
				player.SendBroadcast(message, (ushort)duration, (BroadcastFlags)0, false);
				return;
			}
			Hint hint = new Hint
			{
				Id = "RoundStartItems_Announce",
				Text = message,
				FontSize = 16,
				XCoordinate = -50f,
				YCoordinate = 1000f,
				Alignment = (HintAlignment)1,
				YCoordinateAlign = (HintVerticalAlign)0,
				Hide = false
			};
			playerDisplay.AddHint((AbstractHint)(object)hint);
			Timing.CallDelayed(duration, (Action)delegate
			{
				try
				{
					((AbstractHint)hint).Hide = true;
					playerDisplay.RemoveHint((AbstractHint)(object)hint);
				}
				catch
				{
				}
			});
		}
		catch
		{
			try
			{
				player.SendBroadcast(message, (ushort)duration, (BroadcastFlags)0, false);
			}
			catch
			{
			}
		}
	}
}
