using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace Spartonne_Windows.AnimateSprite
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public Texture2D MoveLeft { get; set; }
        public Texture2D MoveRight { get; set; }
        public Texture2D Stand { get; set; }

        public int Rows { get; set; }
        public int Columns { get; set; }

        public Vector2 SpritePosition;
        private int currentFrame;
        private int totalFrames;

        public AnimatedSprite(Texture2D texture, int rows, int columns, Vector2 startPos)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            SpritePosition = startPos;
        }

        public void Update()
        {
            currentFrame++;
            if (currentFrame == totalFrames)
                currentFrame = 0;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = Texture.Width / Rows;
            int height = Texture.Height / Columns;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)this.SpritePosition.X, (int)this.SpritePosition.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }

        public void Input()
        {
            KeyboardState kb = Keyboard.GetState();
            if (kb.IsKeyDown(Keys.Left))
            {
                this.SpritePosition.X -= 5;
                Texture = MoveLeft;
            }
            else
            {
                if (kb.IsKeyDown(Keys.Right))
                {
                    this.SpritePosition.X += 5;
                    Texture = MoveRight;
                }
                else
                {

                    if (kb.IsKeyDown(Keys.Up))
                    {
                        this.SpritePosition.Y -= 5;
                        Texture = Stand;
                    }
                    else
                    {
                        if (kb.IsKeyDown(Keys.Down))
                        {
                            this.SpritePosition.Y += 5;
                            Texture = Stand;
                        }

                        else
                        {
                            Texture = Stand;
                        }
                    }
                }
            }
            
        }
    }
}