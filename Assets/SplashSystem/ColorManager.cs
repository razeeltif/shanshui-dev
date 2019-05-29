using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public List<ColorPanel> panels = new List<ColorPanel>();
    public GameObject waterPrefab;

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
