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
    class SpaceFlare : SpaceJunk
    {
        const float pewspeed = Star.BaseSpeed;
        static BitmapTexture2D texture;
        static BitmapTexture2D Texture
        {
            get
            {
                if (texture == null)
                {
                    texture = new BitmapTexture2D((Bitmap)Bitmap.FromFile("..\\..\\res\\pew1.png"));
                }
                return texture;
            }
        }
        Vector2 velocity;

        bool hashit;

        public override Vector2 Velocity
        {
            get { return velocity; }
        }

        public override float Depth
        {
            get { return 0.5f; }
        }

        public SpaceFlare(Vector2 startpos, float angle, Colour4 colour)
            : base(startpos.X, startpos.Y, Texture, 0.75f)
        {
            Sprite.Colour = colour;
            Sprite.Rotation = angle;
            velocity = new Vector2
            {
                X = (float)Math.Cos(angle) * pewspeed,
                Y = (float)Math.Sin(angle) * pewspeed
            };
            hashit = false;
        }

        public override bool ShouldRemove(Rectangle bounds)
        {
            return hashit || base.ShouldRemove(bounds);
        }

        public override void Update(IEnumerable<SpaceJunk> junkage)
        {
            Vector2 oldPos = Position;

            if (hashit) return;

            base.Update(junkage);
            foreach (SpaceJunk junk in junkage)
            {
                Vector2 hit;
                if (junk is Roid && junk.IsHit(oldPos + junk.Velocity, Position, out hit))
                {
                    ((Damagable)junk).Damage(12, hit, velocity / 32);
                    Sprite.Position = hit;
                    velocity = new Vector2();
                    return;
                }
            }
        }
    }
}
