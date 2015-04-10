﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuSystem : MonoBehaviour
{
    public string[] gamemodes;
    public string[] levels;
    public Vector3 inGame;
    public Vector3 outGame;
    private bool firstFrame;
    private bool choosingGamemode;
    private bool choosingLevel;
    private choice[] gameChoices;
    private choice[] levelChoices;
    private GameObject[] spawns;
    GUIStyle labelStyle;
    private bool lastA0 = false, lastA1 = false, lastA2 = false, lastA3 = false, lrb0 = false, llb0 = false, lrb1 = false,
        llb1 = false, lrb2 = false, llb2 = false, lrb3 = false, llb3 = false;


    GameSettings gs;
    
    private bool inTransition = false;
    
    private float lerp = 0.0f;
    private float fadeSpeed = 1.0f;

    private string levelName;

    struct choice
    {
        public bool value;
        public string name;
        public string levelToLoad;
    }
    // Use this for initialization
    void Start()
    {

        inTransition = false;
        lerp = 0.0f;

        gs = GameObject.FindObjectOfType<GameSettings>();
        firstFrame = true;
        choosingGamemode = false;
        choosingLevel = false;
        gameChoices = new choice[gamemodes.Length];
        levelChoices = new choice[levels.Length];
        for (int i=0; i<gamemodes.Length; i++)
        {
            gameChoices [i] = new choice();
            gameChoices [i].value = false;
            gameChoices [i].name = gamemodes [i];
        }
        for (int i=0; i<levels.Length; i++)
        {
            levelChoices [i] = new choice();
            levelChoices [i].value = false;
            string[] split = levels [i].Split('/');
            levelChoices [i].name = split [0];
            levelChoices [i].levelToLoad = split [1].TrimEnd();
        }
        
        GameManager[] managers = GameObject.FindObjectsOfType<GameManager>();
        if (managers.Length > 1)
        {
            for (int i=1; i<managers.Length; i++)
            {
                GameObject.Destroy(managers [i].gameObject);
            }
        }
        spawns = GameObject.FindGameObjectsWithTag("SpawnPoint");
        gameChoices [0].value = true;
        levelChoices [0].value = true;
        labelStyle = new GUIStyle();
        labelStyle.fontSize = 12;
        labelStyle.normal.textColor = new Color(1, 0, 0);
    }
    
    void switchAllChoiceBools(choice[] choices, int stay)
    {
        for (int i=0; i<choices.Length; i++)
        {
            if (i != stay)
            {
                choices [i].value = false;
            }
        }
    }
    
    choice getCurrChoice(choice[] choices)
    {
        choice ret = new choice();
        for (int i=0; i<choices.Length; i++)
        {
            if (choices [i].value)
            {
                ret = choices [i];
            }
        }
        return ret;
    }
    
    void spawnPlayer(GameObject[] players, int playerID)
    {
        if (!players [playerID].renderer.isVisible)
        {
            players [playerID].transform.position = spawns[playerID%spawns.Length].transform.position;
        } else
        {
            players [playerID].transform.position = outGame;
        }
    }
    
    void resetPlayers()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("MainMenuPlayer");
        int numPlayers = 0;
        if (players.Length != 4)
        {
            for (int i=0; i<players.Length; i++)
            {
                if (players [i].renderer.isVisible)
                {
                    Destroy(players [i]);
                    numPlayers++;
                }
            }
        }
        players = GameObject.FindGameObjectsWithTag("MainMenuPlayer");
        for (int i=numPlayers; i<players.Length; i++)
        {   
            if (i < numPlayers * 2)
            {
                spawnPlayer(players, i);
            }
        }
        GameManager.hardReset();
        GameManager.UnpauseThisGodForsakenGame();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (firstFrame)
        {
            resetPlayers();
            firstFrame = false; 
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("MainMenuPlayer");

        if (gs.visibleLastFrame > 0)
        {
            lastA0 = true;
            lastA1 = true;
            lastA2 = true;
            lastA3 = true;
        }

        if (gs.visibleLastFrame <= 0 && !gs.show)
        {
            if (Input.GetButton("A_1"))
            {
                if (!lastA0)
                    spawnPlayer(players, 0);
                lastA0 = true;
            } else
            {
                lastA0 = false;
            }
            if (Input.GetButton("A_2"))
            {
                if (!lastA1)
                    spawnPlayer(players, 1);
                lastA1 = true;
            } else
            {
                lastA1 = false;
            }
            if (Input.GetButton("A_3"))
            {
                if (!lastA2)
                    spawnPlayer(players, 2);
                lastA2 = true;
            } else
            {
                lastA2 = false;
            }
            if (Input.GetButton("A_4"))
            {
                if (!lastA3)
                    spawnPlayer(players, 3);
                lastA3 = true;
            } else
            {
                lastA3 = false;
            }


            if (gameChoices [1].value)
            {
                if (Input.GetButton("LB_1"))
                {
                    if (!llb0)
                    {
                        players [0].GetComponent<Character>().teamNum --;
                        if (players [0].GetComponent<Character>().teamNum < 0)
                        {
                            players [0].GetComponent<Character>().teamNum = 3;
                        }
                    }
                    llb0 = true;
                } else
                {
                    llb0 = false;
                }
                if (Input.GetButton("RB_1"))
                {
                    if (!lrb0)
                    {
                        players [0].GetComponent<Character>().teamNum ++;
                        if (players [0].GetComponent<Character>().teamNum > 3)
                        {
                            players [0].GetComponent<Character>().teamNum = 0;
                        }
                    }
                    lrb0 = true;
                } else
                {
                    lrb0 = false;
                }

                if (Input.GetButton("LB_4"))
                {
                    if (!llb3)
                    {
                        players [3].GetComponent<Character>().teamNum --;
                        if (players [3].GetComponent<Character>().teamNum < 0)
                        {
                            players [3].GetComponent<Character>().teamNum = 3;
                        }
                    }
                    llb3 = true;
                } else
                {
                    llb3 = false;
                }
                if (Input.GetButton("RB_4"))
                {
                    if (!lrb3)
                    {
                        players [3].GetComponent<Character>().teamNum ++;
                        if (players [3].GetComponent<Character>().teamNum > 3)
                        {
                            players [3].GetComponent<Character>().teamNum = 0;
                        }
                    }
                    lrb3 = true;
                } else
                {
                    lrb3 = false;
                }



                if (Input.GetButton("LB_3"))
                {
                    if (!llb2)
                    {
                        players [2].GetComponent<Character>().teamNum --;
                        if (players [2].GetComponent<Character>().teamNum < 0)
                        {
                            players [2].GetComponent<Character>().teamNum = 3;
                        }
                    }
                    llb2 = true;
                } else
                {
                    llb2 = false;
                }
                if (Input.GetButton("RB_3"))
                {
                    if (!lrb2)
                    {
                        players [2].GetComponent<Character>().teamNum ++;
                        if (players [2].GetComponent<Character>().teamNum > 3)
                        {
                            players [2].GetComponent<Character>().teamNum = 0;
                        }
                    }
                    lrb2 = true;
                } else
                {
                    lrb2 = false;
                }


                if (Input.GetButton("LB_2"))
                {
                    if (!llb1)
                    {
                        players [1].GetComponent<Character>().teamNum --;
                        if (players [1].GetComponent<Character>().teamNum < 0)
                        {
                            players [1].GetComponent<Character>().teamNum = 3;
                        }
                    }
                    llb1 = true;
                } else
                {
                    llb1 = false;
                }
                if (Input.GetButton("RB_2"))
                {
                    if (!lrb1)
                    {
                        players [1].GetComponent<Character>().teamNum ++;
                        if (players [1].GetComponent<Character>().teamNum > 3)
                        {
                            players [1].GetComponent<Character>().teamNum = 0;
                        }
                    }
                    lrb1 = true;
                } else
                {
                    lrb1 = false;
                }

                /*
            for (int i =0; i < players.Length; i ++)
            {
                int check = (players [i].GetComponent<Character>().teamNum * 4) + i;
                Material mCheck = FindObjectOfType<GameManager>().materials [check];
                players [i].GetComponent<MeshRenderer>().material = new Material(mCheck);
                players [i].GetComponent<Character>().baseColor = players [i].GetComponent<MeshRenderer>().material.color;
            }
            */
            }

        }
        /*
        if (gameChoices [0].value)
        {
            for (int i =0; i < players.Length; i ++)
            {
                int check = i;
                Material mCheck = FindObjectOfType<GameManager>().materials [check];
                players [i].GetComponent<MeshRenderer>().material = new Material(mCheck);
                players [i].GetComponent<Character>().baseColor = players [i].GetComponent<MeshRenderer>().material.color;
            }
        }
        */


        //Fade system.


        guiTexture.color = new Color(guiTexture.color.r, guiTexture.color.g, guiTexture.color.b, lerp);
        if (inTransition)
        {
            lerp += fadeSpeed * Time.deltaTime;
            if (lerp > 1)
            {
                inTransition = false;
                lerp = 0;
                LoadSwitchLevel(levelName);
            }
        }
    }
    
    int indexOfTrue(choice[] choices)
    {
        int index = -1;
        for (int i=0; i<choices.Length && index == -1; i++)
        {
            index = (choices [i].value) ? i : index;
        }
        return index;
    }


    public void GoToLevel(string levelIndex)
    {
        levelName = levelIndex;
        inTransition = true;
    }

    void LoadSwitchLevel(string name)
    {
        if (GameManager.startingTime == 0)
        {
            GameManager.startingTime = float.PositiveInfinity;
        }
        if (GameManager.maxScore == 0)
        {
            GameManager.maxScore = int.MaxValue;
        }
        GameObject[] players = GameObject.FindGameObjectsWithTag("MainMenuPlayer");
        int count = 0;
        List<Character> newPlayers = new List<Character>();
        for (int i=0; i<players.Length; i++)
        {
            if (players [i].renderer.isVisible)
            {
                count++;
                GameObject.DontDestroyOnLoad(players [i]);
                players [i].AddComponent<ScreenWrap>();
                newPlayers.Add(players [i].GetComponent<Character>());
            }
            else
            {
                Destroy(players[i].gameObject);
            }
        }
        if (count > 0)
        {
            //GameManager.setGamemode(indexOfTrue(gameChoices) + 1);
            GameManager.isLoaded = false;
            Application.LoadLevel(name);
            GameManager.setPlayers(newPlayers.ToArray());
        }
    }
    
    //void OnGUI()
    //{
    //    int height = Screen.height / 25;
    //    int width = 150;
    //    int gap = 15;
    //    int gapSoFar = 0;
    //    int numGUIObjects = 2+gameChoices.Length+levelChoices.Length;
    //    GUI.Box(new Rect(Screen.width / 11, Screen.height / 11, width + 2 * (Screen.width / 10 - Screen.width / 11), numGUIObjects * (height + gap) + 2 * (Screen.height / 10 - Screen.height / 11)), "");
    //    GUI.Label(new Rect(Screen.width / 10, Screen.height / 10, width, height), "Choose Gamemode", labelStyle);
    //    gapSoFar += height + gap;
    //    for (int i=0; i<gameChoices.Length; i++)
    //    {
    //        if (GUI.Toggle(new Rect(Screen.width / 10, Screen.height / 10 + gapSoFar, width, height), gameChoices [i].value, gameChoices [i].name))
    //        {
    //            gameChoices [i].value = true;
    //            switchAllChoiceBools(gameChoices, i);
    //        }
    //        gapSoFar += height + gap;
    //    }
    //    GUI.Label(new Rect(Screen.width / 10, Screen.height / 10 + gapSoFar, width, height), "Choose Level", labelStyle);
    //    gapSoFar += height + gap;
    //    for (int i=0; i<levelChoices.Length; i++)
    //    {
    //        if (GUI.Toggle(new Rect(Screen.width / 10, Screen.height / 10 + gapSoFar, width, height), levelChoices [i].value, levelChoices [i].name))
    //        {
    //            levelChoices [i].value = true;
    //            switchAllChoiceBools(levelChoices, i);
    //        }
    //        gapSoFar += height + gap;
    //    }
    //    gapSoFar += gap;
    //    if (GUI.Button(new Rect(Screen.width / 10, Screen.height / 10 + gapSoFar, width, height * 2), "Start Game"))
    //    {
    //        choice level = getCurrChoice(levelChoices);
    //        GameObject[] players = GameObject.FindGameObjectsWithTag("MainMenuPlayer");
    //        int count = 0;
    //        List<Character> newPlayers = new List<Character>();
    //        for (int i=0; i<players.Length; i++)
    //        {
    //            if (players [i].renderer.isVisible)
    //            {
    //                count++;
    //                GameObject.DontDestroyOnLoad(players [i]);
    //                players [i].AddComponent<ScreenWrap>();
    //                newPlayers.Add(players [i].GetComponent<Character>());
    //            }
    //        }
    //        if (count > 0)
    //        {
    //            GameManager.setGamemode(indexOfTrue(gameChoices) + 1);
    //            GameManager.isLoaded = false;
    //            Application.LoadLevel(level.levelToLoad);
    //            GameManager.setPlayers(newPlayers.ToArray());
    //        }
    //    }
    //    gapSoFar += 2 * height + gap;
    //    /*if(GUI.Button(new Rect(Screen.width/10, Screen.height/10+gapSoFar, width, height*2), "Close"))
    //    {
    //        Application.Quit();
    //    }*/
    //}
}

