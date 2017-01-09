using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Fungus;
using System.IO;

public class Controller : MonoBehaviour {
    public int speed = 5;
    public Flowchart flowChart;
    public Text moneyText;
    public Flowchart dialogFlowchart;
    public Clock clock;
    public GameObject prefabDialogWindow;
    public int protection;
    public bool infected;


    private bool inFlowChart;
    private GameObject dialogWindow;
    private Quaternion dialogWindowRotation;
    private bool interacting;
    private GameObject npcObject;
    private bool talking;


    public enum PurchasedItem
    {
        PROTECTION,
        SODA,
        SANDWICH
    };
    
    void Start () {
        dialogWindow = (GameObject)Instantiate(prefabDialogWindow, new Vector3(transform.position.x, transform.position.y + 2, 0), Quaternion.identity);
        dialogWindow.transform.SetParent(gameObject.transform);
        dialogWindow.SetActive(false);
        dialogWindowRotation = dialogWindow.transform.rotation;
        flowChart.SetIntegerVariable("money",GameState.Instance.money);        
        updateMoney();
    }

    void LateUpdate()
    {
        dialogWindow.transform.rotation = dialogWindowRotation;
    }
	
	// Update is called once per frame
	void Update () {
        if (inFlowChart)
            return;
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0);
        
        Vector3 newPosition = transform.position + (move * speed * Time.deltaTime);
        transform.position = newPosition;        
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        
        if (interacting)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                NpcController npc = npcObject.GetComponent<NpcController>();
                prepareFungusForConverstion(npc);
                clock.paused = true;
                npc.pausedPlayer();     
                talking = true;
                setInFlowChart(true);
            }
        }   
    }

    public void beginInteraction(GameObject gameObject)
    {
        if (talking) {
            dialogFlowchart.enabled = false;
            return;
        }

        interacting = gameObject != null;
        npcObject = gameObject;
    }

    public void endDialogWithFungus(bool affectClock)
    {
        NpcController currentNpc = (npcObject != null) ? npcObject.GetComponent<NpcController>() : null;
        if (currentNpc != null)
            currentNpc.continueWalking();
        cleanUpVariables(affectClock);
    }

    private void cleanUpVariables(bool affectClock)
    {
        if (affectClock)
            clock.paused = false;
        dialogFlowchart.enabled = false;
        interacting = false;
        talking = false;
        npcObject = null;
        setInFlowChart(false);
    }

    public void endInteraction(GameObject gameObject)
    {
        NpcController newNpc = gameObject.GetComponent<NpcController>();
        NpcController currentNpc = (npcObject != null) ? npcObject.GetComponent<NpcController>() : null;
        dialogFlowchart.enabled = false;

        if (currentNpc != null && newNpc != null && newNpc.id != currentNpc.id)
            return;
        else
        {        
            if (npcObject != null)
            {
                Text text = npcObject.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
                text.enabled = false;
                
            }
            endDialogWithFungus(false);
        }
    }
    

    public void boughtItem(int cost, PurchasedItem item)
    {   
        if (item == PurchasedItem.PROTECTION)
        {
            protection += 3;
        }
        GameState.Instance.money -= cost;
        updateMoney();
    }

    public void setInFlowChart(bool inFlowChart)
    {
        this.inFlowChart = inFlowChart;
    }

    private void updateMoney()
    {
        moneyText.text = "Money :" + GameState.Instance.money;
        flowChart.SetIntegerVariable("money", GameState.Instance.money);
    }

    public void collectMoney(int money,int time)
    {
        GameState.Instance.money += money;
        protection--;
        if (protection <= 0)
        {
            Text text = dialogWindow.GetComponentInChildren<Canvas>().GetComponentInChildren<Text>();
            text.text = "You are out of protection. You may want to buy more.";
            dialogWindow.SetActive(true);
        }
            
        updateMoney();
        cleanUpVariables(true);
        clock.updateTime(time);
        Debug.Log(time);
    }

    public void resetPlayer()
    {
        transform.position = new Vector3(-20, transform.position.y);
    }

    private void prepareFungusForConverstion(NpcController npc)
    {
        Customer info = npc.customerInfo;
        dialogFlowchart.SetStringVariable("openingLine", info.openingLine);
        dialogFlowchart.SetStringVariable("request", info.request);
        dialogFlowchart.SetBooleanVariable("ignoresYou", info.ignoresYou);
        dialogFlowchart.SetStringVariable("description", info.description);
        dialogFlowchart.SetStringVariable("exitLine", ExitLines.lines[Random.Range(0,ExitLines.lines.Length)]);
        dialogFlowchart.SendFungusMessage("initiateConversation");

    }
    
}
