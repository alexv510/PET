using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public GameObject[] availableRooms;
    public List<GameObject> currentRooms;
    private float screenWidthInPoints;

    // Start is called before the first frame update
    void Start()
    {
        float height = 2.0f * Camera.main.orthographicSize;
        screenWidthInPoints = height * Camera.main.aspect;
        StartCoroutine(GeneratorCheck());
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("CurrentRoom:" + currentRooms);
    }

    void AddRoom(float farthestRoomEndX)
    {
        //picks a random index of the room type (Prefab) to generate
        int randomRoomIndex = Random.Range(0, availableRooms.Length);

        //creates a room object from the array of available rooms using the random index chosen above
        GameObject room = (GameObject)Instantiate(availableRooms[randomRoomIndex]);

        //get the size of the floor inside the room, which is equal to the room's width
        float roomWidth = room.transform.Find("floor").localScale.x;

        //take the furthest edge of the level so far and add half of the new room's width. new room will start exactly where the previous room ended
        float roomCenter = farthestRoomEndX + roomWidth * 0.5f;

        //sets the position of the room. only need to change the x-coord since all roms have the same y and z coord = 0
        room.transform.position = new Vector3(roomCenter, 0, 0);

        //add room to list of current rooms
        currentRooms.Add(room);
    }

    private void GenerateRoomIfRequire()
    {
        List<GameObject> roomsToRemove = new List<GameObject>();

        bool addRooms = true;

        float playerX = transform.position.x;

        float removeRoomX = playerX - screenWidthInPoints;

        float addRoomX = playerX + screenWidthInPoints;

        float farthestRoomEndX = 0;

        foreach(var room in currentRooms)
        {
            float roomWidth = room.transform.Find("floor").localScale.x;
            float roomStartX = room.transform.position.x - (roomWidth * 0.5f);
            float roomEndX = roomStartX + roomWidth;

            if(roomStartX > addRoomX)
            {
                addRooms = false;
            }

            if(roomEndX < removeRoomX)
            {
                roomsToRemove.Add(room);
            }

            farthestRoomEndX = Mathf.Max(farthestRoomEndX, roomEndX);
        }

        foreach(var room in roomsToRemove)
        {
            currentRooms.Remove(room);
            Destroy(room);
        }

        if (addRooms)
        {
            AddRoom(farthestRoomEndX);
        }
    }

    private IEnumerator GeneratorCheck()
    {
        while (true)
        {
            GenerateRoomIfRequire();
            yield return new WaitForSeconds(0.25f);
        }
    }
}
