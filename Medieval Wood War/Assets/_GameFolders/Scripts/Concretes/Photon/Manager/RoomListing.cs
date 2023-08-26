using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class RoomListing : MonoBehaviourPunCallbacks
{
    public Transform Grid;
    public GameObject RoomNamePrefab;
  
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Received room list update. Total rooms: " + roomList.Count);

        foreach (RoomInfo room in roomList)
        {
            Debug.Log("Room: " + room.Name + ", Is Visible: " + room.IsVisible + ", Is Open: " + room.IsOpen);

            if (room.RemovedFromList)
            {
                DeleteRoom(room);
            }
            else 
            {
                AddRoom(room);
            }
        }  
    }
   
    void AddRoom(RoomInfo room)
    {
        Debug.Log("Attempting to add room: " + room.Name);

        GameObject obj = Instantiate(RoomNamePrefab, new Vector2(0, 0), Quaternion.identity);
        obj.transform.SetParent(Grid.transform, false);
        obj.GetComponentInChildren<Text>().text = room.Name;

        Debug.Log("Room added to the list: " + room.Name);
    }

    void DeleteRoom(RoomInfo room)
    {
        Debug.Log("Attempting to delete room: " + room.Name);

        int roomCounts = Grid.childCount;
        for (int i = 0; i < roomCounts; ++i)
        {
            if (Grid.GetChild(i).gameObject.GetComponentInChildren<Text>().text == room.Name)
            {
                Destroy(Grid.GetChild(i).gameObject);
                Debug.Log("Room deleted from the list: " + room.Name);
            }
        }
    }
}