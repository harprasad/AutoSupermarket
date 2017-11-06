using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCodes : MonoBehaviour {

    public static Dictionary<string, Color> Colordictionary;
    static int maxObjectAvailable = 255;

    public static void FillColorCodes()
    {
        if (Colordictionary == null)
        {
            byte colorbyte = 1;
            Colordictionary = new Dictionary<string, Color>(maxObjectAvailable);
            for(int i = 1; i < maxObjectAvailable; i++)
            {
                Color randomcolor = new Color32(colorbyte, colorbyte, colorbyte,255);
                colorbyte++;
                //if black swap it to blue
                if (randomcolor.r == 0 && randomcolor.g == 0 && randomcolor.b == 0) {
                    randomcolor.b = 100;//make it blue
                }
                Colordictionary.Add(i.ToString() + " Scatter", randomcolor);
            }
        }
    }
}
