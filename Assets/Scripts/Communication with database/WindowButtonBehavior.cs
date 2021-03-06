using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowButtonBehavior : MonoBehaviour
{
    [SerializeField] GameObject confirmationPanel;
    public string levelName;
    WindowConstructor windowConstructor;
    private void Start()
    {
        windowConstructor = GetComponent<WindowConstructor>();
    }
    public void ClickOnWindowButton()
    {
        if (GeneralManager.isComingFromDatabaseLevelsChoice) DownloadJson();
        else if (GeneralManager.isComingFromLocalLevelsChoice) PlayLevel();
    }

    void PlayLevel()
    {
        GeneralManager.sceneNameToLoad = windowConstructor.levelName;
        GeneralManager.levelToCreateChosenFromDisplayLevelsScene = windowConstructor.level;

        SceneManager.LoadScene("Niveau");
    }

    public void LoadLevel()
    {
        GeneralManager.SetIsInBuildModeToTrue();
        GeneralManager.sceneNameToLoad = windowConstructor.levelName;

        SceneManager.LoadScene("Niveau");
    }

    void DownloadJson()
    {
        string json = windowConstructor.json;
        Debug.Log(json);

        string directory = Application.persistentDataPath + SaveLoadLevelData.directoryDownloadedLevels;

        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

        Level level = JsonUtility.FromJson<Level>(json);

        levelName = level.levelName;
        json = JsonUtility.ToJson(level, true);

        File.WriteAllText(directory + levelName + ".txt", json);

        GameObject canvas = GameObject.Find("Canvas");

        Instantiate(confirmationPanel, canvas.transform);
    }
}
