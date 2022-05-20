using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.AR;
using TMPro;

public class LineManger : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public TextMeshPro mText;
    public ARPlacementInteractable placementInteractable;
    private int pointCount=0;
    LineRenderer line;
    public bool continous;
    public TextMeshProUGUI buttonText;
    // Start is called before the first frame update
    void Start()
    {
        placementInteractable.objectPlaced.AddListener(DrawLine);
    }

    public void Toggle()
    {
        if(continous)
        {
            buttonText.text="Discrete";
        }
        else
        {
            buttonText.text="Continous";
        }
        continous=!continous;
    }

    public void Refresh()
    {
        pointCount=0;
        lineRenderer.positionCount=0;
    }

    void DrawLine(ARObjectPlacementEventArgs args)
    {
        pointCount++;
        if(pointCount<2)
        {
            line=Instantiate(lineRenderer);
            line.positionCount=1;
        }
        else
        {
            line.positionCount=pointCount;
            if(!continous)
                pointCount=0;
        }

        //let the point location in the line renderer 
        line.SetPosition(index: line.positionCount-1, args.placementObject.transform.position);
        if(line.positionCount>1)
        {
            Vector3 pointA= line.GetPosition(index: line.positionCount -1);
            Vector3 pointB= line.GetPosition(index: line.positionCount -2);
            float dist=Vector3.Distance(pointA, pointB);

            TextMeshPro distText= Instantiate(mText);
            distText.text=""+Mathf.Round(dist*100)*0.01;

            Vector3 dierctionVector=(pointB-pointA);
            Vector3 normal=args.placementObject.transform.up;
            
            Vector3 upd=Vector3.Cross(lhs: dierctionVector, rhs:normal).normalized;
            Quaternion rotation=Quaternion.LookRotation(forward:-normal, upwards: upd);

            distText.transform.rotation=rotation;
            distText.transform.position=(pointA+dierctionVector*0.5f)+upd*0.05f;
        }
    }
}
