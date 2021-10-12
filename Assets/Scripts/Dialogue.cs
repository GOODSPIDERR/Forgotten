using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;


//Fair warning, this entire script is a mess. Turns out, it's really difficult to make intuitive dialogue boxes that do exactly what you want them to do. 
public class Dialogue : MonoBehaviour
{
    Vector3 setSize;
    public int index = 0;
    [TextArea(10, 15)]
    string fullText;
    public int letterIndex;
    public Text text, name;
    public float textDelay;
    Character character;
    public Image portrait;
    public int wordLength;
    public DialogueBox[] dialogueBoxes;
    int dialogueBoxesLength;
    PlayerInput playerInput;
    PlayerInputActions playerInputActions;
    public bool activated = false;
    float timer;
    public float delayAmount = 0.1f;
    Vector3 pOffset1, pOffset2;
    Quaternion rOffset1, rOffset2;
    float tweenTime = 1f;

    private void Start()
    {


        //Input system
        playerInput = GetComponent<PlayerInput>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        dialogueBoxesLength = dialogueBoxes.Length;


    }
    void OnEnable()
    {
        setSize = transform.localScale;
        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);


        transform.DOScale(setSize, 1f).SetEase(Ease.InOutBack).OnComplete(() => { ReadyToRoll(); });
        //index++;
        //RaiseTheCounter(textDelay);
    }

    void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;

            if (timer >= delayAmount)
            {
                timer -= delayAmount;
                letterIndex++;
                letterIndex = Mathf.Clamp(letterIndex, 0, fullText.Length);
            }

            string displayText = fullText.Substring(0, letterIndex);
            text.text = displayText;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Increment();
            }
        }


    }

    [ContextMenu("Increment")]
    public void Increment()
    {
        index++;
        letterIndex = 0;
        timer = 0f;
        activated = false;
        text.text = dialogueBoxes[index].text;
        character = dialogueBoxes[index].character;
        fullText = dialogueBoxes[index].text;
        tweenTime = dialogueBoxes[index].tweenTime;
        name.text = character.name;
        portrait.sprite = character.portrait;
        pOffset1 = portrait.transform.position + dialogueBoxes[index].pOffset1;
        pOffset2 = portrait.transform.position + dialogueBoxes[index].pOffset2;

        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.DOScale(setSize, 1f).SetEase(Ease.InOutBack).OnComplete(() => { ReadyToRoll(); });
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
        portrait.transform.DORotateQuaternion(rOffset1, tweenTime).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => portrait.transform.DORotateQuaternion(rOffset2, tweenTime).SetEase(dialogueBoxes[index].portraitEaseMode));
        portrait.transform.DOMove(pOffset1, tweenTime).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => portrait.transform.DOMove(pOffset2, tweenTime).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => { HeadBob(); }));
    }

    void ReadyToRoll()
    {
        text.text = dialogueBoxes[index].text;
        character = dialogueBoxes[index].character;
        fullText = dialogueBoxes[index].text;
        tweenTime = dialogueBoxes[index].tweenTime;

        name.text = character.name;
        portrait.sprite = character.portrait;

        pOffset1 = portrait.transform.position + dialogueBoxes[index].pOffset1;
        pOffset2 = portrait.transform.position + dialogueBoxes[index].pOffset2;

        //Fuck this
        /*
        rOffset1 = portrait.transform.rotation +
        rOffset2 = portrait.transform.rotation + Quaternion.EulerAngles(dialogueBoxes[index].rOffset2.x, dialogueBoxes[index].rOffset2.y, dialogueBoxes[index].rOffset2.z));
        */

        HeadBob();
        activated = true;
    }
}
