using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Welder : Enemy
    {
        private float attackRadius;
        private float rotation;

        public bool Attacking
        {
            get; set;
        }
        public double TimeCounter
        {
            get; set;
        }
        public double SecondsPerFrame
        {
            get; set;
        }

        public float AttackRadius { get { return attackRadius; } set { attackRadius = value + (Hitbox.Width / 2); } }

        public Welder(float posX, float posY)
            : this(40, 5f, 25, posX, posY)
        {
        }

        public Welder(float health, float damage, float attackCooldown, float posX, float posY)
            : base(health, damage, attackCooldown, posX, posY)
        {
            rotation = 0;
            MaxHealth = 100;
            Hitbox = new Rectangle((int)posX, (int)posY, 64, 64);
            attackRadius = 15 + (Hitbox.Width / 2);
            Texture = ContentManager.Instance.WelderTexture;
            speed = 2;
            TimeCounter = 0;
            SecondsPerFrame = (double)1 / 12;
        }

        public override void Attack(Player p)
        {
            float distance = (float)Distance(Hitbox.Center, p.Hitbox.Center);
            distance = distance - (attackRadius - Hitbox.Width/2);
            if(distance / 2 < attackRadius)
            {
                p.Health = p.Health - this.Damage;
                Cooldown = AttackCooldown;
                Attacking = true;
            }
            else
            {
                Attacking = false;
            }
        }

        public int Distance(Point p1, Point p2)
        {
            return (int)Math.Sqrt((Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2)));
        }

        public override void Update()
        {
            Move(ObjectManager.Instance.Player);
            if (Cooldown <= 0)
            {
                Attack(ObjectManager.Instance.Player);
            }
            else
            {
                Cooldown = Cooldown - 1;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            float x = XTransform;
            float y = YTransform;

            if (x > 1 || x < -1)
            {
                x = x / Math.Abs(x);
            }
            if (y > 1 || y < -1)
            {
                y = y / Math.Abs(y);
            }

            float mouseX = InputManager.Instance.MState.Position.X + ObjectManager.Instance.Camera.Position.X;
            float mouseY = InputManager.Instance.MState.Position.Y + ObjectManager.Instance.Camera.Position.Y;

            float playerX = ObjectManager.Instance.Player.X;
            float playerY = ObjectManager.Instance.Player.Y;

            if (Attacking)
            {
                rotation = (float)Math.Atan((playerY - this.Y) / (playerX - this.X));

                if (playerX < this.X)
                {
                    rotation += (float)(Math.PI);
                }
            }
            else
            {
                TimeCounter += InputManager.Instance.Time.ElapsedGameTime.TotalSeconds;

                if (TimeCounter >= SecondsPerFrame)
                {
                    rotation = (float)(Math.PI * 2 * ObjectManager.Instance.Rng.NextDouble());
                    TimeCounter -= SecondsPerFrame;
                }

            }

                spriteBatch.Draw(ContentManager.Instance.WelderAttackTexture, new Vector2((this.Hitbox.Center.X), (this.Hitbox.Center.Y)), null, Color.White,
                    rotation, new Vector2(0, ContentManager.Instance.WelderAttackTexture.Bounds.Center.Y), new Vector2(1.5f, 1), SpriteEffects.None, 1.0f);

            base.Draw(spriteBatch);
        }
    }
}
