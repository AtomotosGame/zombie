﻿using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuLogic: Singleton<MenuLogic> {

    //private MenuScreens currentScreen;
    private MenuScreens prevScreen;
    private GameObject trash;

    int money = Globals.TOTAL_MONEY;

    public int Money { get { return money; } }

    private void Start() {
        Init();
    }

    private void Init() {
        prevScreen = Globals.Instance.currentScreen = MenuScreens.Default;
        trash = GameObject.FindGameObjectWithTag("Trash");
        trash.SetActive(false);
        //money = PlayerPrefs.GetInt("Money", Globals.TOTAL_MONEY); //TODO: Uncomment it
        GameView.SetText("Txt_CurrMoney", money.ToString());
        LoadStrategy();
        ShutdownScreens();    
        ChangeMenuState(MenuScreens.Main);
    }

    public void BuySoldier (int price) {
        money -= price;
        PlayerPrefs.SetInt("Money", money);
        GameView.SetText("Txt_CurrMoney", money.ToString());
    }

    public void SellSoldier(int price) {
        money += price;
        PlayerPrefs.SetInt("Money", money);
        GameView.SetText("Txt_CurrMoney", money.ToString());
    }

    public void SaveStrategy() {
        var buildTiles = TileManager.Instance.BuildTiles;
        foreach(var tile in buildTiles.Values) {
            //print(tile.Row + ", " + tile.Column + Regex.Match(soldier.name, @"\d+").Value + ", ");
            if(tile.IsInUse) {
                PlayerPrefs.SetString(tile.Row + "," + tile.Column, Regex.Match(tile.Soldier.name, @"^[a-zA-Z0-9]*").Value);
            }
            else {
                PlayerPrefs.SetString(tile.Row + "," + tile.Column, "");
            }
        }
    }

    public void LoadStrategy() {
        int y = 0, z = 0;
        var matrixTile = TileManager.Instance.MatrixTiles;
        var soldierBtns = Globals.Instance.GetAllSoldierBtns();
        for(int i = 0; i < Globals.MAX_SOLDIERS_FOR_PLAYER + 1; i++) {
            string tilePattern = PlayerPrefs.GetString(y + "," + z, "");
            if(tilePattern != "") {
                StrategyEditor.Instance.PlaceSoldier(matrixTile[y, z], soldierBtns[tilePattern].SoldierObject, false);
            }
            z++;
            if(z == 4) {
                y++;
                z = 0;
            }
        }
    }

    private void ShutdownScreens() {
        var unityObjects = Globals.Instance.UnityObjects;
        unityObjects["ScreenMenu"].SetActive(false);
        unityObjects["ScreenLoading"].SetActive(false);
        unityObjects["ScreenOptions"].SetActive(false);
        unityObjects["ScreenStudentInfo"].SetActive(false);
        unityObjects["ScreenMultiplayer"].SetActive(false);
        unityObjects["ScreenEdit"].SetActive(false);
        unityObjects["TitleGameImg"].SetActive(false);
    }

    public void GoBack() {
        if(prevScreen != MenuScreens.Main && Globals.Instance.currentScreen != MenuScreens.Options) {
            prevScreen = MenuScreens.Main;
        }
        ChangeMenuState(prevScreen);
    }

    public void ChangeMenuState(MenuScreens newScreen) {
        var unityObjects = Globals.Instance.UnityObjects;

        prevScreen = Globals.Instance.currentScreen;

        switch(prevScreen) {
            case MenuScreens.Main: unityObjects["ScreenMenu"].SetActive(false); break;

            case MenuScreens.SinglePlayer:
                break;

            case MenuScreens.MultiPlayer: unityObjects["ScreenMultiplayer"].SetActive(false); break;

            case MenuScreens.StudentInfo: unityObjects["ScreenStudentInfo"].SetActive(false); break;

            case MenuScreens.Options: unityObjects["ScreenOptions"].SetActive(false); break;

            case MenuScreens.Loading: unityObjects["ScreenLoading"].SetActive(false); break;

            case MenuScreens.Edit:
                unityObjects["ScreenEdit"].SetActive(false);
                unityObjects["TitleGameImg"].SetActive(false);
                trash.SetActive(false);
                ToggleMenuWindow(true);
                StrategyEditor.Instance.DisableDragSprite();
                StrategyEditor.IsInEdit = false;
                break;

            default: break;
        }

        Globals.Instance.currentScreen = newScreen;
        switch(Globals.Instance.currentScreen) {
            case MenuScreens.Main:
                unityObjects["ScreenMenu"].SetActive(true);
                GameView.SetText("TitleMenu", "Main Menu");
                break;

            case MenuScreens.SinglePlayer:
                //SoundManager.Instance.Music.clip  = SoundManager.Instance.InGameMusic;
                SceneManager.LoadSceneAsync("Game_Scene");
                break;

            case MenuScreens.MultiPlayer:
                unityObjects["ScreenMultiplayer"].SetActive(true);
                GameView.SetText("TitleMenu", "Multiplayer");
                break;

            case MenuScreens.StudentInfo:
                unityObjects["ScreenStudentInfo"].SetActive(true);
                GameView.SetText("TitleMenu", "Student Info");
                break;

            case MenuScreens.Options:
                unityObjects["ScreenOptions"].SetActive(true);
                GameView.SetText("TitleMenu", "Options");
                break;

            case MenuScreens.Loading: unityObjects["ScreenLoading"].SetActive(true); break;

            case MenuScreens.Edit:
                unityObjects["ScreenEdit"].SetActive(true);
                unityObjects["TitleGameImg"].SetActive(true);
                trash.SetActive(true);
                GameView.SetText("TitleMenu", "Edit mode");
                ToggleMenuWindow(false);
                StrategyEditor.IsInEdit = true;
                break;

            default: break;
        }
    }

    public void UpdateMoneySliderTxt(float value) {
        GameView.SetText("MoneyLbl", value + "$");
        PlayerPrefs.SetInt("MoneyBet", (int) value);
    }

    public void UpdateMusicVolume(float value) {
        SoundManager.Instance.Music.volume = value;
        PlayerPrefs.SetFloat("Music",value);
    }


    public void UpdateSfxVolume(float value) {
        SoundManager.Instance.SFX.volume = value;
        PlayerPrefs.SetFloat("SFX", value);
    }

    public void OpenGithub() {
        Application.OpenURL(Globals.GITHUB_PROFILE_URL);
    }

    public void OpenCV() {
        Application.OpenURL(Globals.CV_URL);
    }

    private void ToggleMenuWindow(bool isTurnOn) {
        Globals.Instance.UnityObjects["MainWindow"].SetActive(isTurnOn);
        Globals.Instance.UnityObjects["Img_Logo"].SetActive(isTurnOn);
    }
}