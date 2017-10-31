using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFiller : MonoBehaviour {

    public Transform bin1ButtomLeft;
    public Transform bin2ButtomLeft;
    public Transform bin3ButtomLeft;
    public Transform bin4ButtomLeft;
    public Transform bin5ButtomLeft;

    public float maxstuffineachBin;

    public float startZ;
    public float maxZ;
    public float Zsteps;
    public float Xsteps;
    public GameObject LargeBox;
    public GameObject SmallBox;
    public GameObject Coke;

    int[] Bin1Probability = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 3 };  //1 for large box 2 for small box 3 for coke
    int[] Bin2Probability = { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 3, 3 };  //1 for large box 2 for small box 3 for coke

    int[] Bin3Probability = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3 };  //same here
    int[] Bin4Probability = { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 3 };  //same here

    int[] Bin5Probability = { 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2 };  //same here

    // Use this for initialization
    void Start () {
        PlaceStuff();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PlaceStuff()
    {
        Fillbins(Bin1Probability,bin1ButtomLeft.position.x,bin1ButtomLeft.position.y);
        Fillbins(Bin2Probability,bin2ButtomLeft.position.x,bin2ButtomLeft.position.y);
        Fillbins(Bin3Probability,bin3ButtomLeft.position.x,bin3ButtomLeft.position.y);
        Fillbins(Bin4Probability,bin4ButtomLeft.position.x,bin4ButtomLeft.position.y);
        Fillbins(Bin5Probability,bin5ButtomLeft.position.x,bin5ButtomLeft.position.y);
    }

    void Fillbins(int[] ProbabilityArray,float startx,float starty)
    {
        float nextZ = startZ, nextY = starty, nextX = startx;
        float centerx = startx + 0.2f;
        for (int i = 0; i < maxstuffineachBin; i++)
        {
            if (nextZ > maxZ) return;//if too deep then return bin filled
            int stuffindex = ProbabilityArray[Random.Range(0, ProbabilityArray.Length)];
            if (stuffindex == 1)
            {
                nextZ += Zsteps;
                Instantiate(LargeBox, new Vector3(centerx, starty, nextZ), Quaternion.identity);
            }
            else if (stuffindex == 2)
            {
                nextZ += Zsteps;
                Instantiate(SmallBox, new Vector3(centerx, starty, nextZ), Quaternion.identity);
            }
            else if (stuffindex == 3 && nextX == startx) {
                nextZ += Zsteps;
                for (int count = 0; count < 4; count++)
                {
                    Instantiate(Coke, new Vector3(nextX, starty, nextZ), Quaternion.identity);
                    nextX += Xsteps;
                }
                nextX = startx;
            }
        }
    }

    public void OnRefresh()
    {
        DestroyAll();
        PlaceStuff();
    }
    void DestroyAll()
    {
        GameObject[] stuffs = GameObject.FindGameObjectsWithTag("Stuff");
        foreach (GameObject stuff in stuffs) {
            Destroy(stuff);
        }
    }
}
