using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomUtils : MonoBehaviour {
	public static void TakeScreenShots(int counter,GameObject[] Stuffs,bool Labled,bool classSpecific)
    {
        if (!Labled)
        {
            ScreenCapture.CaptureScreenshot("ScreenShots/JPEGImages/" + "Screenshot" + counter.ToString() + ".png", 3);
        }
        else
        {
            byte colorindex = 10;
            foreach (GameObject stuff in Stuffs)
            {
                if (stuff.GetComponent<Renderer>() == null)
                {
                    //This is for the downloaded models which contains multiple meshes in children
                    Renderer[] renderers = stuff.GetComponentsInChildren<Renderer>();
                    foreach (Renderer rend in renderers)
                    {
                        rend.material.mainTexture = null;
                        if (classSpecific)
                        {
                            foreach (Material mat in rend.materials)
                            {
                                mat.shader = Shader.Find("Unlit/Color");
                                mat.color = ColorCodes.Colordictionary[stuff.name];
                            }
                        }
                        else
                        {
                            foreach (Material mat in rend.materials)
                            {
                                mat.shader = Shader.Find("Unlit/Color");
                                mat.color = new Color32(colorindex, colorindex, colorindex, 255);
                            }
                        }
                    }
                    colorindex++;
                    if (classSpecific)
                    {
                        ScreenCapture.CaptureScreenshot("ScreenShots/cls/" + "Screenshot" + counter.ToString() + ".png", 3);
                    }
                    else
                    {
                        ScreenCapture.CaptureScreenshot("ScreenShots/inst/" + "Screenshot" + counter.ToString() + ".png", 3);
                    }

                }

            }
        }
    }

	public static void SetCullingMask(bool allblack)
    {
        if (!allblack)
        {
            Camera.main.cullingMask = (1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Objects"));
        }
        else
        {
            Camera.main.cullingMask = (1 << LayerMask.NameToLayer("Objects"));
        }
    }
}
