using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MagazineAmmo : MonoBehaviour
{
   [SerializeField] private int magazineAmmo;
   [SerializeField] private GameObject magCanvas;
   [SerializeField] private TextMeshProUGUI ammoTxt;
   
   public int MagAmmo
   {
      get => magazineAmmo;
      set => magazineAmmo = value;
   }

   private void Start()
   {
      UpdateMagUI();
      EnableUI(true);
   }

   public void UpdateMagUI()
   {
      if (MagAmmo <= 10 && MagAmmo > 5)
      {
         ammoTxt.color = Color.yellow;
      }
      else if (MagAmmo <= 5)
      {
         ammoTxt.color = Color.red;
      }
      else
      {
         ammoTxt.color = Color.green;
      }
      
      ammoTxt.text = MagAmmo.ToString();
   }

   public void EnableUI(bool value)
   {
      magCanvas.SetActive(value);
   }
}
