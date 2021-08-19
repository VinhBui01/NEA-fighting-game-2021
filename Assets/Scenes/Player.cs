using System;
using System.Collections;
using UnityEngine;


namespace PlayerGenerator
{
    public class Player : MonoBehaviour

    {
        //damage variables
        public const float DefaultInitialHealth = 600;
        public const int DefaultInitialHeavyDamage = 60;
        public const int DefaultInitialLightDamage = 30;
        private int DefaultInitialCharge = 1200;
        public float CustomHP { get; set; }
        public int CustomHeavyDMG { get; set; } //this is how much damage is recieved when attacked
        public int CustomLightDMG { get; set; } //this is how much damage is recieved when attacked
        public int CustomMaxCharge { get; set; }
        public int Charge = 0;

        // Movement variables
        int JumpCount = 1; //number of jumps available
        public static float MoveSpeed = 13f;
        public static float JumpHeight = 65f;
        public static float DashDistance = 60f;
        float YVelocity;
        float XVelocity;
        bool FacingRight { get; set; } //stores direction facing
        bool InputLocked = false; //locks input if true
        public bool AttackLockedL = false; //activates light attack hitbox if true
        public bool AttackLockedH = false; //activates heavy attack hitbox if true
        public bool isbeingattacked;
        bool DashLocked = false; //activates dash if true
        public int DashTime = 0;
        public int AttackTime = 0;
        //InputCode
        private KeyCode InputRight { get; set; }
        private KeyCode InputLeft { get; set; }
        private KeyCode InputJump { get; set; }
        private KeyCode InputDash { get; set; }
        private KeyCode InputAttackL { get; set; }
        private KeyCode InputAttackH { get; set; }
        private KeyCode InputUlt { get; set; }
        //hitboxes
        private Collider2D AttackHitBoxL { get; set; }
        private Collider2D AttackHitBoxH { get; set; }
        private Collider2D Hitbox { get; set; }
        private Collider2D EnemyHitBoxL { get; set; }
        private Collider2D EnemyHitBoxH { get; set; }
        private Collider2D EnemyHitBox { get; set; }
        private SpriteRenderer ShieldSprite { get; set; }


        public void initinput(bool facingright, KeyCode inputleft, KeyCode inputright, KeyCode inputjump, KeyCode inputdash, KeyCode inputattackl, KeyCode inputattackh, KeyCode inputult)//player input constructor
        {
            this.FacingRight = facingright;
            this.InputLeft = inputleft;
            this.InputRight = inputright;
            this.InputJump = inputjump;
            this.InputDash = inputdash;
            this.InputAttackL = inputattackl;
            this.InputAttackH = inputattackh;
            this.InputUlt = inputult;

        }
        public void initdamage(int custommaxcharge, int customHeavyDMG, int customLightDMG, int customHP)//player damage constructor
        {
            this.CustomHP = customHP;
            this.CustomMaxCharge = custommaxcharge;
            this.CustomHeavyDMG = customHeavyDMG;
            this.CustomLightDMG = customLightDMG;
            this.CustomHeavyDMG = Default(CustomHeavyDMG, DefaultInitialHeavyDamage);
            this.CustomLightDMG = Default(CustomLightDMG, DefaultInitialLightDamage);
            this.CustomHP = Default((int)CustomHP, (int)DefaultInitialHealth);
            this.CustomMaxCharge = Default(CustomMaxCharge, DefaultInitialCharge);
        }
        public void inithitbox(Collider2D AttackHitBoxL, Collider2D AttackHitBoxH, Collider2D HitBox, Collider2D EnemyHitBoxL, Collider2D EnemyHitBoxH, Collider2D EnemyHitBox, SpriteRenderer shieldsprite)//player hitbox constructor
        {
            this.AttackHitBoxL = AttackHitBoxL;
            this.AttackHitBoxH = AttackHitBoxH;
            this.Hitbox = HitBox;
            this.EnemyHitBoxL = EnemyHitBoxL;
            this.EnemyHitBoxH = EnemyHitBoxH;
            this.EnemyHitBox = EnemyHitBox;
            this.ShieldSprite = shieldsprite;
        }

        private void ConstantMovement(KeyCode InputKey, float x, float y, bool InputLocked)//method to set velocity when key held
        {
            if (Input.GetKey(InputKey) & InputLocked == false)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);
            }
        }

        public virtual int BurstMovement(KeyCode InputKey, float x, float y, int Ammo, bool InputLocked) //method for jump movement
        {
            if (Input.GetKeyDown(InputKey) & Ammo > 0 & InputLocked == false)
            {
                this.GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);
                Ammo -= 1;
            }
            return Ammo;
        }

        IEnumerator InputLock(float inputlocktime) //method to lock input for dash timings
        {
            InputLocked = true;
            yield return new WaitForSeconds(inputlocktime);
            InputLocked = false;
        }
        IEnumerator DashLock(float dashtime) //method to lock input for dash timings
        {
            int TempJumpCount = JumpCount;
            DashLocked = true;
            yield return new WaitForSeconds(dashtime);
            DashLocked = false;
            JumpCount = TempJumpCount;
        }
        IEnumerator AttackLockL(float damagetime) //method to lock input for attack timings
        {
            yield return new WaitForSeconds(0.1f);
            AttackLockedL = true;
            yield return new WaitForSeconds(damagetime);
            AttackLockedL = false;
            AttackTime = 15;
        }
        IEnumerator AttackLockH(float damagetime) //method to lock input for attack timings
        {
            yield return new WaitForSeconds(0.3f);
            AttackLockedH = true;
            yield return new WaitForSeconds(damagetime);
            AttackLockedH = false;
            AttackTime = 0;
        }

        IEnumerator AttackLockUlt(float damagetime) //method to lock input for attack timings
        {
            CustomHP = CustomHP * 2;
            ShieldSprite.enabled = true;
            yield return new WaitForSeconds(damagetime);
            ShieldSprite.enabled = false;
            CustomHP = CustomHP /2;
        }


        public void Dashcode(float x, float y) //code for dash moevement
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);
            StartCoroutine(DashLock(0.05f)); //locks player into the dash for 0.29 seconds
            StartCoroutine(InputLock(0.3f));
            DashTime = 0;
        }

        public void LightAttack(KeyCode InputKey, bool AttackLocked) //light attack code
        {
            if (AttackLocked == false & Input.GetKeyDown(InputKey) & AttackTime >= 60)
            {
                StartCoroutine(InputLock(0.35f));
                StartCoroutine(AttackLockL(0.15f));

            }
        }
        public void HeavyAttack(KeyCode InputKey, bool AttackLocked) //heavy attack code
        {
            if (AttackLocked == false & Input.GetKeyDown(InputKey) & AttackTime >= 60)
            {
                StartCoroutine(InputLock(0.9f));
                StartCoroutine(AttackLockH(0.35f));
            }
        }

        public void Ultimate(KeyCode InputKey, bool AttackLocked) //ult attack code
        {
            if (AttackLocked == false & Input.GetKeyDown(InputKey) & Charge > CustomMaxCharge)
            {
                StartCoroutine(InputLock(0.3f));
                StartCoroutine(AttackLockUlt(6));
                Charge = 0;
            }
        }

        int Default(int CustomValue, int DefaultValue) //sets values to default if 0
        {
            if (CustomValue == 0)
            { return DefaultValue; }
            else { return CustomValue; }
        }
        void OnTriggerEnter2D(Collider2D col) 
        {
            if (col == EnemyHitBoxH) { CustomHP -= CustomHeavyDMG;}
            if (col == EnemyHitBoxL) { CustomHP -= CustomLightDMG;}
        }

        void Start()
        {
            Physics2D.IgnoreLayerCollision(6, 6, true);
        }
        void Update()
        {
            YVelocity = this.GetComponent<Rigidbody2D>().velocity.y;
            XVelocity = this.GetComponent<Rigidbody2D>().velocity.x;

            JumpCount = BurstMovement(InputJump, XVelocity, JumpHeight, JumpCount, InputLocked); // jump input

            if (YVelocity*YVelocity < 0.001) { JumpCount = 1; } //resets jump when y velocity is 0 (on ground)

            if (Input.GetKeyDown(InputLeft) & FacingRight == true & InputLocked == false) { FacingRight = false; transform.Rotate(new Vector3(0, 180, 0)); DashDistance = -DashDistance; } //face left
            ConstantMovement(InputLeft, -MoveSpeed, YVelocity, InputLocked); //move left

            if (Input.GetKeyDown(InputRight) & FacingRight == false & InputLocked == false) { FacingRight = true; transform.Rotate(new Vector3(0, 180, 0)); DashDistance = -DashDistance; } //face right

            ConstantMovement(InputRight, MoveSpeed, YVelocity, InputLocked); //move right

            if (Input.GetKeyDown(InputDash) & DashTime>30 &InputLocked == false){ Dashcode(DashDistance, 0);}
            LightAttack(InputAttackL, AttackLockedL);
            HeavyAttack(InputAttackH, AttackLockedH);

            Ultimate(InputUlt, InputLocked);
        }
        void FixedUpdate() //updates 60 times a second
        {
            Charge += 1;
            DashTime += 1;
            AttackTime += 1;
            YVelocity = this.GetComponent<Rigidbody2D>().velocity.y;
            XVelocity = this.GetComponent<Rigidbody2D>().velocity.x;

            //keeps hitboxes enabled while AttackLocked is true
            AttackHitBoxL.gameObject.SetActive(AttackLockedL);
            AttackHitBoxH.gameObject.SetActive(AttackLockedH);

            if (DashLocked ==true) { GetComponent<Rigidbody2D>().velocity = new Vector2(DashDistance, 0); }
        }
    }
}