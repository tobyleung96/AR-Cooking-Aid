using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceCalculator : MonoBehaviour
{
    public GameObject distanceText;
    private List<GameObject> textList = new List<GameObject>();
    private List<Vector3> positionList = new List<Vector3>();



    private LineRenderer line;
    private Vector3 curPosition;
    private Vector3 lastPosition; // for line animation
    private const float k_MovementSpeed = 3f;

    private List<float> scales;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(gameObject.name);
        line = gameObject.GetComponent<LineRenderer>();
        line.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(positionList.Count>1){
            lastPosition = Vector3.Lerp(lastPosition, 
                                    positionList[positionList.Count-1], 
                                    k_MovementSpeed * Time.deltaTime);
            line.SetPosition((positionList.Count-1), lastPosition);
        }
        line.transform.LookAt(Camera.main.transform);

        //having text look at camera
        for(int i = 0; i < (textList.Count-1); i++){
            textList[i].transform.LookAt(Camera.main.transform);
        }
    }


    public void addPosition(Vector3 nextPosition){

        positionList.Add(nextPosition);
        
        if(positionList.Count <2){ //this is the one position
            lastPosition = nextPosition;
        } else{
            lastPosition = positionList[positionList.Count-2]; // line animation
            curPosition = positionList[positionList.Count-2]; // draw text

            // draw text distance
            Vector3 midPoint =  curPosition + (nextPosition - curPosition) / 2;
            GameObject dText = Instantiate(distanceText, midPoint, Quaternion.identity);
            dText.GetComponent<TextMesh>().text = Vector3.Distance(nextPosition, curPosition).ToString("F2") + "m";
            scales.Add(Vector3.Distance(nextPosition, curPosition));
            dText.transform.LookAt(Camera.main.transform);
            textList.Add(dText);
        }
        //add position to line, draw the lines expect the last one
        line.positionCount = line.positionCount + 1;
        line.SetPosition(line.positionCount-1, nextPosition);
        
    }

    public void removeLastPosition(){
        Destroy(textList[textList.Count-1]);
        textList.RemoveAt(textList.Count-1);
        positionList.RemoveAt(positionList.Count-1);
        line.positionCount = line.positionCount-1;


    }


    public void restart(){
        for(int i = textList.Count-1 ; i > -1; i--){
            Destroy(textList[i]);
            textList.RemoveAt(i);
        }
        textList.Clear();
        positionList.Clear();
        line.positionCount = 0;
    }

    public List<float> getScales(){
        // if(scales.Count > 2){
        //     return scales;
        // }else{
        //     return null;
        // }
            return scales;

    }
}
