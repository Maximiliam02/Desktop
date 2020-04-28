using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Template
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch SpriteBatch;

        public Texture2D Skepp, Mynt, Bakgrund, Asteroid;

        Vector2 SkeppPos = new Vector2(100, 300), BakgrundPos, SkeppStart, SkeppFart;

        Rectangle rece = new Rectangle();
        Rectangle rec = new Rectangle();

        float Rotera, RoteraFart = 5f, Friktion = 0.1f;

        const float Hastighet = 5f;

        int screenwidth, screenheight, AsteroidSpawnPos = 0, AsteroidTimer = 0, MyntTimer = 0, SpawnRateAsteroid = 250, SpawnRateMynt = 500, MyntPos;

        private List<Vector2> RandomAsteroidSpawn = new List<Vector2>();
        private List<Vector2> RandomMyntSpawn = new List<Vector2>();

        Rectangle SkeppRektangle;

        private Random Random = new Random();
        private Random rnd = new Random();





        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }



        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            Bakgrund = Content.Load<Texture2D>("Bakgrund");
            Skepp = Content.Load<Texture2D>("Skepp");
            Asteroid = Content.Load<Texture2D>("Asteroid");
            Mynt = Content.Load<Texture2D>("Mynt");

            screenwidth = GraphicsDevice.Viewport.Width;
            screenheight = GraphicsDevice.Viewport.Height;
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>

        protected override void Update(GameTime gameTime)
        {
            KeyboardState kNewstate = Keyboard.GetState();
            KeyboardState kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.Right))
                Rotera += MathHelper.ToRadians(RoteraFart);
            if (kstate.IsKeyDown(Keys.Left))
                Rotera -= MathHelper.ToRadians(RoteraFart);
            if (kstate.IsKeyDown(Keys.Up))
            {
                SkeppFart.Y = (float)Math.Sin(Rotera) * Hastighet;
                SkeppFart.X = (float)Math.Cos(Rotera) * Hastighet;
            }

            else if (SkeppFart != null)
            {
                float a = SkeppFart.X, b = SkeppFart.Y;

                SkeppFart.Y = b -= Friktion * b;
                SkeppFart.X = a -= Friktion * a;
            }

            SkeppPos += SkeppFart;

            SkeppRektangle = new Rectangle((int)SkeppPos.X, (int)SkeppPos.Y, Skepp.Width, Skepp.Height);
            SkeppStart = new Vector2(SkeppRektangle.Width / 2, SkeppRektangle.Height / 2);

            if (SpawnRateAsteroid > 15)//Om Spawnrate är över 15 så kommer den minska med 5 var 10 sekund, 10s = Asteroidtimer 600
            {
                AsteroidTimer++;
                if (AsteroidTimer == 120)
                {
                    SpawnRateAsteroid -= 5;
                    AsteroidTimer = 0;
                }
            }


            if (SpawnRateAsteroid <= 15 && SpawnRateAsteroid > 5)//Om Spawnrate är 5<x<15 så kommer den minska med 1 var 10 sekund, 10s = Asteroidtimer 600
            {
                AsteroidTimer++;
                if (AsteroidTimer == 120)
                {
                    SpawnRateAsteroid -= 1;
                    AsteroidTimer = 0;
                }
            }

            if (SpawnRateMynt > 5)
            {
                MyntTimer++;
                if (MyntTimer == 10)
                {
                    SpawnRateMynt -= 1;
                    MyntTimer = 0;
                }
            }


            if (SpawnRateMynt <= 1 && SpawnRateMynt > 1)
            {
                MyntTimer++;
                if (MyntTimer == 10)
                {
                    SpawnRateMynt -= 1;
                    MyntTimer = 0;
                }
            }


            MyntPos = rnd.Next(100, screenwidth);
            AsteroidSpawnPos = rnd.Next(10, screenwidth);


            if (rnd.Next(0, SpawnRateAsteroid) == 0)
            {
                RandomAsteroidSpawn.Add(new Vector2(AsteroidSpawnPos, 0));
            }
            for (int i = 0; i < RandomAsteroidSpawn.Count; i++)
            {
                RandomAsteroidSpawn[i] = RandomAsteroidSpawn[i] - new Vector2(0, -2);
            }

            if (Random.Next(0, SpawnRateMynt) == 0)
            {
                RandomMyntSpawn.Add(new Vector2(Random.Next(10, 900), Random.Next(30, 450)));
            }

           
            if (SkeppRektangle.Intersects(rec))
            {
                Exit();
            }

            if (SkeppRektangle.Intersects(rece))
            {
            }


            base.Update(gameTime);
        }





        // TODO: Add your update logic here
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            graphics.ApplyChanges();
            SpriteBatch.Begin();
            SpriteBatch.Draw(Bakgrund, BakgrundPos, Color.White);
            SpriteBatch.Draw(Skepp, SkeppPos, null, Color.White, Rotera, SkeppStart, 1, SpriteEffects.None, 0f);
            SpriteBatch.Draw(Mynt, new Rectangle(screenheight, screenwidth, 0, 0), Color.White);
            SpriteBatch.Draw(Asteroid, new Rectangle(0, 0, 0, 0), Color.White);

            foreach (Vector2 RandomAsteroidSpawn in RandomAsteroidSpawn)
            { 
                rec.Location = RandomAsteroidSpawn.ToPoint();
                rec.Size = new Point(100, 100);
                SpriteBatch.Draw(Asteroid, rec, Color.White);
            }

            foreach (Vector2 RandomMyntSpawn in RandomMyntSpawn)
            {
                rece.Location = RandomMyntSpawn.ToPoint();
                rece.Size = new Point(80, 80);
                SpriteBatch.Draw(Mynt, rece, Color.White);

            }









            SpriteBatch.End();


            List<Vector2> temp3 = new List<Vector2>();
            foreach (var item in RandomAsteroidSpawn)
            {
                if (item.Y <= screenheight)
                {
                    temp3.Add(item);
                }
            }

            RandomAsteroidSpawn = temp3;


        }
    }



}