using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{

    public static SceneSwitcher instance;

    private Image fullWhiteImage;

    public float timeTransitionIn = 2f;

    public float timeTransitionOut = 2f;

    private float timerPrecision = 0.01f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.activeSceneChanged += OnChangedScene;
    }

    private void OnDisable()
    {
        SceneManager.activeSceneChanged -= OnChangedScene;
    }
    // Start is called before the first frame update
    void Start()
    {
        fullWhiteImage = GetComponentInChildren<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadMainScene()
    {
        StartCoroutine(FadeIn("water"));
    }

    public void LoadEndScene()
    {

    }

    private IEnumerator FadeIn(string nextScene)
    {
        // initialize alpha to 0
        modifyAlpha(0);

        float timer = 0;
        while(timer < timeTransitionIn)
        {
            float newFade = Mathf.InverseLerp(0, timeTransitionIn, timer);
            modifyAlpha(newFade);
            timer += timerPrecision;
            yield return new WaitForSeconds(timerPrecision);
        }

        StartCoroutine(FadeOut());
        ChangeScene(nextScene);

    }

    private IEnumerator FadeOut()
    {
        // initialize alpha to 0
        modifyAlpha(1);

        float timer = 0;
        while (timer < timeTransitionOut)
        {
            float newFade = Mathf.InverseLerp(0, timeTransitionOut, timer);
            newFade = 1 - newFade;
            modifyAlpha(newFade);
            timer += timerPrecision;
            yield return new WaitForSeconds(timerPrecision);
        }
    }

    private void modifyAlpha(float newAplpha)
    {
        Color tmp = fullWhiteImage.color;
        tmp.a = newAplpha;
        fullWhiteImage.color = tmp;
    }

    private void ChangeScene(string nextScene)
    {
        SceneManager.LoadScene(nextScene);
        Scene scene = SceneManager.GetSceneByName("nextScene");
        SceneManager.SetActiveScene(scene);

    }


    private void OnChangedScene(Scene current, Scene next)
    {
        GetComponentInChildren<Canvas>().worldCamera = Camera.main;
    }
}
