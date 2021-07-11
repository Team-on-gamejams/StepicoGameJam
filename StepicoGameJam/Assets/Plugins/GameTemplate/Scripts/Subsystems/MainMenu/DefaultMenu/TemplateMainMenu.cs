using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateMainMenu : MenuBase {
	public static int sceneIdToLoad = 1;

	public void Play() {
		SceneLoader.Instance.LoadScene(TemplateMainMenu.sceneIdToLoad, true, true);
	}

	public void Load() {
		SceneLoader.Instance.LoadScene(TemplateMainMenu.sceneIdToLoad, false, false);
	}
}
