using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip woodOptionSelectFX, woodOptionEnterFX, optionSelectFX, optionEnterFX, happyCitizenFX, angryCitizenFX, fireFX, unfireFX, paperOpenFX, paperCloseFX, buyFX, addItemFX, timeEndingFX, timeUpFX;

    public void PlaySound(string sound)
    {
        AudioClip soundToPlay;

        switch (sound)
        {
            case "wood_option_select":
                soundToPlay = woodOptionSelectFX;
                break;
            case "wood_option_enter":
                soundToPlay = woodOptionEnterFX;
                break;
            case "option_select":
                soundToPlay = optionSelectFX;
                break;
            case "option_enter":
                soundToPlay = optionEnterFX;
                break;
            case "happy_citizen":
                soundToPlay = happyCitizenFX;
                break;
            case "angry_citizen":
                soundToPlay = angryCitizenFX;
                break;
            case "fire":
                soundToPlay = fireFX;
                break;
            case "unfire":
                soundToPlay = unfireFX;
                break;
            case "paper_open":
                soundToPlay = paperOpenFX;
                break;
            case "paper_close":
                soundToPlay = paperCloseFX;
                break;
            case "buy":
                soundToPlay = buyFX;
                break;
            case "add_item":
                soundToPlay = addItemFX;
                break;
            case "time_ending":
                soundToPlay = timeEndingFX;
                break;
            case "time_up":
                soundToPlay = timeUpFX;
                break;
            default:
                soundToPlay = null;
                break;
        }

        if(soundToPlay != null)
        {
            source.clip = soundToPlay;
            source.PlayScheduled(0);
        }

        
    }
}
