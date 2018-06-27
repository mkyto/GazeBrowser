using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*#if WINDOWS_UWP
using Windows.Storage;
using System.Threading.Tasks;
using System;
#endif*/

public class createScene : MonoBehaviour
{
    GameObject symbolToBeDublicated;
    GameObject thingToBeDublicated;
    Transform[] thingToBeDub_children;
    List<GameObject> dublicatedThings;
    List<GameObject> dublicatedSymbols;
    GameObject parentForDup;
    public int thingType;
    List<Vector3> positions;
    Vector3 startingPosition;
    Vector3 startingPositionThing;
    int a;
    int id;
    List<Texture2D> statusScreens;
    List<Texture2D> dataLogScreens;
    //int gazeMode;
    GameObject[] things;

    Vector3 originalSymbolScale;
    Vector3 originalThingTabScale;
    Vector3 originalThingRadialScale;
    Vector3 originalThingHierScale;

    float originalSymbolColliderRadius;

    public int condition;
    // Use this for initialization
    void Awake()
    {

        if(condition == 1 || condition == 2) 
        {

            GameObject.Find("EyeCursor").SetActive(false);
            GameObject.Find("HeadCursor").SetActive(false);
        }

        if (condition == 3)
        {
            GameObject.Find("Cursor").SetActive(false);
        }


        positions = new List<Vector3>();

        //startingPosition = GameObject.Find("ThingSymbol").transform.position;
        //startingPositionThing = GameObject.Find("ThingTab").transform.position;

        startingPosition = new Vector3(0.0f, 0.0f, 0.0f);
        startingPositionThing = new Vector3(0.0f, 0.0f, 0.0f);
        //startingPositionThing = GameObject.Find("ThingTab").transform.position;
        parentForDup = GameObject.Find("ImageTargetChips");
        //Vector3 offsetPosition = new Vector3(-0.5f, -0.05f, -0.7f);

        Vector3 offsetPosition = new Vector3(0.0f, 0.0f, 0.0f);
       /* for (int i = 1; i < 4; i++)
        {
            for (int j = 1; j < 4; j++) {
                
                float s_x = Mathf.Sign(-0.5f + Random.value);
                if(s_x == 0)
                {
                    s_x = 1.0f;
                }

                float x_value = Random.value + 3 * s_x; // Check if zero

                // float s_y = Mathf.Sign(-0.5f + Random.value);
                float y_value = Random.value * 3.0f;

                float s_z = Mathf.Sign(-0.5f + Random.value);

                if (s_z == 0)
                {
                    s_z = 1.0f;
                }
                float scalingFactor = 1.0f;//0.1357f;
                float z_value = Random.value + 3 * s_z;
                Debug.Log("X value: " + x_value);
                positions.Add(new Vector3(x_value * scalingFactor, y_value * scalingFactor, z_value * scalingFactor) + offsetPosition);
                //positions.Add(new Vector3(i * 0.4f, 0.0f, j * 0.3f )+offsetPosition);
            }
        }*/


        positions.Add(new Vector3(-3.6f,0.2f,5.5f));
        positions.Add(new Vector3(-7.7f, 1.0f, -20.7f));
        positions.Add(new Vector3(6.23f, -2.1f, -27.1f));
        positions.Add(new Vector3(15.23f, 1.1f, -27.1f));
        positions.Add(new Vector3(21.23f, 0.0f, -26.1f));
        positions.Add(new Vector3(4.23f, 0.2f, -30.1f));
        positions.Add(new Vector3(16.23f, 0.1f, -35.1f));
        positions.Add(new Vector3(20.23f, 0.5f, -12.1f));
        positions.Add(new Vector3(10.23f, 0.5f, -12.1f));
        dublicatedThings = new List<GameObject>();
        dublicatedSymbols = new List<GameObject>();
        symbolToBeDublicated = GameObject.Find("ThingSymbol");
        originalSymbolScale = symbolToBeDublicated.transform.localScale;

        originalSymbolColliderRadius = symbolToBeDublicated.GetComponent<SphereCollider>().radius;
        originalThingTabScale = GameObject.Find("ThingTab").transform.localScale;
        originalThingRadialScale = GameObject.Find("ThingRadial").transform.localScale;
        originalThingHierScale = GameObject.Find("ThingHier").transform.localScale;
        

        statusScreens = new List<Texture2D>();
        dataLogScreens = new List<Texture2D>();
        
        parentForDup.GetComponent<MeshRenderer>().enabled = false;

        a = 1;

        GameObject.Find("ThingSymbol").transform.Translate(new Vector3(10000.0f, 0.0f, 0.0f));
        GameObject.Find("ThingHier").transform.Translate(new Vector3(10000.0f, 0.0f, 0.0f));
        GameObject.Find("ThingTab").transform.Translate(new Vector3(10000.0f, 0.0f, 0.0f));
        GameObject.Find("ThingRadial").transform.Translate(new Vector3(10000.0f, 0.0f, 0.0f));
        GameObject.Find("Thing5Symbol").transform.Translate(new Vector3(10000.0f, 0.0f, 0.0f));


        /*GameObject.Find("ThingSymbol").GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("ThingHier").GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("ThingTab").GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("ThingRadial").GetComponent<MeshRenderer>().enabled = false;
        GameObject.Find("Thing5Symbol").GetComponent<MeshRenderer>().enabled = false;*/

       /* GameObject.Find("ThingHier").GetComponent<Collider>().enabled = false;
        GameObject.Find("ThingTab").GetComponent<Collider>().enabled = false;
        GameObject.Find("ThingRadial").GetComponent<Collider>().enabled = false;
        GameObject.Find("Thing5Symbol").GetComponent<Collider>().enabled = false;*/


        //GameObject.Find("Thing1Symbol").transform.Translate(new Vector3(100.0f, 0.0f, 0.0f));

        foreach (Vector3 pos in positions)
        {
            string pathToTextures = "Thing" + a + "/Slide1.JPG";
            string pathToLogs = "Thing" + a + "/Slide2.JPG";
            string fileNameStatus = Path.Combine(Application.streamingAssetsPath, pathToTextures);
            string fileNameData = Path.Combine(Application.streamingAssetsPath, pathToLogs);
            //string pathToTextures = Application.dataPath + "/Resources/" + "Thing" + a + "/Slide1.JPG";
            //string pathToLogs = Application.dataPath + "/Resources/" + "Thing" + a + "/Slide2.JPG";

            
            if (File.Exists(fileNameStatus))
            {
                Debug.Log("Tekstuuri loytyi");
                statusScreens.Add(new Texture2D(1, 1));
                dataLogScreens.Add(new Texture2D(1, 1));
                //Texture2D tex = new Texture2D(1, 1);
                //Texture2D tex2 = new Texture2D(1, 1);
                //byte[] bytes = System.IO.File.ReadAllBytes(pathToTextures);
                //byte[] bytes2 = System.IO.File.ReadAllBytes(pathToLogs)
                byte[] bytes = UnityEngine.Windows.File.ReadAllBytes(fileNameStatus);
                byte[] bytes2 = UnityEngine.Windows.File.ReadAllBytes(fileNameData);
                statusScreens[statusScreens.Count - 1].LoadImage(bytes);
                dataLogScreens[dataLogScreens.Count - 1].LoadImage(bytes2);

            }
            Vector3 temp;
            temp = startingPosition + pos;
            Debug.Log("Paikka1 :" + temp);

            GameObject t = Object.Instantiate(symbolToBeDublicated, (startingPosition + pos) * parentForDup.transform.localScale.x, symbolToBeDublicated.transform.rotation, parentForDup.transform) as GameObject; //parent scale is symmetric

            temp = t.transform.position;
                Debug.Log("Paikka2 :" + temp);
            t.GetComponent<SymbolProperties>().ID = a;
            t.transform.FindChild("id_text").GetComponent<TextMesh>().text = a.ToString();
            t.name = "dublicateThingSymbol" + a;
            t.tag = "dublicateThingSymbol";
            a = a + 1;
        }
    }




    // Update is called once per frame
    void Update()
    {

    }


    void selectType(int thingType)
    {
        GameObject[] toDelete = GameObject.FindGameObjectsWithTag("Thing");
        
        for (int i = 0; i < toDelete.Length; i++)
        {
            Destroy(toDelete[i].transform.parent.gameObject);
            Destroy(toDelete[i].gameObject);

        }

        


        if (thingType == 1)
        {
            //GameObject.Find("ThingHier").GetComponent<MeshRenderer>().enabled = true;
            thingToBeDublicated = GameObject.Find("ThingHier");
            //thingToBeDublicated.transform.localScale = 1.5f * originalThingHierScale;


        }

        if (thingType == 2)
        {
           // GameObject.Find("ThingTab").GetComponent<MeshRenderer>().enabled = true;
            thingToBeDublicated = GameObject.Find("ThingTab");
           // thingToBeDublicated.transform.localScale = 1.5f * originalThingTabScale;
        }

        if (thingType == 3)
        {
          //  GameObject.Find("ThingRadial").GetComponent<MeshRenderer>().enabled = true;
            thingToBeDublicated = GameObject.Find("ThingRadial");
          //  thingToBeDublicated.transform.localScale = 1.5f * originalThingRadialScale;

        }
        id = 1;

        if (condition == 2)
        {
            //thingToBeDublicated.transform.localScale = 2 * thingToBeDublicated.transform.localScale;
        }


        foreach (Vector3 pos in positions)
        {
            

            

            GameObject dubThing = Object.Instantiate(thingToBeDublicated, (startingPosition + pos) * parentForDup.transform.localScale.x, thingToBeDublicated.transform.rotation, parentForDup.transform) as GameObject;
         
            //if (dubThing.transform.FindChild("Thing").GetComponent<myRayCastListenerTab>() != null)
            //{
             //   dubThing.transform.FindChild("Thing").FindChild("pinnedID_text").GetComponent<TextMesh>().text = id.ToString();

            //}

            if (dubThing.transform.FindChild("Thing").GetComponent<myRayCastListenerRadial>() != null)
            {
                dubThing.transform.FindChild("Thing").FindChild("pinnedID_text").GetComponent<TextMesh>().text = id.ToString();

            }


            dubThing.GetComponent<ThingProperties>().ID = id;
            dubThing.name = thingToBeDublicated.name + id; 
            dubThing.transform.FindChild("Thing").GetComponent<thingID>().ID = id;
            dubThing.transform.FindChild("Thing").tag = "Thing";
            thingToBeDub_children = dubThing.GetComponentsInChildren<Transform>();

            //Material newMat = Resources.Load("SlideX.jpg", typeof(Material)) as Material;

            foreach (Transform trans in thingToBeDub_children)
                {

                    if (thingType == 1)
                    {
                        if (trans.name == "infoScreenThing1")
                        {
                            trans.gameObject.GetComponent<Renderer>().material.mainTexture = statusScreens[id-1];
                        }

                        if (trans.name == "infoScreen2Thing1")
                        {

                            trans.gameObject.GetComponent<Renderer>().material.mainTexture = dataLogScreens[id - 1];

                        }
                    }

                    if (thingType == 2)
                    {
                        if (trans.name == "statusScreen")
                        {
                            trans.gameObject.GetComponent<Renderer>().material.mainTexture = statusScreens[id - 1];
                    }

                        if (trans.name == "DatalogScreen")
                        {
                        
                            trans.gameObject.GetComponent<Renderer>().material.mainTexture = dataLogScreens[id - 1];

                    }
                    }

                    if (thingType == 3)
                    {

                    Debug.Log("Statusten maara: " + statusScreens);
                    Debug.Log("Yritys: " + id);

                    if (trans.name == "statusScreen")
                        {
                            trans.gameObject.GetComponent<Renderer>().material.mainTexture = statusScreens[id - 1];
                    }

                        if (trans.name == "infoScreenDataLog")
                        {

                            trans.gameObject.GetComponent<Renderer>().material.mainTexture = dataLogScreens[id - 1];
                    }
                    }         
            }
            id = id + 1;
            dublicatedThings.Add(dubThing);
        }

    } 
}