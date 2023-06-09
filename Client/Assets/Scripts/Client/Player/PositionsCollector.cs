using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionsCollector : MonoBehaviour
{
    // Contains the enemy gameobjects
    private List<GameObject> positions = new List<GameObject>();
   
    public GameObject enemyGameobject;

    private bool canUpdate = false;
    private string message;

    public CheckCollisions checkCollisions;

    private void Start()
    {
        checkCollisions = GetComponent<CheckCollisions>();


        // Believe it or not we actually need this.
        // In my case when I had my system language on Dutch it would save floats such as 5.4 as 5,4 breaking some of the code
        // I found this out by accident by the way
        // Alternatively I could've just replaced the "," with somehting like a # or other character when splitting the positions
        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
        System.Threading.Thread.CurrentThread.CurrentCulture = culture;
        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
    }

    // Needs to be ran from the main thread
    private void Update()
    {
        if (canUpdate)
        {
            List<GameObject> tempPositions = this.positions;

            canUpdate = false;

            // First we need to delete all Gameobjects in our positions list
            foreach (var position in tempPositions)
            {
                Destroy(position.gameObject);
            }

            // We clear our positions so we can add them from the positions_string
            tempPositions.Clear();

            // Is going to store all of the Vector2 positions of the GameObjects we are going to create in a bit
            List<Vector2> vector2s = new List<Vector2>();


            String positions_string = this.message.Split("_")[3];
            string[] positonsCSV = positions_string.Split(",");

            foreach (var i in positonsCSV)
            {
                string[] coords = i.Split(";");
                vector2s.Add(new Vector2(float.Parse(coords[0]), float.Parse(coords[1])));
            }

            foreach (var i in vector2s)
            {
                GameObject enemy = Instantiate(enemyGameobject, i, Quaternion.identity);
                tempPositions.Add(enemy);
            }

            this.positions = tempPositions;

            checkCollisions.setEnemies(this.positions);
            checkCollisions.checkCollisions();

        }
    }


    public void updateEnemeyPositions(string message)
    {
        print("UPDATING ALREADY LMAO");
        print(message);
        // The message format: positions_roomNumber_playerNum_actualPositionsString
        canUpdate=true;
        this.message = message;          
    }

    public void reset()
    {
        foreach (GameObject i in this.positions)
        {
            Destroy(i);
        }

        this.positions.Clear();
        this.message = "";
    }
    



}