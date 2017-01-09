using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class PersonSpawner : MonoBehaviour {
    public GameObject npcPrefab;
    public Sprite[] sprites;
    public List<GameObject> npcs;
    public int respawnTimer = 1000;
        
    public int timeUntilRespawn;
    public int currentNPC = 0;
    private int spawnedNPCs = 0;

    //customers are all the customers that cant be picked from
    //empty on startup
    public List<Customer> spawnedCustomers;

    //when a customer is spawned, they will be placed in spawnedCustomers and removed
    //from availble customers. This will ensure we dont get the same person running around 
    //at the same time.
    public List<Customer> availableCustomers;

    void Awake()
    {
        npcs = new List<GameObject>();
        timeUntilRespawn = respawnTimer;
    }

    void Start()
    {
        string path = Application.dataPath + "/resources/people.json";
        Customers customers = JsonUtility.FromJson<Customers>(File.ReadAllText(path));
        this.availableCustomers = new List<Customer>(customers.customers);
    }

    public void destoryPerson(GameObject person)
    {
        NpcController npc = person.GetComponent<NpcController>();
        updateAvailableList(npc.customerInfo);
        npcs.Remove(person);
        DestroyObject(person);

    }

	void Update () {
        
        if (npcs.Count < 3)
        {
            timeUntilRespawn -= 1;
            if (timeUntilRespawn > 0)
                return;
            float direction = (currentNPC % 2 == 0 ? -1 : 1);
            Quaternion rotation = new Quaternion(0, direction == 1 ? 0 : 180f, 0, 0);
            int startingPosition = direction == -1 ? -20 : 20;
            GameObject person = (GameObject)Instantiate(npcPrefab, new Vector2(startingPosition, -2.25f), rotation); 
            person.GetComponent<SpriteRenderer>().sprite = sprites[currentNPC];
            //initialize the npcs starting position, customer info, id, etc
            initNPCController(person, startingPosition);

            if (currentNPC == 2)
                currentNPC = 0;
            spawnedNPCs++;
            currentNPC++;

            npcs.Add(person);
            timeUntilRespawn = respawnTimer;
        }
            
	}

    void initNPCController(GameObject person, int startingPosition)
    {
        NpcController npc = person.GetComponent<NpcController>();
        npc.setId(spawnedNPCs);
        npc.customerInfo = availableCustomers[Random.Range(0, availableCustomers.Count)];
        npc.setDirection(startingPosition < 0 ? NpcController.Direction.RIGHT : NpcController.Direction.LEFT);
        updateSpawnedList(npc.customerInfo);
    }

    void updateSpawnedList(Customer spawned)
    {
        spawnedCustomers.Add(spawned);
        availableCustomers.Remove(spawned);
    }

    void updateAvailableList(Customer destoryed)
    {
        spawnedCustomers.Remove(destoryed);
        availableCustomers.Add(destoryed);
    }
}
