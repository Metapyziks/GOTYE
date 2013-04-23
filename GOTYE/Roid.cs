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
            "roid1"
            
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

        float rotspeed;

        private Vector2 velocity;

        protected override Vector2 Velocity
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
            velocity = new Vector2
            {
                X = -(8 + (float) Math.Sqrt(scenenumber)) / Sprite.Scale.X,
                Y = 0
            };
            rotspeed = Program.Rand.NextSingle() * MathHelper.Pi / 10 - MathHelper.Pi / 20;
            byte clr = (byte) Program.Rand.Next(32, 128);
            Sprite.Colour = new Color4(clr, clr, clr, 255);
        }

        private Roid(Vector2 pos, Vector2 vel, float scale)
            : base(pos.X, pos.Y, Textures[Program.Rand.Next(Textures.Length)], scale)
        {
            HP = (int)(Sprite.Scale.X * 50);
            velocity = vel;
            rotspeed = Program.Rand.NextSingle() * MathHelper.Pi / 10 - MathHelper.Pi / 20;
            byte clr = (byte) Program.Rand.Next(32, 128);
            Sprite.Colour = new Color4(clr, clr, clr, 255);
        }

        public void Push(Vector2 impulse)
        {
            velocity += impulse;
        }

        protected override void OnDamaged(int amount, Vector2 force)
        {
            velocity = velocity + (force / Sprite.Scale.X);
            //Sprite.Colour = Color4.Orange;
        }

        protected override void OnKilled()
        {
            Sprite.Colour = Color4.Red;
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
                Scene.AddJunk(new Roid(Sprite.Position + offset * edgedist, Velocity + offset * Program.Rand.NextSingle() * 2, newscale));
            }            
        }

        public override bool IsHit(Vector2 pos)
        {
            float radius = Sprite.Width / 2;
            float distance = (pos - Sprite.Position).Length;
            return radius > distance;
        }

        public override void Update(IEnumerable<SpaceJunk> junkage)
        {
            base.Update(junkage);
            Sprite.Rotation = Sprite.Rotation + rotspeed;
        }
    }
}
