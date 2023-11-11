using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHub : MonoBehaviour
{
    // Start is called before the first frame update
    //Static Functionality
    private static AiHub _inst;
    public static AiHub Instance
    {
        get
        {
            if (_inst is null)
            {
                _inst = new AiHub();
                _inst.Init();
            }
            return _inst;
        }

    }


    //Class Values
    public static bool globalArcEnabled;

    public bool displayGlobalArcEnabled = globalArcEnabled;
    public static GameObject lastEnemyViewing;
    public static GameObject lastViewArc;

    //private Constructor 
    private AiHub()
    {

    }
    public static void SetGlobalArc(GameObject currentEnemy, GameObject currentArc)
    {
        if (lastEnemyViewing != null && lastViewArc != null)
        {
            lastEnemyViewing.GetComponent<FieldOfView>().enabled = false;
            lastViewArc.SetActive(false);
            if (currentEnemy != lastEnemyViewing && currentArc != lastViewArc)
            {
                currentEnemy.GetComponent<FieldOfView>().enabled = true;
                currentArc.SetActive(true);
                lastEnemyViewing = currentEnemy;
                lastViewArc = currentArc;
                //globalArcEnabled = true;
            }
            else
            {
                currentEnemy.GetComponent<FieldOfView>().enabled = false;
                currentArc.SetActive(false);
                lastEnemyViewing = null;
                lastViewArc = null;
                //globalArcEnabled = false;
            }

        }
        else
        {
            currentEnemy.GetComponent<FieldOfView>().enabled = true;
            currentArc.SetActive(true);
            lastEnemyViewing = currentEnemy;
            lastViewArc = currentArc;
            //globalArcEnabled = true;
        }

    }
    //initialize values here
    private void Init()
    {
        globalArcEnabled = false;
    }
}
