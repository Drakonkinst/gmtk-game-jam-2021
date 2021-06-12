using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public static HUD instance;

    public SmoothUIBar healthBarDisplay;
    public SmoothUIBar manaBarDisplay;
    public Image[] spellIcons;

    private Spell[] spells;
    private int numSpells = 0;

    private void Awake()
    {
        instance = this;

        // Correct the spell objects
        for(int i = 0; i < spellIcons.Length; ++i)
        {
            if(spellIcons[i].transform.Find("Spell") != null)
            {
                spellIcons[i] = spellIcons[i].transform.Find("Spell").GetComponent<Image>();
            }
        }
    }

    private void Update()
    {
        for (int i = 0; i < numSpells; ++i)
        {
            Spell spell = spells[i];
            Image icon = spellIcons[i];

            if(spells[i] == null || !spells[i].isUnlocked)
            {
                continue;
            }

            if(!spell.OnCooldown())
            {
                icon.fillAmount = 1;
            }
            else
            {
                icon.fillAmount -= 1 / spell.cooldown * Time.deltaTime;
                if(icon.fillAmount <= 0)
                {
                    icon.fillAmount = 0;
                }
            }
            //Debug.Log(icon.fillAmount);
        }
    }

    // Call this whenever a new spell is unlocked
    public void UpdateSpellsUnlocked()
    {
        for(int i = 0; i < numSpells; ++i)
        {
            bool isUnlocked = spells[i] != null && spells[i].isUnlocked;
            spellIcons[i].transform.parent.gameObject.SetActive(isUnlocked);
        }
    }

    public void SetSpells(Spell[] spells)
    {
        this.spells = spells;
        numSpells = Mathf.Min(spellIcons.Length, spellIcons.Length);
    }
}
