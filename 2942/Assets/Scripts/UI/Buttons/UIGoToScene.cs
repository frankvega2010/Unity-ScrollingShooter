using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGoToScene : MonoBehaviour
{
    public string sceneName;
    public bool isLevelRestartButton;

    public void GoToThisScene()
    {
        switch (sceneName)
        {
            case "Level1":
                BGMMenu.Get().stopMenuMusic();
                LoaderManager.Get().LoadScene(sceneName);
                UILoadingScreen.Get().SetVisible(true);
                break;
            case "Level2":
                BGMMenu.Get().stopMenuMusic();
                LoaderManager.Get().LoadScene(sceneName);
                UILoadingScreen.Get().SetVisible(true);
                break;
            case "GameOver":
                SceneManager.LoadScene(sceneName);
                break;
            case "Menu":
                if (SceneManager.GetActiveScene().name == "GameOver")
                {
                    BGMMenu.Get().playMenuMusic();
                }
                SceneManager.LoadScene(sceneName);
                break;
            case "Controls":
                SceneManager.LoadScene(sceneName);
                break;
            case "Credits":
                SceneManager.LoadScene(sceneName);
                break;
            default:
                if (isLevelRestartButton)
                {
                    SceneManager.LoadScene(SaveLastLevel.Get().loadLevelName());
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }
                break;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
