﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2D.GameObjects
{
    public abstract class GameObject
    {
        protected Vector2 position;
        protected Texture2D texture;
        protected float textureScale;
        protected Vector2 velocity = new Vector2(0, 0);

        protected GameObject(Vector2 position, Texture2D texture, float textureScale)
        {
            this.position = position;
            this.texture = texture;
            this.textureScale = textureScale;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var rectangle = new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * TextureScale), (int)(texture.Height * TextureScale));
            spriteBatch.Draw(texture, rectangle, Color.White);
        }

        public abstract void Update();

        public Vector2 Position { get => position; set => position = value; }
        public Rectangle GetBoundary => new Rectangle((int)position.X, (int)position.Y, (int)(texture.Width * TextureScale), (int)(texture.Height * TextureScale));

        public float TextureScale { get => textureScale; }
    }
}
