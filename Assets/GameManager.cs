using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using PlayerGenerator;

public class GameManager : MonoBehaviour
{   
    public Player Player1;
    public GameObject Player1UI;
    public GameObject Player2UI;
    public Player Player2;

    public Vector2 PlayerPosition1 = new Vector2 (-7f, -2f);
    public Vector2 PlayerPosition2 = new Vector2 (7f, -2f);

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
    public int P1HP = 0;

    public int P2Charge = 0;
    public int P2Heavy = 0;//how much damage p2 does
    public int P2Light = 0;//how much damage p2 does
    public int P2HP = 0;

    public void SetSlider() { }
    void Start()
    {
        Player1.inithitbox(Player1HitBoxL, Player1HitBoxH, Player1HitBox, Player2HitBoxL, Player2HitBoxH, Player2HitBox, Player1ShieldSprite); //instantiation of player1 hitbox
        Player1.initinput(true, P1Left, P1Right, P1Jump, P1Dash, P1AttackL, P1AttackH, P1Ult); //instantiation of player1 inputs
        Player1.initdamage(P1Charge, P2Heavy, P2Light, P1HP); //instantiation of player 1 HP

        Player2.inithitbox(Player2HitBoxL, Player2HitBoxH, Player2HitBox, Player1HitBoxL, Player1HitBoxH, Player1HitBox, Player2ShieldSprite); //instance of player 2
        Player2.initinput(false, P2Left, P2Right, P2Jump, P2Dash, P2AttackL, P2AttackH, P2Ult); //instance of player 2
        Player2.initdamage(P2Charge, P1Heavy, P1Light, P2HP); //instance of player 2
        Player2.transform.Rotate(new Vector3(0, 180, 0));//reverses player 2 direction
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float Player1HP = Player1.GetComponent<Player>().CustomHP;
        float Player2HP = Player2.GetComponent<Player>().CustomHP;
        
        Debug.Log(Player1HP);
        Debug.Log("P1C:" + Player1.GetComponent<Player>().Charge);
        Debug.Log(Player2HP);
        Debug.Log("P2C:" + Player2.GetComponent<Player>().Charge);

        if (Player1HP <= 0 ) { this.enabled = false; Debug.Log("Player2 won"); }
        else if (Player2HP <= 0) { this.enabled = false; Debug.Log("Player1 won"); }
    }
}
