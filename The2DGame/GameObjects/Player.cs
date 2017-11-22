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
        private int score = 0;
        private int highscore = 0;

        private int jumpSpeed = 23;
        private int jumpCounter = 0;
        private int maxJumps = 2;
        private bool jumpKeyWasPressed = false;

        public Player(Vector2 position, Texture2D texture, float textureScale) : base(position, texture, textureScale)
        {

        }

        public override void Update()
        {
            var Keyboardstate = Keyboard.GetState();
            if (IsGrounded())
            {
                jumpKeyWasPressed = false;
                jumpCounter = 0;
                velocity.Y = 0;
                if (Keyboardstate.IsKeyDown(Keys.W) && !jumpKeyWasPressed)
                {
                    Jump();
                }
            }
            else
            {
                if (Keyboardstate.IsKeyUp(Keys.W))
                {
                    jumpKeyWasPressed = false;
                }
                if (Keyboardstate.IsKeyDown(Keys.W) && jumpCounter < maxJumps)
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
    }
}
