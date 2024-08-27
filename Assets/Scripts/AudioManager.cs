using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Clip
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public List<Clip> audioClips;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySE(string clipName)
    {
        Clip SEClip = audioClips.Find(c => c.name == clipName);
        if (SEClip != null && SEClip.clip != null)
        {
            audioSource.PlayOneShot(SEClip.clip);
        }
        else
        {
            Debug.LogWarning($"AudioClip with name {clipName} not found");
        }
    }
}