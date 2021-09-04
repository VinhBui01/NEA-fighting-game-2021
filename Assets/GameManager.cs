using System;
using System.Collections.Generic;

using UnityEngine;
using PlayerGenerator;
using UnityEngine.SceneManagement;
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

    public int[][] MatchAttackCount;
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
        MatchAttackCount[0] = new int[] { Player1.GetComponent<Player>().HeavyCount, Player1.GetComponent<Player>().LightCount};
        MatchAttackCount[1] = new int[] { Player2.GetComponent<Player>().HeavyCount, Player2.GetComponent<Player>().LightCount};
        Debug.Log(winner + " won"); 
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

        SetinitialSliders((int)Player1.CustomHP, Player1.CustomMaxCharge, P1HPBar, P1ChargeBar, P1AttackBar, P1DashBar);
        SetinitialSliders((int)Player2.CustomHP, Player2.CustomMaxCharge, P2HPBar, P2ChargeBar, P2AttackBar, P2DashBar);
        timer = 0;
        Debug.Log("1");
        new WaitForSeconds(3);
        Debug.Log("3");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += 1;
        float Player1HP = Player1.GetComponent<Player>().CustomHP;
        float Player2HP = Player2.GetComponent<Player>().CustomHP;

        SetCurrentValues((int)Player1HP, Player1.Charge, Player1.DashTime, Player1.AttackTime, P1HPBar, P1ChargeBar, P1AttackBar, P1DashBar);
        SetCurrentValues((int)Player2HP, Player2.Charge, Player2.DashTime, Player2.AttackTime, P2HPBar, P2ChargeBar, P2AttackBar, P2DashBar);
        if (Player1HP <= 0 ) { GameEnd("Player 2"); }
        else if (Player2HP <= 0) { GameEnd("Player 1"); }
        formattime();
    }
}
