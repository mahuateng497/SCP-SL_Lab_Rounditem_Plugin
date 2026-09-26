using System;
using LabApi.Events.CustomHandlers;
using LabApi.Features.Console;

namespace RoundStartItems;

public class RoundStartEventHandler : CustomEventsHandler
{
	public override void OnServerRoundStarted()
	{
		try
		{
			Logger.Info((object)"[开局发卡] 回合开始");
			MainPlugin.OnRoundStarted();
		}
		catch (Exception ex)
		{
			Logger.Error((object)("[开局发卡] 回合开始事件错误: " + ex.Message));
		}
	}
}
