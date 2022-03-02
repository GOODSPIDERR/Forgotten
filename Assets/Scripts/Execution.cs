using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Execution : MonoBehaviour
{
    private MeshRenderer[] _renderers;
    public Material black, white;
    public SkinnedMeshRenderer characterMat1, characterMat2;
    public GameObject ui;
    public PlayerAttack playerAttack;
    public Animator animator;
    private AudioSource _audioSource;
    private static readonly int Fade = Animator.StringToHash("Fade");

    private void Start()
    {
        _renderers = FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
        Time.timeScale = 1f;
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //Debug.Log(Time.timeScale);
    }

    public void Execute()
    {
        foreach (var mr in _renderers)
        {
            mr.material = black;
            characterMat1.material = white;
            characterMat2.material = white;
            ui.SetActive(false);
            animator.SetTrigger(Fade);
            _audioSource.Play();
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.01f, 0.25f).SetEase(Ease.Linear).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        }
    }
}
