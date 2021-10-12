using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;

public class Dialogue : MonoBehaviour
{
    Vector3 setSize;
    public int index = 0;
    [TextArea(10, 15)]
    public string fullText;
    public int letterIndex;
    public Text text, name;
    public float textDelay;
    public Character character;
    public Image portrait;
    public int wordLength;
    public DialogueBox[] dialogueBoxes;
    int dialogueBoxesLength;

    private void Start()
    {
        name.text = character.name;
        portrait.sprite = character.portrait;

        dialogueBoxesLength = dialogueBoxes.Length;
    }
    void OnEnable()
    {
        setSize = transform.localScale;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.DOScale(setSize, 1f).SetEase(Ease.InOutBack).OnComplete(() => { HeadBob(); });
        //RaiseTheCounter(textDelay);

        text.text = dialogueBoxes[index].text;
        character = dialogueBoxes[index].character;
        fullText = dialogueBoxes[index].text;
    }

    void Update()
    {
        letterIndex++; //Needs to be reworked to count up based on real time, not framerate
        letterIndex = Mathf.Clamp(letterIndex, 0, fullText.Length);

        string displayText = fullText.Substring(0, letterIndex);
        text.text = displayText;
    }

    [ContextMenu("Increment")]
    public void Increment()
    {
        index++;
        letterIndex = 0;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.DOScale(setSize, 1f).SetEase(Ease.InOutBack).OnComplete(() => { HeadBob(); });
    }

    //Idk I'll figure it out later
    /*
        IEnumerator RaiseTheCounter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            letterIndex++;
            RaiseTheCounter(textDelay);
        }
    */
    void Disable()
    {
        transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 1f).OnComplete(() => { gameObject.SetActive(false); });
    }

    void HeadBob()
    {
        portrait.transform.DORotate(portrait.transform.rotation.eulerAngles + dialogueBoxes[index].rOffset1.eulerAngles, 1f).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => portrait.transform.DORotate(portrait.transform.rotation.eulerAngles + dialogueBoxes[index].rOffset2.eulerAngles, 1f).SetEase(dialogueBoxes[index].portraitEaseMode));
        portrait.transform.DOMove(portrait.transform.position + dialogueBoxes[index].pOffset1, 1f).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => portrait.transform.DOMove(portrait.transform.position + dialogueBoxes[index].pOffset2, 1f).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => { HeadBob(); }));
    }
}
