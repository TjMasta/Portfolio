using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Bomber : Enemy
    {
        private Rectangle explosionBox;
        private GameTime time;
        private int deathTimer;

        public GameTime Time { get { return time; } set { time = value; } }

        public Bomber(float posX, float posY, GameTime time)
            : this(35, 40f, 25, posX, posY, time)
        {

        }

        public Bomber(float health, float damage, float attackCooldown, float posX, float posY, GameTime time)
            : base(health, damage, attackCooldown, posX, posY)
        {
            Texture = ContentManager.Instance.BomberTexture;
            explosionBox = new Rectangle((int)posX - 64, (int)posY - 64, 192, 192);
            Hitbox = new Rectangle((int)posX, (int)posY, 64, 64);
            IsActive = false;
            this.time = time;
            deathTimer = 0;
        }

        public override void Attack(Player p)
        {
            float distance = (float)Math.Sqrt(Math.Pow(this.Hitbox.Center.X - p.Hitbox.Center.X, 2) + Math.Pow(this.Hitbox.Center.Y - p.Hitbox.Center.Y, 2));

            if(distance < explosionBox.Width/2)
            {
                DoDamage(p);
            }
        }

        public void DoDamage(Player p)
        {
            if(explosionBox.Intersects(p.Hitbox))
            {
                p.Health -= Damage;
            }
            Texture = ContentManager.Instance.BomberAttackTexture;
            scale = 2;
        }

        public override void Update()
        {
            if(Texture == ContentManager.Instance.BomberTexture)
            {
                Move(ObjectManager.Instance.Player);
                Attack(ObjectManager.Instance.Player);
                explosionBox.X = (int)this.X - 64;
                explosionBox.Y = (int)this.Y - 64;
            }
            else
            {
                XTransform = 0;
                YTransform = 0;
                Health = 0;
                if(deathTimer >= 1000)
                {
                    Texture = null;
                }
                deathTimer += time.ElapsedGameTime.Milliseconds;
            }
        }

        public override void OnDeath()
        {
            if (deathTimer == 0)
                DoDamage(ObjectManager.Instance.Player);
            else if(deathTimer >= 1000)
                ObjectManager.Instance.Enemies.Remove(this);
        }

        public override void OnCollision(GameObject other)
        {
            if (Health <= 0)
            {
                XTransform = 0;
                YTransform = 0;
                OnDeath();
                return;
            }
            base.OnCollision(other);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(Health > 0)
                base.Draw(spriteBatch);
            else if(deathTimer <= 1000)
                spriteBatch.Draw(this.Texture, new Vector2((int)this.explosionBox.X, (int)this.explosionBox.Y), new Rectangle(0, 0, (int)(this.Texture.Width), (int)(this.Texture.Height)), Color.White, 0.0f, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 1.0f);
        }
    }
}
