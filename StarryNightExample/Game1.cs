using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Media;

namespace StarryNightExample
{
    public class Game1 : Game
    {
        //standard MonoGame attributes
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //textures we need - stars and ground
        private Texture2D _starSprite, _groundSprite;
        
        //this is how we'll keep track of all those stars
        private List<Star> _stars;

        //RNG makes everything coooooool, and frustrating, but coooooooooool.
        private Random _rng;

        //a variable we need for the spacebar press
        private bool spacebarDown = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            //I added these three lines so that I could have a defined screen size
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();
            
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //create a list of stars to manage them all, list starts out empty
            _stars = new List<Star>();
            //create a random object for the fun
            _rng = new Random();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //load up our two sprites
            _starSprite = Content.Load<Texture2D>("star");
            _groundSprite = Content.Load<Texture2D>("ground");

            //call this method (see below) to add our first 100 stars
            AddStars(100);
        }

        protected override void Update(GameTime gameTime)
        {
            //MonoGame standard line
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //This is how we process all the stars in the list - for 
            //each star in the list...
            for(int i = 0; i < _stars.Count; i++)
            {
                _stars[i].Update(); //call the individual star's update method
                if (_stars[i].HasExpired()) //if the star's lifetime is up
                    _stars.RemoveAt(i);     //remove the star from the list
            }

            //pressing a key once is not a trivial thing. Default for MonoGame is to register
            //a sustained press. This is the logice for a single press.
            if (Keyboard.GetState().IsKeyDown(Keys.Space) && !spacebarDown)
            {
                spacebarDown = true;
                AddStars(_rng.Next(10,51));  //pressed once, add 10-51 stars (see below)
            }
            if (Keyboard.GetState().IsKeyUp(Keys.Space))
            {
                spacebarDown = false;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            //see method below
            DrawGround(_groundSprite);

            //for each star in the list, call the star's draw method
            foreach (Star star in _stars)
            {
                star.Draw(_spriteBatch);
            }

            base.Draw(gameTime);
        }

        //simple method to draw the sprites for the ground.
        private void DrawGround(Texture2D groundSprite)
        {
            _spriteBatch.Begin();
            int startX = 0;
            int width = 99;
            for(int i = 0; i < 9; i++)
            {
                _spriteBatch.Draw(groundSprite, new Vector2(startX + i * width, 520), Color.Gray);
            }
            _spriteBatch.End();
        }

        //all this method does is set up all the random values, create a star by calling the 
        //star constructor, then adds it to the list of stars
        private void AddStars(int numStars)
        {
            //for the number of stars that I want...
            for (int i = 0; i < numStars; i++)
            {
                float rndX = _rng.Next(0, 800);     //random x coordinate
                float rndY = _rng.Next(0, 500);     //random y coordinate
                float rndAngle = (float)_rng.NextDouble();  //random start angle between 0 and 1
                float rndRotation = _rng.Next(-100, 100) / 2000.0f;     //how much to add or subtract for rotation each step?
                float rndScale = (float)(_rng.NextDouble() * 2) + 1;    //scale factor
                int rndLifeTime = _rng.Next(250, 1500);                 //how many steps to live for
                Color rndColor = new Color(_rng.Next(0, 255), _rng.Next(0, 255), _rng.Next(0, 255));    //random colour
                float rndAlpha = (float)_rng.NextDouble() + 0.25f;  //random transparency

                //phew, call the constructor
                _stars.Add(new Star(rndX, rndY, rndAngle, rndRotation, rndScale, rndLifeTime, rndColor, rndAlpha, _starSprite));
            }
        }
    }
}