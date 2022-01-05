using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BootStrap : MonoBehaviour
{
    [SerializeField] private List<string> SceneAlwaysLoaded = new List<string>();
    [SerializeField] private List<string> LobbyScenes = new List<string>();
    [SerializeField] private List<string> Level1Scenes = new List<string>();
    [SerializeField] private List<string> WinScenes = new List<string>();


    private List<string> currentLoadedScenes = new List<string>();

    private int _sceneCount;

    private UnityEvent _onAllScenesLoaded = new UnityEvent();

    private void Awake()
    {
        foreach (string Scene in SceneAlwaysLoaded)
        {
            SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
        }

        foreach (string Scene in LobbyScenes)
        {
            SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
        }


    }

    public IEnumerator LoadLevelOne()
    {
        _onAllScenesLoaded.RemoveAllListeners();
        _sceneCount = Level1Scenes.Count + LobbyScenes.Count;

        StartCoroutine(FadeManager.Instance.FadeIn());

        yield return new WaitUntil(FadeManager.Instance.IsOpaque);


        foreach (string Scene in LobbyScenes)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(Scene);
            op.completed += (_) => _sceneCount--;
        }

        foreach (string Scene in Level1Scenes)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(Scene, LoadSceneMode.Additive);
            op.completed += (_) => _sceneCount--;
        }



        _onAllScenesLoaded.AddListener(OnLevel1ScenesLoaded);

        StartCoroutine(SceneLoadCheckCoroutine());
    }

    public IEnumerator ReloadLevelOne(GameData gData)
    {
        _onAllScenesLoaded.RemoveAllListeners();
        _sceneCount = Level1Scenes.Count + LobbyScenes.Count;

        StartCoroutine(FadeManager.Instance.FadeIn());

        yield return new WaitUntil(FadeManager.Instance.IsOpaque);


        foreach (string Scene in LobbyScenes)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(Scene);
            op.completed += (_) => _sceneCount--;
        }

        foreach (string Scene in Level1Scenes)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(Scene, LoadSceneMode.Additive);
            op.completed += (_) => _sceneCount--;
        }



        _onAllScenesLoaded.AddListener(() => { OnLevel1ScenesLoaded(gData); });

        StartCoroutine(SceneLoadCheckCoroutine());
    }
    public IEnumerator LoadWin()
    {
        _onAllScenesLoaded.RemoveAllListeners();
        _sceneCount = Level1Scenes.Count + WinScenes.Count;

        StartCoroutine(FadeManager.Instance.FadeIn());

        yield return new WaitUntil(FadeManager.Instance.IsOpaque);

        foreach (string Scene in Level1Scenes)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(Scene);
            op.completed += (_) => _sceneCount--;
        }

        foreach (string Scene in WinScenes)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(Scene, LoadSceneMode.Additive);
            op.completed += (_) => _sceneCount--;
        }



        _onAllScenesLoaded.AddListener(OnSceneLoadedDefault);

        StartCoroutine(SceneLoadCheckCoroutine());
    }


    public IEnumerator LoadLobby()
    {

        _onAllScenesLoaded.RemoveAllListeners();
        _sceneCount = WinScenes.Count + LobbyScenes.Count;

        StartCoroutine(FadeManager.Instance.FadeIn());

        yield return new WaitUntil(FadeManager.Instance.IsOpaque);

        foreach (string Scene in LobbyScenes)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(Scene, LoadSceneMode.Additive);
            op.completed += (_) => _sceneCount--;
        }

        foreach (string Scene in WinScenes)
        {
            AsyncOperation op = SceneManager.UnloadSceneAsync(Scene);
            op.completed += (_) => _sceneCount--;
        }

        _onAllScenesLoaded.AddListener(OnSceneLoadedDefault);

        StartCoroutine(SceneLoadCheckCoroutine());
    }

    private IEnumerator SceneLoadCheckCoroutine()
    {
        yield return new WaitUntil(() => _sceneCount == 0);
        _onAllScenesLoaded.Invoke();
    }

    private void OnLevel1ScenesLoaded()
    {
        GameManager.Instance.Level1Loaded();
        StartCoroutine(FadeManager.Instance.FadeOut());
    }

    private void OnLevel1ScenesLoaded(GameData gData)
    {
        GameManager.Instance.Level1Loaded(gData);
        StartCoroutine(FadeManager.Instance.FadeOut());
    }

    private void OnSceneLoadedDefault()
    {
        StartCoroutine(FadeManager.Instance.FadeOut());
    }
}
