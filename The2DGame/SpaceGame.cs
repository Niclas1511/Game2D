using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game2D.GameObjects;
using System;
using System.Collections.Generic;

namespace Game2D
{
    public class SpaceGame : Game
    {
        //static:
        public static float GameSpeed = 1;
        public static float Gravity = 0.9f;
        //Technisch:
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Texturen:
        Texture2D backgroundTile;
        Texture2D earthTexture;
        private SpriteFont Font;
        //Objekte:
        List<GameObject> gameObjects = new List<GameObject>();
        Player player;


        public SpaceGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferWidth = 1000;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Player
            var playerTexture = Content.Load<Texture2D>("playerTexture");
            var playerTextureCoward = Content.Load<Texture2D>("playerTextureCoward");
            int playertTextureScale = 3;
            var playerPos = new Vector2(40, GraphicsDevice.Viewport.Height - playerTexture.Height * playertTextureScale);
            player = new Player(playerPos, playerTexture, playertTextureScale, playertTextureScale, playerTextureCoward);
            //Background
            backgroundTile = Content.Load<Texture2D>("spaceTile");
            earthTexture = Content.Load<Texture2D>("doener2");
            //Font
            Font = Content.Load<SpriteFont>("Courier New");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Neue Objekte erstellen:
            Random random = new Random();
            List<float> possibleY = new List<float>();
            possibleY.Add(GraphicsDevice.Viewport.Height - 0.25f * earthTexture.Height);
            possibleY.Add(GraphicsDevice.Viewport.Height - (player.TextureStanding.Height*3) - 0.25f * earthTexture.Height);
            possibleY.Add(GraphicsDevice.Viewport.Height - ((player.TextureStanding.Height*3) / 2) - 0.25f * earthTexture.Height);
            if (random.Next(70) == 0)
            {
                //das letzte erstelle Objekt hat 800 - 600 = 200 Abstand zum rechten Rand
                if (gameObjects.Count == 0 || gameObjects[gameObjects.Count - 1].Position.X < 600)
                {
                    var startPosition = new Vector2(GraphicsDevice.Viewport.Width, possibleY[random.Next(3)]);//random.Next(GraphicsDevice.Viewport.Height) - 
                    gameObjects.Add(new Earth(startPosition, earthTexture, 2f, 5f));
                }
            }
            //Alles Updaten:
            player.Update();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update();
                //Kollision:
                if (player.GetBoundary.Intersects(gameObjects[i].GetBoundary))
                {
                    if (player.Score > player.Highscore) player.Highscore = player.Score;
                    player.Score = 0;
                    GameSpeed = 1;
                }
                if (gameObjects[i].Position.X < -500)
                {
                    gameObjects.RemoveAt(i);
                    player.Score += 100;
                }
            }
            //Score:
            player.Score+= (int)(1 * SpaceGame.GameSpeed);
            SpaceGame.GameSpeed += 0.005f;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            drawBackground(spriteBatch);
            //Objekte malen
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Draw(spriteBatch);
            }
            player.Draw(spriteBatch);
            //Font schreiben
            spriteBatch.DrawString(Font, "HS: " + player.Highscore.ToString(), new Vector2(40, 0), Color.ForestGreen);
            spriteBatch.DrawString(Font, player.Score.ToString(), new Vector2(40, 70), Color.Firebrick);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private int colorCounter = 0;
        private Color color = Color.White;
        private void drawBackground(SpriteBatch spriteBatch)
        {
            if (colorCounter > 10)
            {
                colorCounter = 0;
                color = Color.White;
            }
            else if (colorCounter > 4)
            {
                color = Color.Red;
            }
            int n = GraphicsDevice.Viewport.Width / backgroundTile.Width + 1;
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    spriteBatch.Draw(backgroundTile, new Vector2(x * backgroundTile.Width, y * backgroundTile.Height), color);
                }
            }
            colorCounter++;
        }
    }
}
