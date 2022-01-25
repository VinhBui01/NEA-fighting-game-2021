using System;
using System.Collections.Generic;

using UnityEngine;
using PlayerGenerator;
using TMPro;
public class GameManager : MonoBehaviour
{
    public bool GameWon = false;   
    public TMP_Text timerUI;
    public Player Player1;
    public Player Player2;

    public Vector2 PlayerPosition1 = new Vector2 (-7f, -2f);
    public Vector2 PlayerPosition2 = new Vector2 (7f, -2f);

    public int timer;
    //Player1 inputs
    public KeyCode P1Right = KeyCode.D;
    public KeyCode P1Left = KeyCode.A;
    public KeyCode P1Jump = KeyCode.W;
    public KeyCode P1Dash = KeyCode.LeftShift;
    public KeyCode P1AttackL = KeyCode.U;
    public KeyCode P1AttackH = KeyCode.I;
    public KeyCode P1Ult = KeyCode.Q;

    //Player2 inputs
    public KeyCode P2Right = KeyCode.RightArrow;
    public KeyCode P2Left = KeyCode.LeftArrow;
    public KeyCode P2Jump = KeyCode.UpArrow;
    public KeyCode P2Dash = KeyCode.RightShift;
    public KeyCode P2AttackL = KeyCode.H;
    public KeyCode P2AttackH = KeyCode.J;
    public KeyCode P2Ult = KeyCode.Slash;
    //hitboxes
    public Collider2D Player1HitBox;
    public Collider2D Player1HitBoxL;
    public Collider2D Player1HitBoxH;

    public SpriteRenderer Player1ShieldSprite;

    public Collider2D Player2HitBox;
    public Collider2D Player2HitBoxL;
    public Collider2D Player2HitBoxH;
    public SpriteRenderer Player2ShieldSprite;
    // initial health and damage values;
    public int P1Charge = 0;
    public int P1Heavy = 0;//how much damage p1 does
    public int P1Light = 0;//how much damage p1 does
    public float P1HP = 0;

    public int P2Charge = 0;
    public int P2Heavy = 0;//how much damage p2 does
    public int P2Light = 0;//how much damage p2 does
    public float P2HP = 0;
    //sliders
    public SliderScript P1HPBar;
    public SliderScript P1ChargeBar;
    public SliderScript P1AttackBar;
    public SliderScript P1DashBar;

    public SliderScript P2HPBar;
    public SliderScript P2ChargeBar;
    public SliderScript P2AttackBar;
    public SliderScript P2DashBar;
    //counters
    public int P1HeavyCount = 0;
    public int P1LightCount = 0;
    public int P2HeavyCount = 0;
    public int P2LightCount = 0;
    public void SetinitialSliders(int hp, int charge, SliderScript HPbar, SliderScript chargebar, SliderScript attackbar, SliderScript dashbar) 
    {
        HPbar.SetMax(hp);
        chargebar.SetMax(charge);
        attackbar.SetMax(60);
        dashbar.SetMax(30);
    }
    public void SetCurrentValues(int hp, int charge, int attack, int dash, SliderScript HPbar, SliderScript chargebar, SliderScript attackbar, SliderScript dashbar) 
    {
        HPbar.SetCurrent(hp);
        chargebar.SetCurrent(charge);
        attackbar.SetCurrent(attack);
        dashbar.SetCurrent(dash);
    }

    public void formattime() 
    {
        string second = ((timer / 60) % 60).ToString();
        string minute = (timer / 3600).ToString();

        if (second.Length < 2) { second = "0" + second; }
        if (minute.Length < 2) { minute = "0" + minute; }
        timerUI.text = minute+":"+ second;
    }

    public void GameEnd( string winner)
    {
        GameWon = true;
        Debug.Log(winner);
    }

    private void OnGUI()
    {
        GUI.contentColor = Color.white;
        GUI.Label(new Rect(0, 10, 200, 20), Player1.CustomHP.ToString(),new GUIStyle() { fontSize = 10 });
        GUI.Label(new Rect(70, 10, 200, 20), Player2.CustomHP.ToString(), new GUIStyle() { fontSize = 10 });
    }
    void Start()
    {
        Player1.inithitbox(Player1HitBoxL, Player1HitBoxH, Player1HitBox, Player2HitBoxL, Player2HitBoxH, Player2HitBox, Player1ShieldSprite); //instantiation of player1 hitbox
        Player1.initinput(true, P1Left, P1Right, P1Jump, P1Dash, P1AttackL, P1AttackH, P1Ult); //instantiation of player1 inputs
        Player1.initdamage(P1Charge, P2Heavy, P2Light, P1HP); //instantiation of player 1 HP

        Player2.inithitbox(Player2HitBoxL, Player2HitBoxH, Player2HitBox, Player1HitBoxL, Player1HitBoxH, Player1HitBox, Player2ShieldSprite); //instance of player 2
        Player2.initinput(false, P2Left, P2Right, P2Jump, P2Dash, P2AttackL, P2AttackH, P2Ult); //instance of player 2
        Player2.initdamage(P2Charge, P1Heavy, P1Light, P2HP); //instance of player 2
        Player2.transform.Rotate(new Vector3(0, 180, 0));//reverses player 2 direction
        Player2.DashDistance = -Player2.DashDistance;
        SetinitialSliders((int)Player1.CustomHP, Player1.CustomMaxCharge, P1HPBar, P1ChargeBar, P1AttackBar, P1DashBar);
        SetinitialSliders((int)Player2.CustomHP, Player2.CustomMaxCharge, P2HPBar, P2ChargeBar, P2AttackBar, P2DashBar);
        timer = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += 1;
        float Player1HP = Player1.GetComponent<Player>().CustomHP;
        float Player2HP = Player2.GetComponent<Player>().CustomHP;
        // Debug.Log(Player1.CustomHP); Debug.Log(Player2.CustomHP);
        P1HeavyCount = Player1.GetComponent<Player>().HeavyCount;//number of times p1 was hit
        P1LightCount = Player1.GetComponent<Player>().LightCount;
        P2HeavyCount = Player2.GetComponent<Player>().HeavyCount;//number of times p2 was hiit
        P2LightCount = Player2.GetComponent<Player>().LightCount;
        SetCurrentValues((int)Player1HP, Player1.Charge, Player1.DashTime, Player1.AttackTime, P1HPBar, P1ChargeBar, P1AttackBar, P1DashBar);
        SetCurrentValues((int)Player2HP, Player2.Charge, Player2.DashTime, Player2.AttackTime, P2HPBar, P2ChargeBar, P2AttackBar, P2DashBar);
        if (Player1HP <= 0 ) { GameEnd("Player 2 won");}
        else if (Player2HP <= 0) { GameEnd("Player 1 won"); }

        formattime();
    }
}
