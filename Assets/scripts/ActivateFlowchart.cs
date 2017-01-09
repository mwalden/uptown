using UnityEngine;
using System.Collections;
using Fungus;

public class ActivateFlowchart : MonoBehaviour
{

    public Flowchart flowChart;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;
        flowChart.SetStringVariable("interacting", gameObject.tag);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag != "Player")
            return;
        flowChart.SetStringVariable("interacting", "");

    }
}
