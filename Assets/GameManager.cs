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
    public SpriteRenderer Player1LightSprite;
    public SpriteRenderer Player1HeavySprite;
    public SpriteRenderer Player1ShieldSprite;

    public Collider2D Player2HitBox;
    public Collider2D Player2HitBoxL;
    public Collider2D Player2HitBoxH;
    public SpriteRenderer Player2LightSprite;
    public SpriteRenderer Player2HeavySprite;
    public SpriteRenderer Player2ShieldSprite;



    void Start()
    {
        Player1.inithitbox(Player1HitBoxL, Player1HitBoxH, Player1HitBox, Player2HitBoxL, Player2HitBoxH, Player2HitBox, Player1HeavySprite, Player1LightSprite, Player1ShieldSprite); //instantiation of player1 hitbox
        Player1.initinput(true, P1Left, P1Right, P1Jump, P1Dash, P1AttackL, P1AttackH, P1Ult); //instantiation of player1 inputs
        Player1.initdamage(0, 0, 0, 0); //instantiation of player 1 HP

        Player2.inithitbox(Player2HitBoxL, Player2HitBoxH, Player2HitBox, Player1HitBoxL, Player1HitBoxH, Player1HitBox, Player2HeavySprite, Player2LightSprite, Player2ShieldSprite); //instance of player 2
        Player2.initinput(false, P2Left, P2Right, P2Jump, P2Dash, P2AttackL, P2AttackH, P2Ult); //instance of player 2
        Player2.initdamage(0, 0, 0, 0); //instance of player 2
        Player2.transform.Rotate(new Vector3(0, 180, 0));//reverses player 2 direction
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float Player1HP = Player1.GetComponent<Player>().CustomHP;
        float Player2HP = Player2.GetComponent<Player>().CustomHP;
        
        Debug.Log(Player1HP);
        //Debug.Log("P1C:" + Player1.GetComponent<Player>().Charge);
        //Debug.Log(Player2HP);
        //Debug.Log("P2C:" + Player2.GetComponent<Player>().Charge);

        if (Player1HP <= 0 ) { this.enabled = false; Debug.Log("Player2 won"); }
        else if (Player2HP <= 0) { this.enabled = false; Debug.Log("Player1 won"); }
    }
}
