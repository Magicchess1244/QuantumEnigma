using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class NewUI : MonoBehaviour
{
	[SerializeField] private UIDocument UI;
	[SerializeField] private Texture2D LockLogoTextures;
	private Background LockLogo;
    private float ShowTimer;
	private Label Label;
	private VisualElement Menu;
	private VisualElement LevelSelector;
	private VisualElement Settings;
	private VisualElement PauseButtons;
	private List<Button> BBack;
	private List<Button> BLevels;
	private GlobalManager GlobalManager;
	private bool Show = false;
	private void Start()
	{
        LockLogo = new Background(LockLogoTextures);
        UI = GetComponent<UIDocument>();
		GlobalManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GlobalManager>();
		if (SceneManager.GetActiveScene().buildIndex == 0)
		{
			MakeMenuWork();
		}
		else 
		{
			MakePauseWork();
		}

    }
	private VisualElement FindVisualElementUI(VisualElement Element, string Name) 
	{
		if (Element == null)
		{
			Element = UI.rootVisualElement;
		}
		return Element.Q(Name);
	}
	private List<Button> FindVisualElementsUI(VisualElement visualElement, string Name)
	{
		return visualElement.Query<Button>(Name).ToList();
	}
	private void Load(bool ShowMenu, bool ShowLevels, bool ShowSettigs)
	{
		Menu.visible = ShowMenu;
		LevelSelector.visible = ShowLevels;
		Settings.visible = ShowSettigs;
	}
	public void LoadLevel(int Level)
	{
		SceneManager.LoadScene(Level);
	}
	private void End()
	{
		Application.Quit();
	}
	private void MakeMenuWork()
	{
		Menu = FindVisualElementUI(null, "Menu");
		LevelSelector = FindVisualElementUI(null, "LevelSelect");
		Settings = FindVisualElementUI(null, "Settings");

		BBack = FindVisualElementsUI(UI.rootVisualElement, "Back");
		BLevels = FindVisualElementsUI(LevelSelector, "Level");

		Button storyMode = FindVisualElementUI(Menu, "StoryMode") as Button;
		Button BSettings = FindVisualElementUI(Menu, "BSettings") as Button;
		Button Exit = FindVisualElementUI(Menu, "Exit") as Button;

		foreach (Button bBack in BBack)
		{
			bBack.clicked += () => Load(true, false, false);
			;
		}
		foreach (Button bLevel in BLevels)
		{
			if (!LockButton(bLevel, bLevel.parent.IndexOf((VisualElement)bLevel) + 1))
			{
                bLevel.clicked += () => LoadLevel(bLevel.parent.IndexOf((VisualElement)bLevel) + 1);
            }
        }

		storyMode.clicked += () => Load(false, true, false);
		BSettings.clicked += () => Load(false, false, true);
		Exit.clicked += End;
	}
	private bool LockButton(Button button, int Num)
	{
		bool Lock = PlayerPrefs.GetInt("MaxLevel") + 1 < Num;
        if (Lock)
        {
			button.text = "";
			button.iconImage = LockLogo;
        }
        else
        {
            button.text = Num.ToString();
            button.iconImage = null;
        }
        return Lock;
    }
	private void MakePauseWork()
	{
		PauseButtons = FindVisualElementUI(null, "PauseButtons");
		Button Resume = FindVisualElementUI(PauseButtons, "Resume") as Button;
		Button Restart = FindVisualElementUI(PauseButtons, "Restart") as Button;
		Button BSettings = FindVisualElementUI(PauseButtons, "BSettings") as Button;
		Button Menu = FindVisualElementUI(PauseButtons, "Menu") as Button;

		Resume.clicked += () => ShowPause();
		Restart.clicked += () => LoadLevel(SceneManager.GetActiveScene().buildIndex);
		Menu.clicked += () => LoadLevel(0);
	}
	public void ShowPause()
	{
		GlobalManager.Playing = !GlobalManager.Playing;
		Show = !Show;
        FindVisualElementUI(null, "FullPause").visible = Show;
	}
}
