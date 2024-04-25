//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[RequireComponent(typeof(PolygonCollider2D))]
//public class pentagonFilling : MonoBehaviour
//{
//    PolygonCollider2D polygonCollider;

//    void Start()
//    {
//        polygonCollider = GetComponent<PolygonCollider2D>();

//        // Set the points of the PolygonCollider2D to match the positions of the LineRenderer
//        PentagonFillOutline outlineScript = FindObjectOfType<PentagonFillOutline>();
//        if (outlineScript != null)
//        {
//            polygonCollider.points = outlineScript.GetPentagonPoints();
//        }
//    }
//}   