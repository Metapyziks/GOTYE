using OpenTKTools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace GOTYE
{
    using Colour4 = OpenTK.Graphics.Color4;
    class SpaceShip: SpaceJunk
    {
        static BitmapTexture2D texture;
        static BitmapTexture2D Texture
        {
            get
            {
                if (texture == null)
                {
                    texture = new BitmapTexture2D((Bitmap)Bitmap.FromFile("..\\..\\res\\gotyeship.png"));
                }
                return texture;
            }
        }

        double nextpewtime;

        Vector2 velocity;
        protected override Vector2 Velocity
        {
            get { return velocity; }
        }

        public override float Depth
        {
            get { return 0; }
        }

        public SpaceShip(Vector2 startpos)
            : this(startpos, Colour4.Pink)
        {

        }

        public SpaceShip(Vector2 startpos, Colour4 colour)
            : base(startpos.X, startpos.Y, Texture, 0.5f)
        {

        }

        public override void Update(IEnumerable<SpaceJunk> junkage)
        {
            if (Program.MouseDevice[OpenTK.Input.MouseButton.Left])
            {
                if (Scene.CurrentTime() > nextpewtime)
                {
                    nextpewtime = Scene.CurrentTime() + 1.0;
                    Scene.AddJunk(new SpaceFlare(Sprite.Position, Sprite.Rotation - MathHelper.Pi / 32f, Colour4.Red));
                    Scene.AddJunk(new SpaceFlare(Sprite.Position, Sprite.Rotation - MathHelper.Pi / 64f, Colour4.Red));
                    Scene.AddJunk(new SpaceFlare(Sprite.Position, Sprite.Rotation, Colour4.Red));
                    Scene.AddJunk(new SpaceFlare(Sprite.Position, Sprite.Rotation + MathHelper.Pi / 64f, Colour4.Red));
                    Scene.AddJunk(new SpaceFlare(Sprite.Position, Sprite.Rotation + MathHelper.Pi / 32f, Colour4.Red));
                }

            }

            //foreach (var junk in junkage) {
            //    if (junk is Roid) {
            //        var force = Position - junk.Position;
            //        var mag = force.Length;
            //        force.Normalize();
            //        force = force / (mag * junk.Scale * junk.Scale);
            //        ((Roid) junk).Push(force);
            //    }
            //}

            Scene.AddJunk(new Trail(Sprite.Position, Sprite.Rotation, Color.FromArgb(70, Color.RoyalBlue)));

            velocity.X = (Program.MouseDevice.X - Sprite.X) * 0.1f;
            velocity.Y = (Program.MouseDevice.Y - Sprite.Y) * 0.1f;
            Sprite.Rotation = (float)Math.Atan2(velocity.Y, Star.BaseSpeed);
            base.Update(junkage);
        }

        public override bool ShouldRemove(Rectangle bounds)
        {
            return false;
        }

    }
}
