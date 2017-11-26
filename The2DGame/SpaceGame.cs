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
        public static float gameSpeed = 1;
        public static float gravity = 1.2f;

        //Technisch:
        Random random = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        //Texturen:
        Texture2D backgroundTile;
        List<Texture2D> obstacleTextures = new List<Texture2D>();
        private SpriteFont font;
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
            Texture2D playerTextureStanding = Content.Load<Texture2D>("playerTexture");
            Texture2D playerTextureCoward = Content.Load<Texture2D>("playerTextureCoward");
            int playerTexureScale = 3;
            Vector2 playerPos = new Vector2(40, GraphicsDevice.Viewport.Height - playerTextureStanding.Height * playerTexureScale);
            player = new Player(playerPos, playerTextureStanding, playerTexureScale, playerTextureCoward);
            backgroundTile = Content.Load<Texture2D>("spaceTile");
            font = Content.Load<SpriteFont>("Courier New");
            obstacleTextures.Add(Content.Load<Texture2D>("asteroid"));
            obstacleTextures.Add(Content.Load<Texture2D>("penis")); 
        }

        private Obstacle createRandomObstacle(Texture2D textureObstacle, float speed)
        {
            Viewport screen = GraphicsDevice.Viewport;
            float textureScale = ((player.TextureStanding.Height * player.TextureScale) / 2) / textureObstacle.Height;
            float[] possibleY = new float[3];
            for (int i = 0; i < possibleY.Length; i++)
            {
                if (i == 0)
                    possibleY[i] = screen.Height - textureScale * textureObstacle.Height;
                else
                    possibleY[i] = screen.Height - player.TextureStanding.Height * player.TextureScale - textureScale* textureObstacle.Height * (i - 1);
            }
            Vector2 position = new Vector2(screen.Width, possibleY[random.Next(possibleY.Length)]);
            return new Obstacle(position, textureObstacle, textureScale, speed);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            //Neue Objekte erstellen:
            if (random.Next(60) == 0)
            {
                if (gameObjects.Count == 0 || gameObjects[gameObjects.Count - 1].Position.X < 600)
                {
                    gameObjects.Add(createRandomObstacle(obstacleTextures[random.Next(obstacleTextures.Count)], 6f));
                }
            }
            //Alles Updaten:
            player.Update();
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Update();
                if (gameObjects[i].Position.X < -200)
                {
                    gameObjects.RemoveAt(i);
                    continue;
                }
                //Kollision:
                if (player.GetBoundary.Intersects(gameObjects[i].GetBoundary))
                {
                    if (player.Score > player.Highscore) player.Highscore = player.Score;
                    player.Score = 0;
                    gameSpeed = 1;
                }
            }
            //Score:
            player.Score += (int)(1 * SpaceGame.gameSpeed);
            SpaceGame.gameSpeed += 0.002f;
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
            spriteBatch.DrawString(font, "HS: " + player.Highscore.ToString(), new Vector2(40, 0), Color.ForestGreen);
            spriteBatch.DrawString(font, player.Score.ToString(), new Vector2(40, 70), Color.Firebrick);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void drawBackground(SpriteBatch spriteBatch)
        {
            int n = GraphicsDevice.Viewport.Width / backgroundTile.Width + 1;
            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    spriteBatch.Draw(backgroundTile, new Vector2(x * backgroundTile.Width, y * backgroundTile.Height), Color.LightYellow);
                }
            }
        }
    }
}
