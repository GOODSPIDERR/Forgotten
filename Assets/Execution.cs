using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Execution : MonoBehaviour
{
    // Start is called before the first frame update
    MeshRenderer[] renderers;
    public Material black, white;
    public SkinnedMeshRenderer characterMat1, characterMat2;
    public GameObject ui;
    public PlayerAttack playerAttack;
    public Animator animator;
    AudioSource audioSource;
    void Start()
    {
        renderers = FindObjectsOfType(typeof(MeshRenderer)) as MeshRenderer[];
        Time.timeScale = 1f;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);
    }

    public void Execute()
    {
        foreach (MeshRenderer mr in renderers)
        {
            mr.material = black;
            characterMat1.material = white;
            characterMat2.material = white;
            ui.SetActive(false);
            animator.SetTrigger("Fade");
            audioSource.Play();
            DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.01f, 0.25f).SetEase(Ease.Linear).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        }
    }
}
