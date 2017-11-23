using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2D.GameObjects
{
    public class Player : GameObject
    {
        private Texture2D textureCower;
        private Texture2D textureStanding;

        private int score = 0;
        private int highscore = 0;

        private int jumpSpeed = 23;
        private int jumpCounter = 0;
        private int maxJumps = 2;
        private bool jumpKeyWasPressed = false;
        private bool cowerKeyWasPressed = false;

        public Player(Vector2 position, Texture2D texture, int playertTextureScale, float textureScale, Texture2D textureCower) : base(position, texture, textureScale)
        {
            this.textureCower = textureCower;
            this.textureStanding = texture;
        }

        public override void Update()
        {
            var Keyboardstate = Keyboard.GetState();

            if (Keyboardstate.IsKeyUp(Keys.S) && cowerKeyWasPressed)
            {
                texture = textureStanding;
                position.X += 30;
                cowerKeyWasPressed = false;
            }
            if (IsGrounded() && Keyboardstate.IsKeyDown(Keys.S) && !cowerKeyWasPressed)
            {
                texture = textureCower;
                position.X -= 30;
                cowerKeyWasPressed = true;
            }
            if (IsGrounded())
            {
                jumpKeyWasPressed = false;
                jumpCounter = 0;
                velocity.Y = 0;
                if (Keyboardstate.IsKeyDown(Keys.Space) && !jumpKeyWasPressed && Keyboardstate.IsKeyUp(Keys.S))
                {
                    Jump();
                }
            }
            else
            {
                if (Keyboardstate.IsKeyUp(Keys.Space))
                {
                    jumpKeyWasPressed = false;
                }
                if (Keyboardstate.IsKeyDown(Keys.Space) && jumpCounter < maxJumps)
                {
                    if (jumpKeyWasPressed == false)
                    {
                        Jump();
                    }
                }
                velocity.Y += SpaceGame.Gravity;
            }
            position += velocity;
        }

        private void Jump()
        {
            velocity.Y = -jumpSpeed;
            jumpCounter++;
            jumpKeyWasPressed = true;
        }

        public bool IsGrounded()
        {
            if (position.Y + velocity.Y >= 600 - texture.Height * textureScale)
            {
                position.Y = 600 - texture.Height * textureScale;
                return true;
            }
            else
                return false;
        }

        public int Score { get => score; set => score = value; }
        public int Highscore { get => highscore; set => highscore = value; }
        public Texture2D TextureStanding { get => textureStanding; }
    }
}
