using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Clip
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public List<Clip> SEClips;
    public List<Clip> BGMClips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
    }

    public static AudioManager instance;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "TitleScene":
                PlayBGM("Title");
                break;
            case "MapScene":
                PlayBGM("StageSelect");
                break;
            case "Stage1":
                PlayBGM("Stage1");
                break;
            case "Stage2":
                PlayBGM("Stage2");
                break;
            case "Stage3":
                PlayBGM("Stage3");
                break;
            case "Stage4":
                PlayBGM("Stage4");
                break;
            case "TestScene":
                PlayBGM("Stage1");
                break;
            default:
                break;
        }
    }

    public void PlaySE(string clipName)
    {
        Clip SEClip = SEClips.Find(c => c.name == clipName);
        if (SEClip != null && SEClip.clip != null)
        {
            audioSource.PlayOneShot(SEClip.clip);
        }
        else
        {
            Debug.LogWarning($"AudioClip with name {clipName} not found");
        }
    }
    public void PlayBGM(string clipName)
    {
        audioSource.Stop();
        Clip BGMClip = BGMClips.Find(c => c.name == clipName);
        if (BGMClip != null && BGMClip.clip != null)
        {
            audioSource.clip = BGMClip.clip;
            audioSource.Play();
        }
    }
}