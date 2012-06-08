using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Spartonne_Windows.ParticleEngine;
using Spartonne_Windows.AnimateSprite;

namespace Spartonne_Windows
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<ParticleEngine.ParticleEngine> particleEngine = new List<Spartonne_Windows.ParticleEngine.ParticleEngine>();
        List<Texture2D> textures = new List<Texture2D>();
        private AnimatedSprite animatedSprite;
        MouseState ms;
        Vector2 position = new Vector2(400, 200);
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

            // Particles
            textures.Add(Content.Load<Texture2D>("Particle/circle"));
            textures.Add(Content.Load<Texture2D>("Particle/star"));
            textures.Add(Content.Load<Texture2D>("Particle/diamond"));
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D Stand = Content.Load<Texture2D>("TextureAtlas/SmileyWalk");
            Texture2D WalkLeft = Content.Load<Texture2D>("TextureAtlas/SmileyWalk");
            Texture2D WalkRight = Content.Load<Texture2D>("TextureAtlas/SmileyWalk");

            animatedSprite = new AnimatedSprite(Stand, 4, 4,new Vector2(200,200));
            animatedSprite.MoveLeft = WalkLeft;
            animatedSprite.MoveRight = WalkRight;
            animatedSprite.Stand = Stand;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();



            animatedSprite.Input();

            // if collide with wall, add a new particle explosion
            if (animatedSprite.SpritePosition.X <= 10)
            {
                animatedSprite.SpritePosition = new Vector2(10, animatedSprite.SpritePosition.Y);
                CollisionExplosion((int)animatedSprite.SpritePosition.X, (int)animatedSprite.SpritePosition.Y, textures, new Vector2(300, 400));
            }
            animatedSprite.Update();

            #region Controlled Explosions

            // cleans up dead particle emitters, stopping them going off
            List<ParticleEngine.ParticleEngine> rems = new List<Spartonne_Windows.ParticleEngine.ParticleEngine>();

            // update alive emitters and add dead ones to the list
            foreach (ParticleEngine.ParticleEngine part in particleEngine)
            {
                part.Update(ms);
                if (!part.alive)
                {
                    rems.Add(part);
                }
            }

            //go through list and remove dead emitters
            foreach (ParticleEngine.ParticleEngine part in rems)
            {
                particleEngine.Remove(part);
            }

            #endregion


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            #region Draw any particles
            foreach (ParticleEngine.ParticleEngine part in particleEngine)
            {
                if (part.alive)
                {
                    part.Draw(spriteBatch);
                }

            }
            #endregion

            animatedSprite.Draw(spriteBatch);

            base.Draw(gameTime);
        }

        public void CollisionExplosion(int X, int Y, List<Texture2D> tex, Vector2 location)
        {
            ParticleEngine.ParticleEngine part = new ParticleEngine.ParticleEngine(tex, location);
            part.alive = true;
            part.EmitterLocation = new Vector2(X, Y);
            particleEngine.Add(part);
        }

    }
}
