using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class WindowConstructor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    //[SerializeField] List<Sprite> sprites = new List<Sprite>(); //0 = trap, 1 = tree, 2 = portal
    [SerializeField] Sprite trapSprite;
    [SerializeField] Sprite treeSprite;
    [SerializeField] Sprite teleportSprite;
    [SerializeField] GameObject imageGo;

    Transform imagesParent;

    [HideInInspector] public string levelName, json, trapsData;

    public Level level;

    public void CreateWindow(Level a_level)
    {
        level = a_level;

        Transform textsParent = transform.Find("Texts");

        TextMeshProUGUI maxTurnsTMP = textsParent.Find("Turns").Find("maxTurns").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI levelNameTMP = textsParent.Find("Level").Find("Level name").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI creationDateTMP = textsParent.Find("Creation Date").Find("creationDate").GetComponent<TextMeshProUGUI>();

        levelName = level.levelName;
        maxTurnsTMP.text = level.nbTurns.ToString();

        levelNameTMP.text = levelName;
        level.creatorName = a_level.creatorName;

        imagesParent = gameObject.transform.Find("Images").gameObject.transform;
        creationDateTMP.text = DateParsed(level.creationDate);
        json = JsonUtility.ToJson(level, true);

        Button button = GetComponentInChildren<Button>();

        if (GeneralManager.isComingFromDatabaseLevelsChoice) button.GetComponentInChildren<TextMeshProUGUI>().text = "Download";
        else if (GeneralManager.isComingFromLocalLevelsChoice) button.GetComponentInChildren<TextMeshProUGUI>().text = "Play";

        if (level.isInDarkMode) GetComponent<Image>().color = new Color(0, 0, 0, GetComponent<Image>().color.a);

        if ((level.creatorName != Environment.UserName) && GeneralManager.isComingFromDatabaseLevelsChoice) transform.GetChild(1).gameObject.SetActive(false);

        DisplayImages();
    }
    void DisplayImages()
    {
        string objectsContained = level.objectsContained;

        if(objectsContained != null)
        {

            if (objectsContained.Contains("trap"))
            {
                GameObject imageInstance = Instantiate(imageGo, imagesParent);
                imageInstance.GetComponent<Image>().sprite = trapSprite;
            }

            if (objectsContained.Contains("tree"))
            {
                GameObject imageInstance = Instantiate(imageGo, imagesParent);
                imageInstance.GetComponent<Image>().sprite = treeSprite;
            }

            if (objectsContained.Contains("teleport"))
            {
                GameObject imageInstance = Instantiate(imageGo, imagesParent);
                imageInstance.GetComponent<Image>().sprite = teleportSprite;
            }
        }
    }

    string DateParsed(string a_date)
    {
        string tempDate = a_date;

        string[] charsToRemove = new string[] { "?", "?"};

        tempDate = tempDate.Replace("T", "\nTime: ").Replace("-", "/");

        foreach (string c in charsToRemove) tempDate = tempDate.Replace(c, string.Empty);

        if(GeneralManager.isComingFromDatabaseLevelsChoice) tempDate = tempDate.Substring(0, tempDate.Length - 7);

        return tempDate;
    }

    public void OnPointerEnter(PointerEventData eventData) { if(level.description != null) DisplayLevelManager.descriptionTMP.text = level.description; }

    public void OnPointerExit(PointerEventData eventData) { if (DisplayLevelManager.descriptionTMP.text != "") DisplayLevelManager.descriptionTMP.text = ""; }
}
