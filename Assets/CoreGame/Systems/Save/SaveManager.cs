using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private bool isUsingDebugSaves = false;
    private SaveData currentSaveData = null;

    private void Start()
    {
        isUsingDebugSaves = (Debug.isDebugBuild || Application.isEditor) && GameManager.instance.debugManager.isDebugSaveDataEnabled;
        if (!File.Exists(GetSaveFilePath()) || isUsingDebugSaves)
        {
            CreateNewSaveFile();
        }
        LoadGame();
    }

    public void SaveGame()
    {
        TransferSaveDataFromGame();
        WriteSaveData();
    }

    private void WriteSaveData()
    {
        string saveJson = JsonUtility.ToJson(currentSaveData, true);
        File.WriteAllText(GetSaveFilePath(), saveJson);
    }

    public void LoadGame()
    {
        ReadSaveData();
        TransferSaveDataToGame();
    }

    private void ReadSaveData()
    {
        string saveJson = File.ReadAllText(GetSaveFilePath());
        currentSaveData = JsonUtility.FromJson<SaveData>(saveJson);
    }

    private void CreateNewSaveFile()
    {
        if (isUsingDebugSaves)
        {
            currentSaveData = GameManager.instance.debugManager.debugSave.debugSaveData;
        }
        else
        {
            currentSaveData = new SaveData();
        }
        WriteSaveData();
    }

    private string GetSaveFilePath()
    {
        if (isUsingDebugSaves)
        {
            return Application.persistentDataPath + "/debug_save.data";
        }
        else
        {
            return Application.persistentDataPath + "/save.data";
        }
    }

    private void TransferSaveDataFromGame()
    {
        currentSaveData.maxLevelReached = GameManager.instance.progressionManager.GetMaxLevelUnlocked();
        currentSaveData.progression = GameManager.instance.progressionManager.GetProgression();
    }

    private void TransferSaveDataToGame()
    {
        GameManager.instance.progressionManager.SetMaxLevelUnlocked(currentSaveData.maxLevelReached);
        GameManager.instance.progressionManager.UnlockProgression((ProgressionManager.Progression)currentSaveData.progression);
    }
}
