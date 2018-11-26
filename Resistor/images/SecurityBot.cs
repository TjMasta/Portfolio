using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1
{
    class SecurityBot : Enemy
    {
        private float attackDistance;

        private float projectileSpeed;

        public float AttackDistance { get { return attackDistance; } set { attackDistance = value; } }
        public float ProjectileSpeed { get { return projectileSpeed; } set { projectileSpeed = value; } }

        public SecurityBot(float posX, float posY)
            : this(30, 5f, 35, posX, posY)
        {
        }

        public SecurityBot(float health, float damage, float attackColdown, float posX, float posY)
            : base(health, damage, attackColdown, posX, posY)
        {
            Hitbox = new Rectangle((int)posX, (int)posY, 64, 64);
            attackDistance = 320;
            projectileSpeed = 7;
            Texture = ContentManager.Instance.SecurityBotTexture;
            speed = 1;
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

        public override void Attack(Player p)
        {
            float distance = (float)Math.Sqrt(Math.Pow(this.X - p.X, 2) + Math.Pow(this.Y - p.Y, 2));
            if(distance <= attackDistance)
            {
                Projectile pro = new Projectile((int)(this.Hitbox.Center.X), (int)(this.Hitbox.Center.Y), (p.X - this.X) / distance, (p.Y - this.Y) / distance, projectileSpeed);
                pro.Type = GameObjectType.EnemyProjectile;
                pro.Texture = ContentManager.Instance.EnemyProjectileTexture;
                ObjectManager.Instance.EnemyProjectiles.Add(pro);
                Cooldown = AttackCooldown;
            }

        }

        public override void Move(Player p)
        {
            int distance = (int)Math.Sqrt(Math.Pow(this.X - p.X, 2) + Math.Pow(this.Y - p.Y, 2));

            XTransform = 0;
            YTransform = 0;

            if (IsActive)
            {
                if (distance > attackDistance)
                {
                    attackDistance = 320;
                    base.Move(p);
                }
                /*else
                {
                    attackDistance = 384;
                    if (p.X - this.X >= 0 && p.Y - this.Y > 0)
                    {
                        YTransform--;
                        XTransform++;
                    }
                    else if (p.X - this.X >= 0 && p.Y - this.Y <= 0)
                    {
                        YTransform--;
                        XTransform--;
                    }
                    else if (p.X - this.X < 0 && p.Y - this.Y > 0)
                    {
                        YTransform++;
                        XTransform++;
                    }
                    else if (p.X - this.X < 0 && p.Y - this.Y <= 0)
                    {
                        YTransform++;
                        XTransform--;
                    }
                }*/
            }
        }
    }
}
