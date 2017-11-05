using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCodes : MonoBehaviour {

    public static Dictionary<string, Color> Colordictionary;
    static int maxObjectAvailable = 1001;

    public static void FillColorCodes()
    {
        if (Colordictionary == null)
        {
            Colordictionary = new Dictionary<string, Color>(maxObjectAvailable);
            for(int i = 1; i < maxObjectAvailable; i++)
            {
                Color randomcolor = Random.ColorHSV();
                Colordictionary.Add(i.ToString() + " Scatter", randomcolor);
            }
        }
    }
}
