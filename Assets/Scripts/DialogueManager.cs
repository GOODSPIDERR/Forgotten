using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.InputSystem;


//Fair warning, this entire script is a mess. Turns out, it's really difficult to make intuitive dialogue boxes that do exactly what you want them to do. 
[RequireComponent(typeof(Text))]
public class DialogueManager : MonoBehaviour
{
    private Vector3 _setSize;
    public int index = 0;
    private string _fullText;
    public int letterIndex;
    private readonly Text _text;
    private readonly Text _charName;
    private Character _character;
    private readonly Image _portrait;
    public int wordLength;
    public DialogueBox[] dialogueBoxes;
    private int _dialogueBoxesLength;
    private PlayerInput _playerInput;
    private PlayerInputActions _playerInputActions;
    public bool activated = false;
    private float _timer;
    public float delayAmount = 0.1f;
    private Vector3 _pOffset1, _pOffset2;
    private Quaternion _rOffset1, _rOffset2;
    private float _tweenTime = 1f;

    public DialogueManager(Text text, Text charName, Image portrait, Quaternion rOffset1, Quaternion rOffset2)
    {
        _text = text;
        _charName = charName;
        this._portrait = portrait;
        this._rOffset1 = rOffset1;
        this._rOffset2 = rOffset2;
    }

    private void Start()
    {
        //Input system
        _playerInput = GetComponent<PlayerInput>();
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();

        _dialogueBoxesLength = dialogueBoxes.Length;
    }
    void OnEnable()
    {
        var transform1 = transform;
        _setSize = transform1.localScale;
        transform1.localScale = new Vector3(0.01f, 0.01f, 0.01f);

        transform.DOScale(_setSize, 1f).SetEase(Ease.InOutBack).OnComplete(() => { ReadyToRoll(); });
        //index++;
    }

    private void Update()
    {
        if (!activated) return;
        _timer += Time.deltaTime;

        if (_timer >= delayAmount)
        {
            _timer -= delayAmount;
            letterIndex++;
            letterIndex = Mathf.Clamp(letterIndex, 0, _fullText.Length);
        }

        var displayText = _fullText.Substring(0, letterIndex);
        _text.text = displayText;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Increment();
        }
    }

    [ContextMenu("Increment")]
    public void Increment()
    {
        index++;
        letterIndex = 0;
        _timer = 0f;
        activated = false;
        SetParameters();

        transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        transform.DOScale(_setSize, 1f).SetEase(Ease.InOutBack).OnComplete(() => { ReadyToRoll(); });
    }

    private void Disable()
    {
        transform.DOScale(new Vector3(0.01f, 0.01f, 0.01f), 1f).OnComplete(() => { gameObject.SetActive(false); });
    }

    private void HeadBob()
    {
        //portrait.transform.DORotateQuaternion(rOffset1, tweenTime).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => portrait.transform.DORotateQuaternion(rOffset2, tweenTime).SetEase(dialogueBoxes[index].portraitEaseMode));
        _portrait.transform.DOMove(_pOffset1, _tweenTime).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => _portrait.transform.DOMove(_pOffset2, _tweenTime).SetEase(dialogueBoxes[index].portraitEaseMode).OnComplete(() => { HeadBob(); }));
    }

    private void ReadyToRoll()
    {
        SetParameters();

        HeadBob();
        activated = true;
    }

    private void SetParameters()
    {
        _text.text = dialogueBoxes[index].text;
        _character = dialogueBoxes[index].character;
        _fullText = dialogueBoxes[index].text;
        _tweenTime = dialogueBoxes[index].tweenTime;

        _charName.text = _character.name;
        _portrait.sprite = _character.portrait;

        var transform1 = _portrait.transform;
        var position = transform1.position;
        _pOffset1 = position + dialogueBoxes[index].pOffset1;
        _pOffset2 = position + dialogueBoxes[index].pOffset2;

        Debug.Log(index);

        //Fuck this
        /*
        rOffset1 = portrait.transform.rotation +
        rOffset2 = portrait.transform.rotation + Quaternion.EulerAngles(dialogueBoxes[index].rOffset2.x, dialogueBoxes[index].rOffset2.y, dialogueBoxes[index].rOffset2.z));
        */
    }
}
