using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTKTools;
using System.Drawing;

namespace GOTYE
{
    using Colour4 = OpenTK.Graphics.Color4;
    class Trail : SpaceJunk
    {
        float trailspeed = Star.BaseSpeed;
        static BitmapTexture2D texture;
        static BitmapTexture2D Texture
        {
            get
            {
                if (texture == null)
                {
                    texture = new BitmapTexture2D((Bitmap)Bitmap.FromFile("..\\..\\res\\trail0.png"));
                }
                return texture;
            }
        }
        Vector2 velocity;

        protected override Vector2 Velocity
        {
            get { return velocity; }
        }

        public override float Depth
        {
            get { return 0.6f; }
        }

        public override void Draw(SpriteShader shader)
        {
            Sprite.Scale = Sprite.Scale * 0.95f;
            base.Draw(shader);
        }

        public Trail(Vector2 startpos, float angle, Colour4 colour)
            : base(startpos.X - 32f, startpos.Y, Texture, 0.75f)
        {
            Sprite.Colour = colour;
            Sprite.Rotation = angle;
            velocity = new Vector2
            {
                X = -trailspeed,
                Y = 0
            };
        }
    }
}
