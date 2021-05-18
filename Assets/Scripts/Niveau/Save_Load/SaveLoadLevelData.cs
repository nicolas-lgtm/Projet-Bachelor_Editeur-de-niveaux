﻿using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;


public class SaveLoadLevelData : MonoBehaviour
{
    public static string directoryStoryModeLevels = "/Mode Histoire/";
    public static string directoryDownloadedLevels = "/LevelsSaved/";

    public static Level levelToSave = new Level();
    public static string levelToSave_json;

    public static string levelToLoadName;

    public static GameManager _GM;

    public void RecordLevelData()
    {
        //Ajout des informations des cases à 'LevelData' et ajout de l'information concernant l'index
        string creatorName = Environment.UserName;
        levelToSave.creatorName = creatorName;

        levelToSave.nbTurns = GameManager.nbOfMovesLimit;

        levelToSave.isInDarkMode = GameManager.level.isInDarkMode;
        levelToSave.h = GameManager.level.h;
        levelToSave.w = GameManager.level.w;

        GameObject boxes = null;

        if (GameObject.Find("Squares")) boxes = GameObject.Find("Squares");
        else return;

        int boxesNb = boxes.transform.childCount;

        levelToSave.boxes.Clear();

        for (int i = 0; i < boxesNb; i++)
        {
            GameObject box = boxes.transform.GetChild(i).gameObject;
            LevelBoardBox boxDatas = box.GetComponent<BoxDatas>().box;

            boxDatas.index = i;

            if (box.transform.childCount == 1 && box.transform.GetChild(0).name == "Player") boxDatas.type = LevelBoardBoxType.Player;
            else if (box.transform.childCount == 1 && box.transform.GetChild(0).name == "Witch") boxDatas.type = LevelBoardBoxType.Witch;

            levelToSave.boxes.Add(boxDatas);
        }

        levelToSave.creationDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        GiveValueToObjectContained();
        levelToSave_json = JsonUtility.ToJson(levelToSave, true);

    }

    public void SaveLevelNameToLevelStructure()
    {
        string _levelName = GameObject.Find("LevelName_InputField").GetComponent<InputField>().text;

        if (_levelName != null && _levelName != "") levelToSave.levelName = _levelName;
    }

    void GiveValueToObjectContained()
    {
        levelToSave.objectsContained = "";

        bool trapAlreadyAdded = false;
        bool treeAlreadyAdded = false;
        bool teleportAlreadyAdded = false;

        for (int i = 0; i < levelToSave.boxes.Count; i++)
        {
            if ((levelToSave.boxes[i].type == LevelBoardBoxType.Trap || levelToSave.boxes[i].type == LevelBoardBoxType.DarkTrap) && !trapAlreadyAdded)
            {
                levelToSave.objectsContained += "trap ";
                trapAlreadyAdded = true;
            }

            if (levelToSave.boxes[i].type == LevelBoardBoxType.Tree && !treeAlreadyAdded)
            {
                levelToSave.objectsContained += "tree ";
                treeAlreadyAdded = true;
            }

            if (levelToSave.boxes[i].type == LevelBoardBoxType.Teleport_IN && !teleportAlreadyAdded)
            {
                levelToSave.objectsContained += "teleport ";
                teleportAlreadyAdded = true;
            }

            if (trapAlreadyAdded && treeAlreadyAdded && teleportAlreadyAdded) break;
        }

        if (levelToSave.objectsContained == "") levelToSave.objectsContained = "empty";
        levelToSave.objectsContained += "^^";
    }

    public static void LoadFromSavedLevelsDirectory(string a_levelNameToLoad)
    {
        _GM = GameObject.Find("Game Manager").GetComponent<GameManager>();

        string directory = "";

        if (GeneralManager.isInStoryMode) directory = directoryStoryModeLevels;
        else if (GeneralManager.isComingFromLocalLevelsChoice/* || GeneralManager.isComingFromDatabaseLevelsChoice*/|| GeneralManager.isInBuildMode) directory = directoryDownloadedLevels;

        string fullPath = Application.persistentDataPath + directory + a_levelNameToLoad + ".txt";

        //Debug.Log(fullPath);

        if (File.Exists(fullPath))
        {
            string json = File.ReadAllText(fullPath);
            LoadLevelFromJson(json);

            return;
        }

        //Affiche un message si le niveau n'existe pas
        //DisplayAlertMessages.DisplayMessage("Ce niveau n'a pas été trouvé.");
    }

    //Pour charger
    public static void LoadLevelFromJson(string level_json)
    {
        //Debug.Log(level_json);
        GameManager.level = JsonUtility.FromJson<Level>(level_json);
        _GM.CreateLevel(GameManager.level);
    }
}