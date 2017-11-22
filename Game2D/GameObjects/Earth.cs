using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceJumper.GameObjects
{
    public class Earth : GameObject
    {
        private float speed;

        public Earth(Vector2 position, Texture2D texture, float textureScale, float speed) : base(position, texture, textureScale)
        {
            this.speed = speed;
        }

        public override void Update()
        {
            position.X -= (speed * SpaceGame.GameSpeed);
        }
    }
}
