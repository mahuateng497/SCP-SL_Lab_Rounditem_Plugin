namespace RoundStartItems;

public class PluginConfig
{
	public bool IsEnabled { get; set; } = true;


	public bool Debug { get; set; }

	public float DelaySeconds { get; set; } = 5f;


	public float AnnounceDuration { get; set; } = 20f;


	public float JanitorCardChance { get; set; } = 40f;


	public float ScientistCardChance { get; set; } = 50f;


	public float ResearchSupervisorCardChance { get; set; } = 10f;


	public float PainkillersChance { get; set; } = 40f;


	public float MedkitChance { get; set; } = 40f;


	public float AdrenalineChance { get; set; } = 20f;

}
