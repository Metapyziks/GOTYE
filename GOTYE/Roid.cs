using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKTools;
using OpenTK.Graphics;
using System.Drawing;

namespace GOTYE
{
    class Roid: Damagable
    {
        static String[] texturenames = new[]
        {
            "roid4",
            "roid5"            
        };

        static BitmapTexture2D[] textures;
        static BitmapTexture2D[] Textures
        {
            get
            {
                if (textures == null)
                {
                    textures = texturenames.Select(name => new BitmapTexture2D((Bitmap)Bitmap.FromFile("..\\..\\res\\" + name + ".png"))).ToArray();
                }
                return textures;
            }
        }

        int colourmin = 80;
        int colourmax = 150;

        float rotspeed;

        private Vector2 velocity;

        public override Vector2 Velocity
        {
            get
            {
                return velocity;
            }
        }

        public override float Depth
        {
            get { return 1; }
        }
        
        public Roid(float x, float miny, float maxy, int scenenumber)
            : base(x, miny, maxy, Textures[Program.Rand.Next(Textures.Length)], Program.Rand.NextSingle() * 1.75f + 0.25f) 
        {
            HP = (int)(Sprite.Scale.X * 50);
            Sprite.X = Sprite.X + Sprite.Width;
            int roidcolour = Program.Rand.Next(colourmin, colourmax);
            velocity = new Vector2
            {
                X = -(8 + (float) Math.Sqrt(scenenumber)) / Sprite.Scale.X,
                Y = 0
            };
            rotspeed = Program.Rand.NextSingle() * MathHelper.Pi / 10 - MathHelper.Pi / 20;
            Sprite.Colour = Color.FromArgb(roidcolour, roidcolour, roidcolour);
            
        }

        private Roid(Vector2 pos, Vector2 vel, Color4 roidcolour, float scale)
            : base(pos.X, pos.Y, Textures[Program.Rand.Next(Textures.Length)], scale)
        {
            HP = (int)(Sprite.Scale.X * 50);
            velocity = vel;
            rotspeed = Program.Rand.NextSingle() * MathHelper.Pi / 10 - MathHelper.Pi / 20;
            Sprite.Colour = roidcolour;
            Sprite.Colour = new Color4(clr, clr, clr, 255);
        }

        public void Push(Vector2 impulse)
        {
            velocity += impulse;
        }

        protected override void OnDamaged(int amount, Vector2 hitpos, Vector2 force)
        {
            Vector2 normal = Position - hitpos;
            normal.Normalize();

            Vector2 tangent = new Vector2(-normal.Y, normal.X);

            Vector2 forceNormal = force;
            forceNormal.Normalize();

            velocity = velocity + force * (float) Math.Abs(Vector2.Dot(forceNormal, normal)) / Sprite.Scale.X;
            rotspeed = rotspeed - force.Length * Vector2.Dot(forceNormal, tangent) / (Sprite.Scale.X * MathHelper.Pi * 16f);
        }

        protected override void OnKilled()
        {
            //Sprite.Colour = Color4.Red;
            int pieces = Program.Rand.Next(3,7);
            float newscale = Sprite.Scale.X / (float)Math.Sqrt(pieces);
            float edgedist = (Sprite.Scale.X - newscale) * Sprite.Texture.Width / 3f;
            for (int i = 0; i < pieces; ++i)
            {
                double ang = (i * Math.PI * 2 / pieces) + (Program.Rand.NextDouble() * Math.PI / pieces) * 2;
                Vector2 offset = new Vector2 
                {
                    X = (float)Math.Cos(ang),
                    Y = (float)Math.Sin(ang)
                };
                Scene.AddJunk(new Roid(Sprite.Position + offset * edgedist, Velocity + offset * Program.Rand.NextSingle() * 2, Sprite.Colour, newscale));
            }            
        }

        public override bool IsHit(Vector2 start, Vector2 end, out Vector2 hit)
        {
            hit = new Vector2();
            double r = Sprite.Width / 2.0;

            Vector2d ba = (Vector2d) (end - start);
            Vector2d ca = (Vector2d) (Position - start);

            double a = ba.X * ba.X + ba.Y * ba.Y;
            double bBy2 = ba.X * ca.X + ba.Y * ca.Y;
            double c = ca.X * ca.X + ca.Y * ca.Y - r * r;

            double pBy2 = bBy2 / a;
            double q = c / a;

            double disc = pBy2 * pBy2 - q;
            if (disc < 0) {
                return false;
            }

            double tmpSqrt = Math.Sqrt(disc);
            double abScalingFactor1 = -pBy2 + tmpSqrt;
            double abScalingFactor2 = -pBy2 - tmpSqrt;

            Vector2d p1 = new Vector2d(start.X - ba.X * abScalingFactor1, start.Y - ba.Y * abScalingFactor1);
            Vector2d p2 = new Vector2d(start.X - ba.X * abScalingFactor2, start.Y - ba.Y * abScalingFactor2);

            double t1 = (p1.X - start.X) / ba.X;
            double t2 = (p2.X - start.X) / ba.X;

            if (t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1) {
                hit = (p1.X < p2.X) ? (Vector2) p1 : (Vector2) p2;
                return true;
            } else if (t1 >= 0 && t1 <= 1) {
                hit = (Vector2) p1;
                return true;
            } else if (t2 >= 0 && t2 <= 1) {
                hit = (Vector2) p2;
                return true;
            } else {
                return false;
            }
        }

        public override void Update(IEnumerable<SpaceJunk> junkage)
        {
            base.Update(junkage);
            Sprite.Rotation = Sprite.Rotation + rotspeed;
        }
    }
}
