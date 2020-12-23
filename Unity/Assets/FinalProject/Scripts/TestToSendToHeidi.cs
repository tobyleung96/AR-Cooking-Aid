using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TestToSendToHeidi : MonoBehaviour
{
    [SerializeField]
    private GameObject ARSessionOrigin;
    private ARRaycastManager _arRaycastManager;
    private Vector2 touchPosition;
    public static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    bool expanded = false;
    private GameObject OrderSummary;
    private GameObject Plane;
    private GameObject StaticText;
    private GameObject UpdateText;
    float k_Scale1Speed = 3.0f;
    float k_Scale2Speed = 3.0f;
    float k_Transform1Speed = 2.0f;
    float k_Transform2Speed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        _arRaycastManager = ARSessionOrigin.GetComponent<ARRaycastManager>();
        Plane = OrderSummary.transform.GetChild(0).gameObject;
        StaticText = OrderSummary.transform.GetChild(1).gameObject;
        UpdateText = OrderSummary.transform.GetChild(2).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Handle Touch Events
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Ended)
            {
                bool isOverUI = IsPointOverUIObject(touch.position);

                if (isOverUI == false)
                {
                    if (_arRaycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                    {
                        //addedInstances.Add(Instantiate(gameObjectToInstantiate, hits[0].pose.position, Quaternion.identity));
                        if(expanded == false)
                        {
                            StartCoroutine(ScaleUp());
                            expanded = true;
                        }
                        else if (expanded == true)
                        {
                            StartCoroutine(ScaleDown());
                            expanded = true;
                        }
                    }
                }
            }
        }
    }

    IEnumerator ScaleUp()
    {
        Plane.transform.localScale = new Vector3(1, Mathf.Lerp(1,5, Time.deltaTime * k_Scale1Speed), 1);
        StaticText.transform.position = new Vector3(1, Mathf.Lerp(1, 5, Time.deltaTime * k_Transform1Speed), 1);
        UpdateText.transform.position = new Vector3(1, Mathf.Lerp(1, 5, Time.deltaTime * k_Transform2Speed), 1);
        UpdateText.transform.localScale = new Vector3(1, Mathf.Lerp(0, 5, Time.deltaTime * k_Scale2Speed), 1);
        yield return null;
    }

    IEnumerator ScaleDown()
    {
        Plane.transform.localScale = new Vector3(1, Mathf.Lerp(5, 1, Time.deltaTime * k_Scale1Speed), 1);
        StaticText.transform.position = new Vector3(1, Mathf.Lerp(5, 1, Time.deltaTime * k_Transform1Speed), 1);
        UpdateText.transform.position = new Vector3(1, Mathf.Lerp(5, 1, Time.deltaTime * k_Transform2Speed), 1);
        UpdateText.transform.localScale = new Vector3(1, Mathf.Lerp(5, 0, Time.deltaTime * k_Scale2Speed), 1);
        yield return null;
    }

    public static bool IsPointOverUIObject(Vector2 pos)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false;
        }

        PointerEventData eventPosition = new PointerEventData(EventSystem.current);
        eventPosition.position = new Vector2(pos.x, pos.y);

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventPosition, results);

        return results.Count > 0;
    }
}
