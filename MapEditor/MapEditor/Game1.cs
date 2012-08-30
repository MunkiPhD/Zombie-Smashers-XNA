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
using MapEditor.MapClasses;

namespace MapEditor {
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Text text;
        SpriteFont font;

        Map map;

        Texture2D[] mapsTex;
        Texture2D nulLTex;

        Texture2D iconsTexture;
        int mouseX, mouseY;
        bool rightMouseDown;
        bool mouseClick;

        int mouseDragSegment = -1;
        int currentLayer = 1;
        int pMouseX, pMouseY;

        public Game1() {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferHeight = 700;
           // graphics.PreferredBackBufferWidth = 900;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize() {
            // TODO: Add your initialization logic here
            map = new Map();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            font = Content.Load<SpriteFont>(@"Fonts\Arial");
            text = new Text(spriteBatch, font);

            // text = new Text(textTex, sprite);
            nulLTex = Content.Load<Texture2D>(@"gfx/1x1");
            mapsTex = new Texture2D[1];
            for (int i = 0; i < mapsTex.Length; i++)
                mapsTex[i] = Content.Load<Texture2D>(@"gfx/maps" + (i + 1).ToString());

            iconsTexture = Content.Load<Texture2D>(@"gfx/icons");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent() {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime) {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            MouseState mState = Mouse.GetState();
            mouseX = mState.X;
            mouseY = mState.Y;

            bool pMouseDown = rightMouseDown;
            if(mState.LeftButton == ButtonState.Pressed)
                rightMouseDown = true;
            else
                rightMouseDown = false;

            if(pMouseDown && !rightMouseDown)
                mouseClick = true;

            if(mouseDragSegment > -1) {
                if(!rightMouseDown)
                    mouseDragSegment = -1;
                else {
                    Vector2 loc = map.Segments[currentLayer, mouseDragSegment].Location;
                    loc.X += (mouseX - pMouseX);
                    loc.Y += (mouseY - pMouseY);
                    map.Segments[currentLayer, mouseDragSegment].Location = loc;
                }
            }

            pMouseX = mouseX;
            pMouseY = mouseY;
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            map.Draw(spriteBatch, mapsTex, new Vector2());

            DrawMapSegments();
            DrawText();
            DrawCursor();
            base.Draw(gameTime);
        }


        private void DrawMapSegments() {
            Rectangle sRect = new Rectangle();
            Rectangle dRect = new Rectangle();

            text.Size = 0.8f;

            spriteBatch.Begin();

            spriteBatch.Draw(nulLTex, new Rectangle(500, 20, 280, 550), new Color(0, 0, 0, 100));
            spriteBatch.End();

            for(int i = 0; i < 9; i++) {
                SegmentDefinition segDef = map.SegmentDefinitions[i];
                if(segDef == null)
                    continue;

                spriteBatch.Begin();
                dRect.X = 500;
                dRect.Y = 50 + i * 60;

                sRect = segDef.SourceRectangle;

                if(sRect.Width > sRect.Height) {
                    dRect.Width = 45;
                    dRect.Height = (int)(((float)sRect.Height / (float)sRect.Width) * 45.0f);
                } else {
                    dRect.Height = 45;
                    dRect.Width = (int)(((float)sRect.Width / (float)sRect.Height * 45.0f));
                }


                spriteBatch.Draw(mapsTex[segDef.SourceIndex], dRect, sRect, Color.White);

                spriteBatch.End();

                text.Color = Color.White;
                text.DrawText(dRect.X + 50, dRect.Y, segDef.Name);


                // THIS should be elsewhere, but we're breaking convention and putting logic here
                if(rightMouseDown) {
                    if(mouseX > dRect.X && mouseX < 780 &&
                        mouseY > dRect.Y && mouseY < dRect.Y + 45) {
                        if(mouseDragSegment == -1) {
                            int f = map.AddSegment(currentLayer, i);
                            if(f <= -1)
                                continue;

                            map.Segments[currentLayer, f].Location.X = mouseX - sRect.Width / 4;
                            map.Segments[currentLayer, f].Location.Y = mouseY - sRect.Height / 4;
                            mouseDragSegment = f;
                        }
                    }
                }
            }



            //// TODO: Add your drawing code here
            //text.Size = 3.0f;
            //text.Color = new Color(0, 0, 0, 125);
            //for (int i = 0; i < 3; i++) {
            //    if (i == 2)
            //        text.Color = Color.White;

            //    text.DrawText(25 - i * 2, 250 - i * 2, "Blarg dsfsdfds fsd fds ");
            //}

        }

        /// <summary>
        /// 
        /// </summary>
        private void DrawCursor() {
            spriteBatch.Begin();
            spriteBatch.Draw(iconsTexture,
                new Vector2(mouseX, mouseY),  
                new Rectangle(0,0,32,32), 
                Color.White, 
                0.0f, 
                new Vector2(0,0), 
                1.0f, 
                SpriteEffects.None, 
                0.0f);
                
            spriteBatch.End();
        }



        /// <summary>
        /// 
        /// </summary>
        private void DrawText() {
            string layerName = "map";
            switch(currentLayer) {
                case 0:
                    layerName = "back";
                    break;
                case 1:
                    layerName = "mid";
                    break;
                case 2:
                    layerName = "fore";
                    break;
            }
            if(text.DrawClickText(5, 5, "layer " + layerName, mouseX, mouseY, mouseClick))
                currentLayer = (currentLayer + 1) % 3;
            mouseClick = false;
        }
    }
}
