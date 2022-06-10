using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSettingsTab : MonoBehaviour
{
    [SerializeField] private SystemCursor audioCursor;
    [SerializeField] private List<AudioSetting> audioSettings;

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
            nextSetting = audioSettings.Count - 1;
        }

        MoveCursor(nextSetting);
    }

    public void MoveCursorDown()
    {
        int nextSetting = (currentIndex + 1) % audioSettings.Count;

        MoveCursor(nextSetting);
    }

    public void MoveCursor(int nextIndex)
    {
        currentIndex = nextIndex;
        audioCursor.Move(audioSettings[currentIndex].GetComponent<RectTransform>().position);
    }

    public void OnLeftPressed()
    {
        audioSettings[currentIndex].DecreaseSetting();
    }

    public void OnRightPressed()
    {
        audioSettings[currentIndex].IncreaseSetting();
    }
}
