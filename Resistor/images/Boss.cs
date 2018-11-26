using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game1
{
    class Boss : Enemy
    {
        private GameTime time;
        private float timer;
        private float moveTimer;
        private List<Enemy> minions;
        private Random attackPicker;
        private float projectileSpeed;
        private bool chargeAttack;
        private Rectangle zapZone;
        private bool zapHit;

        public GameTime Time { get { return time; } set { time = value; } }

        public List<Enemy> Minions { get { return minions; } }

        public Boss(float posX, float posY, GameTime time)
            : this(250, 20, 25, posX, posY, time)
        {

        }

        public Boss(float health, float damage, float attackCooldown, float posX, float posY, GameTime time)
            : base(health, damage, attackCooldown, posX, posY) //2752, 3264
        {
            Hitbox = new Rectangle((int)posX, (int)posY, 128, 128);
            this.time = time;
            timer = 10000;
            moveTimer = 0;
            minions = new List<Enemy>();
            attackPicker = new Random();
            projectileSpeed = 5;
            Texture = ContentManager.Instance.MrRobotoTexture;
            chargeAttack = false;
            zapHit = false;
        }

        public override void Move(Player p)
        {
            YTransform = 0;
            XTransform = 0;
            if(IsActive)
            {
                if (moveTimer <= 8000)
                {
                    YTransform++;
                }
                else if (moveTimer <= 16500)
                {
                    XTransform--;
                }
                else if (moveTimer <= 24500)
                {
                    YTransform--;
                }
                else if (moveTimer <= 33000)
                {
                    XTransform++;
                }
                else
                {
                    moveTimer = 0;
                }
            }
        }

        public override void Attack(Player p)
        {
            if(IsActive)
            {
                if(minions.Count == 0)
                {
                    if(timer >= 3250)
                    {
                        if(Health <= 150)
                        {
                            SecurityBot w = new SecurityBot(this.Hitbox.Center.X + 64, this.Hitbox.Center.Y - 32);
                            SecurityBot s = new SecurityBot(this.Hitbox.Center.X - 128, this.Hitbox.Center.Y - 32);

                            w.AttackDistance = 1080;
                            w.ProjectileSpeed = 7;
                            s.ProjectileSpeed = 7;
                            s.AttackDistance = 1080;
                            w.MaxHealth = 50;
                            w.Health = 50;
                            s.MaxHealth = 50;
                            s.Health = 50;
                            w.AttackCooldown = 13;
                            s.AttackCooldown = 13;

                            minions.Add(w);
                            minions.Add(s);

                            if (p.Health + 20 >= p.MaxHealth)
                            {
                                p.Health = p.MaxHealth;
                            }
                            else
                            {
                                p.Health += 10;
                            }
                        }
                        else
                        {
                            Welder w = new Welder(this.Hitbox.Center.X + 64, this.Hitbox.Center.Y - 32);
                            Welder s = new Welder(this.Hitbox.Center.X - 128, this.Hitbox.Center.Y - 32);

                            w.MaxHealth = 70;
                            w.Health = 70;
                            s.MaxHealth = 70;
                            s.Health = 70;
                            w.AttackCooldown = 10;
                            s.AttackCooldown = 10;

                            minions.Add(w);
                            minions.Add(s);

                            if (p.Health + 20 >= p.MaxHealth)
                            {
                                p.Health = p.MaxHealth;
                            }
                            else
                            {
                                p.Health += 10;
                            }
                        }
                        timer = 0;
                    }
                }
                else
                {
                    if(timer >= AttackCooldown)
                    {
                        if(attackPicker.Next(0,100) < 75)
                        {
                            PrimaryAttack(attackPicker.Next(4, 7));
                        }
                        else
                        {
                            chargeAttack = true;
                        }
                    }
                }
            }
        }

        private void PrimaryAttack(float projectiles)
        {
            AttackCooldown = 1000;
            for (int i = 0; i < projectiles; i++)
            {
                Projectile pro = new Projectile((int)(this.Hitbox.Center.X), (int)(this.Hitbox.Center.Y), (i - projectiles) / projectiles, i / projectiles, projectileSpeed / attackPicker.Next(1, (int)projectiles), attackPicker.Next(100, 300));
                pro.Type = GameObjectType.EnemyProjectile;
                pro.Texture = ContentManager.Instance.EnemyProjectileTexture;
                ObjectManager.Instance.EnemyProjectiles.Add(pro);
            }
            for (int i = 0; i < projectiles; i++)
            {
                Projectile pro = new Projectile((int)(this.Hitbox.Center.X), (int)(this.Hitbox.Center.Y), (projectiles - i) / projectiles, i / projectiles, projectileSpeed / attackPicker.Next(1, (int)projectiles), attackPicker.Next(100, 300));
                pro.Type = GameObjectType.EnemyProjectile;
                pro.Texture = ContentManager.Instance.EnemyProjectileTexture;
                ObjectManager.Instance.EnemyProjectiles.Add(pro);
            }
            for (int i = 0; i < projectiles; i++)
            {
                Projectile pro = new Projectile((int)(this.Hitbox.Center.X), (int)(this.Hitbox.Center.Y), -i / projectiles, (i - projectiles) / projectiles, projectileSpeed / attackPicker.Next(1, (int)projectiles), attackPicker.Next(100, 300));
                pro.Type = GameObjectType.EnemyProjectile;
                pro.Texture = ContentManager.Instance.EnemyProjectileTexture;
                ObjectManager.Instance.EnemyProjectiles.Add(pro);
            }
            for (int i = 0; i < projectiles; i++)
            {
                Projectile pro = new Projectile((int)(this.Hitbox.Center.X), (int)(this.Hitbox.Center.Y), i / projectiles, (i - projectiles) / projectiles, projectileSpeed / attackPicker.Next(1, (int)projectiles), attackPicker.Next(100, 300));
                pro.Type = GameObjectType.EnemyProjectile;
                pro.Texture = ContentManager.Instance.EnemyProjectileTexture;
                ObjectManager.Instance.EnemyProjectiles.Add(pro);
            }
            timer = 0;
        }

        private void ChargeAttack(Player p)
        {
            AttackCooldown = 3000;
            if(timer <= 2050)
                zapZone = new Rectangle(p.Hitbox.Center.X, p.Hitbox.Center.Y, 64, 64);

            if(timer >= 2400)
            {
                if (p.Hitbox.Intersects(zapZone) && !zapHit)
                {
                    p.Health -= this.Damage;
                    zapHit = true;
                }
            }

            if(timer >= 2900)
            {
                chargeAttack = false;
                zapHit = false;
                AttackCooldown = 1500;
                timer = 0;
            }
        }

        public override void Update()
        {
            if(IsActive)
            {
                timer += time.ElapsedGameTime.Milliseconds;
                moveTimer += time.ElapsedGameTime.Milliseconds;
                Move(ObjectManager.Instance.Player);
                Attack(ObjectManager.Instance.Player);
                if (chargeAttack)
                    ChargeAttack(ObjectManager.Instance.Player);

                for (int i = 0; i < minions.Count; i++)
                {
                    minions[i].XTransform = this.XTransform;
                    minions[i].YTransform = this.YTransform;

                    if (minions[i].Health <= 0)
                        minions.Remove(minions[i]);
                }

                foreach (Enemy e in minions)
                {
                    if (e.Cooldown <= 0)
                    {
                        e.Attack(ObjectManager.Instance.Player);
                    }
                    else
                    {
                        e.Cooldown = e.Cooldown - 1;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(chargeAttack)
            {
                Texture = ContentManager.Instance.MrRobotoZapModeTexture;
                if (timer >= 2400)
                {
                    spriteBatch.Draw(ContentManager.Instance.MrRobotoZapAttackTexture, new Rectangle(zapZone.X, zapZone.Y, zapZone.Width, zapZone.Height), Color.White);
                    Texture = ContentManager.Instance.MrRobotoTexture;
                }
            }

            foreach (Enemy e in minions)
                e.Draw(spriteBatch);

            if(minions.Count == 0)
            {
                spriteBatch.Draw(this.Texture, new Vector2((int)this.X, (int)this.Y), new Rectangle(0, 0, (int)(this.Texture.Width), (int)(this.Texture.Height)), Color.White, 0.0f, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 1.0f);
            }
            else
            {
                spriteBatch.Draw(this.Texture, new Vector2((int)this.X, (int)this.Y), new Rectangle(0, 0, (int)(this.Texture.Width), (int)(this.Texture.Height)), Color.HotPink, 0.0f, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 1.0f);
            }
        }

        public override void OnCollision(GameObject other)
        {
            if(minions.Count == 0)
            {
                if (other.Type is GameObjectType.Projectile)
                {
                    Health -= ObjectManager.Instance.Player.Damage;
                    if (this.Health <= 0)
                    {
                        OnDeath();
                    }
                    
                }
            }

            if(other is Player)
            {
                other.XTransform = 0;
                other.YTransform = 0;
            }

            if(other.Type is GameObjectType.Projectile)
            {
                ObjectManager.Instance.Projectiles.Remove((Projectile)other);
            }
        }

        public override void OnDeath()
        {
            ObjectManager.Instance.TheBoss = null;
        }
    }
}
