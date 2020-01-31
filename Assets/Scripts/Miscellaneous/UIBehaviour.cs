﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBehaviour : MonoBehaviour
{
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text currentClipText;
    [SerializeField] private TMP_Text remainingClipsText;
    [SerializeField] private Slider oxygenSlider;
    [SerializeField] private TMP_Text oxygenPercentage;

    /// <summary>
    /// Called by multiple scripts to update the player's UI.
    /// The player's UI consists of: health, current ammo and clip,
    /// and oxygen.
    /// </summary>
    /// <param name="attackScript"></param>
    public void UpdateUI(PlayerAttack attackScript = null)
    {
        remainingClipsText.text = "Remaining clips: " + attackScript.currentWeapon.magsLeft.ToString();
        currentClipText.text = "Current clip: " + attackScript.currentWeapon.bulletsInCurrentMag.ToString() + "/" + attackScript.currentWeapon.magSize.ToString();
        healthText.text = "Health: " + attackScript.healthScript.currentHealth.ToString();
        oxygenSlider.value = attackScript.oxygenAmountSeconds / attackScript.maxOxygenAmountSeconds * 100; // Gets the percentage
        oxygenPercentage.text = Mathf.Floor(oxygenSlider.value).ToString();
    }
}
