using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoText : MonoBehaviour
{
    private TMP_Text text;
    public Weapon weapon;

    void Start()
    {
        text = GetComponent<TMP_Text>();
        weapon.onShoot.AddListener(UpdateText);
        UpdateText();

    }

    public void UpdateText()
    {
        text.text = $"{weapon.clipAmmo}/{weapon.ammo}";
    }

}
