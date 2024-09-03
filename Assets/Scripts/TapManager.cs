using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TapManager : MonoBehaviour
{
    public GameObject[] tab;
    public Image[] tabBtnImage;
    public Sprite[] idleSprite, selectSprite;

    private void Start()
    {
        TabClick(0);
    }

    public void TabClick(int n)
    {
        for(int i = 0; i < tab.Length; i++)
        {
            tab[i].SetActive(i == n);
            tabBtnImage[i].sprite = i == n ? selectSprite[i] : idleSprite[i];
        }
    }
}
