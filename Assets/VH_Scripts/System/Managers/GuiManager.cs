using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SmallTown {
public class GuiManager : MonoBehaviour, IManager {

	[Header("Menu")]
	[SerializeField] private MenuManager menuManager;
	[SerializeField] private NewGameManager newGameManager;
	[SerializeField] private SettingsManager settingsManager;
	[SerializeField] private TutorialManager tutorialManager;
	[SerializeField] private CreditsManager creditsManager;

	[Header("Hud")]
	[SerializeField] private HudManager hudManager;
	[SerializeField] private JournalManager journalManager;

	// Public variables
	public MenuManager Menu { get { return menuManager; } }
	public NewGameManager NewGame { get { return newGameManager; } }
	public SettingsManager Settings { get { return settingsManager; } }
	public TutorialManager Tutorial { get { return tutorialManager; } }
	public CreditsManager Credits { get { return creditsManager; } }
	public HudManager Hud { get { return hudManager; } }
	public JournalManager Journal { get { return journalManager; } }
	public bool Initialized { get; private set; }


	// ************************************************************************************************************ CUSTOM FUNCTIONS

	public void Setup() {
		menuManager.Setup();
		newGameManager.Setup();
		settingsManager.Setup();
		tutorialManager.Setup();
		creditsManager.Setup();
		hudManager.Setup();
		journalManager.Setup();

		Initialized = true;
		Helpers.Initialized(GetType().Name);
	}
}
}