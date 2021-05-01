using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private Animator menuMusic;
    [SerializeField] private Animator happyMusic;
    [SerializeField] private Animator sadMusic;

    string currentMusic = "menu";
    string musicBeforeMenu;

    private void Start()
    {
        FadeIn(currentMusic);
    }

    public void SetMusic(string music)
    {
        if(currentMusic != music)
        {
            FadeOut(currentMusic);
            currentMusic = music;
            FadeIn(currentMusic);
        }
    }

    public void SaveCurrentMusic()
    {
        musicBeforeMenu = currentMusic;
    }

    public void DisableMusic(bool b)
    {
        menuMusic.gameObject.SetActive(b);
        happyMusic.gameObject.SetActive(b);
        sadMusic.gameObject.SetActive(b);
    }


    public void RestoreMusic()
    {
        FadeOut(currentMusic);
        currentMusic = musicBeforeMenu;
        FadeIn(currentMusic);
    }

    private void FadeOut(string music)
    {
        switch (music)
        {
            case "menu":
                menuMusic.SetTrigger("FadeOut");
                break;
            case "happy":
                happyMusic.SetTrigger("FadeOut");
                break;
            case "sad":
                sadMusic.SetTrigger("FadeOut");
                break;
            default:
                break;
        }
    }

    private void FadeIn(string music)
    {
        switch (music)
        {
            case "menu":
                menuMusic.gameObject.GetComponent<AudioSource>().PlayScheduled(0);
                menuMusic.SetTrigger("FadeIn");
                break;
            case "happy":
                happyMusic.gameObject.GetComponent<AudioSource>().PlayScheduled(0);
                happyMusic.SetTrigger("FadeIn");
                break;
            case "sad":
                sadMusic.gameObject.GetComponent<AudioSource>().PlayScheduled(0);
                sadMusic.SetTrigger("FadeIn");
                break;
            default:
                break;
        }
    }
}
