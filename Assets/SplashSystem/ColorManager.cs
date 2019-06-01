using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public List<ColorPanel> panels = new List<ColorPanel>();
    public GameObject waterPrefab;

    public GameObject[] splashPrefabs;

    public enum Colors {
        None = 0,
        Red = 1,
        Green = 2,
        Blue = 4,
        RG = Red & Green,
        RB = Red & Blue,
        GB = Green & Blue,
    }

    static public ColorManager CM = null;
    void Awake()
    {
        if(CM == null)
        {
            CM = this;
        }
        else if ( CM != this )
        {
            Destroy(this);            
        }
    }


    public void CreateSplash(Color color1, Color color2, Vector3 pos)
    {
        int index = Random.Range(0, splashPrefabs.Length);
        GameObject spla = Instantiate(splashPrefabs[index]);
        spla.transform.position = new Vector3(pos.x, PoissonFishing.instance.fishingManagement.waterPlane.transform.position.y, pos.z);
        spla.transform.Rotate(Vector3.up * Random.Range(-180,180), Space.Self);
        spla.GetComponentInChildren<ColorPanel>().baseColor = color1;
        
        int index2 = Random.Range(0, splashPrefabs.Length);
        GameObject spla2 = Instantiate(splashPrefabs[index]);
        spla2.transform.position = new Vector3(pos.x, PoissonFishing.instance.fishingManagement.waterPlane.transform.position.y, pos.z);
        spla2.transform.Rotate(Vector3.up * Random.Range(-180, 180), Space.Self);
        spla2.GetComponentInChildren<ColorPanel>().baseColor = color2;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) SendNewColor(Colors.Blue, new Vector3(0,0,5), 1.5f);
        if(Input.GetKeyDown(KeyCode.R)) SendNewColor(Colors.Red, new Vector3(0,0,5), 1.5f);
    }

    static public void SendNewColor(Colors color, Vector3 position = new Vector3(), float time = 1f)
    {
        if(position != Vector3.zero)
        {
            Instantiate(CM.waterPrefab, position, Quaternion.identity).GetComponentInChildren<ColorWater>().UpdateColor(color, time);
        }

        CM.SendColorsToPanels(color, time);
    }

    private void SendColorsToPanels(Colors color, float time)
    {
        foreach (ColorPanel panel in panels)
        {
            panel.UpdateColor(color, time);    
        }
        /*foreach (ColorWater water in waters)
        {
            water.UpdateColor(color, time);    
        }*/
    }
}
