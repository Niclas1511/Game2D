using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2D.GameObjects
{
    public class Obstacle : GameObject
    {
        private float speed;

        public Obstacle(Vector2 position, Texture2D texture, float textureScale, float speed) : base(position, texture, textureScale)
        {
            this.Speed = speed;
        }

        public float Speed { get => speed; set => speed = value; }

        public override void Update()
        {
            position.X -= (Speed * SpaceGame.gameSpeed);
        }
    }
}
