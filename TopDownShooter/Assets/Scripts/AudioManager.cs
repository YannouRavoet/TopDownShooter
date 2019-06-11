using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager: MonoBehaviour {
    public enum AudioChannel { Master, Sfx, Music };

    public float masterVolume {
        get; private set;
    }
    public float sfxVolume {
        get; private set;
    }
    public float musicVolume {
        get; private set;
    }

    AudioSource sfx2dSource;
    AudioSource[] musicSources;
    int activeMusicSourceIndex;

    public static AudioManager instance;
    SoundLibrary library;
    Transform audioListener;
    Transform player;

    void Awake () {
        if (instance != null) {
            Destroy (gameObject);
        } else {
            instance = this;
            DontDestroyOnLoad (gameObject);
            library = GetComponent<SoundLibrary> ();
            musicSources = new AudioSource[2];
            for (int i = 0; i < musicSources.Length; i++) {
                GameObject newMusicSource = new GameObject ("Music source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource> ();
                newMusicSource.transform.parent = transform;
            }
            GameObject newSfx2DSource = new GameObject ("2D sfx source ");
            sfx2dSource = newSfx2DSource.AddComponent<AudioSource> ();
            newSfx2DSource.transform.parent = transform;

            audioListener = FindObjectOfType<AudioListener> ().transform;
            
            masterVolume = PlayerPrefs.GetFloat ("master vol", 1);
            musicVolume = PlayerPrefs.GetFloat ("music vol", 1);
            sfxVolume = PlayerPrefs.GetFloat ("sfx vol", 1);
        }
    }
    void Start () {
        if (FindObjectOfType<Player> () != null) {
            player = FindObjectOfType<Player> ().transform;
        }
    }


    void Update () {
        if (player != null) {
            audioListener.position = player.position;
        }
    }

    public void SetVolume (float volume, AudioChannel channel) {
        switch (channel) {
            case AudioChannel.Master:
                masterVolume = volume;
                break;
            case AudioChannel.Music:
                musicVolume = volume;
                break;
            case AudioChannel.Sfx:
                sfxVolume = volume;
                break;
        }
        musicSources[activeMusicSourceIndex].volume = musicVolume * masterVolume;

        PlayerPrefs.SetFloat ("master vol", masterVolume);
        PlayerPrefs.SetFloat ("sfx vol", sfxVolume);
        PlayerPrefs.SetFloat ("music vol", musicVolume);
        PlayerPrefs.Save ();
    }

    public void PlayMusic (AudioClip clip, float fadeDuration = 1) {
        activeMusicSourceIndex = 1 - activeMusicSourceIndex;
        musicSources[activeMusicSourceIndex].clip = clip;
        musicSources[activeMusicSourceIndex].Play ();
        StartCoroutine (MusicFadeIn (fadeDuration));

    }

    IEnumerator MusicFadeIn (float duration) {
        float percent = 0;

        while (percent < 1) {
            percent += Time.deltaTime * 1 / duration;
            musicSources[activeMusicSourceIndex].volume = Mathf.Lerp (0, musicVolume * masterVolume, percent);
            musicSources[1 - activeMusicSourceIndex].volume = Mathf.Lerp (musicVolume * masterVolume, 0, percent);
            yield return null;
        }
    }

    public void PlaySound (AudioClip clip, Vector3 pos) {
        if (clip != null) {
            AudioSource.PlayClipAtPoint (clip, pos, sfxVolume * masterVolume);
        }
    }

    public void PlaySound (string name, Vector3 pos) {
        PlaySound (library.GetClipFromName (name), pos);
    }

    public void PlaySound2D (string name) {
        sfx2dSource.PlayOneShot (library.GetClipFromName (name), sfxVolume * masterVolume);
    }
}
