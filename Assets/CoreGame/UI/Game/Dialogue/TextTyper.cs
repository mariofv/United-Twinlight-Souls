using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class TextTyper : MonoBehaviour
{
    public float typingSpeed;
    public UnityEvent endTypingEvent;

    private TextMeshProUGUI text;
    private bool typing = false;

    private string textString;
    private int currentCharacter = 0;
    private float currentTime = 0f;
    private int numCharacters = 0;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!typing)
        {
            return;
        }

        currentTime += Time.deltaTime;
        currentCharacter = Mathf.Min(Mathf.RoundToInt(currentTime * typingSpeed), numCharacters);

        string textColorString = ColorUtility.ToHtmlStringRGB(text.color);
        text.text = textString.Substring(0, currentCharacter) + "<color=#" + textColorString + "00>" + textString.Substring(currentCharacter, numCharacters - currentCharacter) + "</color>";

        if (currentCharacter == numCharacters)
        {
            EndTyping();
        }
    }

    public void StartTyping()
    {
        typing = true;
        textString = text.text;
        numCharacters = textString.Length;
        currentCharacter = 0;
        currentTime = 0f;
    }

    public void EndTyping()
    {
        typing = false;
        text.text = textString;
        endTypingEvent.Invoke();
    }

    public bool IsTyping()
    {
        return typing;
    }
}
