using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsGroup : MonoBehaviour
{
    [SerializeField] private SystemCursor settingsCursor;
    [SerializeField] private List<Setting> settings;

    private int currentIndex = 0;

    public void Open()
    {
        MoveCursor(0);
    }

    public void MoveCursorUp()
    {
        int nextSetting = currentIndex - 1;
        if (nextSetting == -1)
        {
            nextSetting = settings.Count - 1;
        }

        MoveCursor(nextSetting);
    }

    public void MoveCursorDown()
    {
        int nextSetting = (currentIndex + 1) % settings.Count;

        MoveCursor(nextSetting);
    }

    public void MoveCursor(int nextIndex)
    {
        currentIndex = nextIndex;
        settingsCursor.Move(settings[currentIndex].GetComponent<RectTransform>().position);
    }

    public void OnLeftPressed()
    {
        settings[currentIndex].DecreaseSetting();
    }

    public void OnRightPressed()
    {
        settings[currentIndex].IncreaseSetting();
    }
}
