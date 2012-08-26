using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor {
    class Text {
        private float size = 1f;
        private Color color = Color.White;
        SpriteFont font;
        SpriteBatch sprite;


        public Text(SpriteBatch _sprite, SpriteFont _font) {
            font = _font;
            sprite = _sprite;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="s"></param>
        public void DrawText(int x, int y, String s) {
            sprite.Begin();
            sprite.DrawString(font, s, new Vector2((float)x, (float)y), color, 0f, new Vector2(), size, SpriteEffects.None, 1f);
            sprite.End();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="s"></param>
        /// <param name="mosX"></param>
        /// <param name="mosY"></param>
        /// <param name="mouseClick"></param>
        /// <returns></returns>
        public bool DrawClickText(int x, int y, String s, int mosX, int mosY, bool mouseClick) {
            color = Color.White;
            bool r = false;
            if (mosX > x && mosY > y &&
                mosX < x + font.MeasureString(s).X * size &&
                mosY < y + font.MeasureString(s).Y * size) {
                    color = Color.Yellow;
                    if (mouseClick)
                        r = true;
            }

            DrawText(x, y, s);

            return r;
        }

        public Color Color {
            get { return color; }
            set { color = value; }
        }


        public float Size {
            get { return size; }
            set { size = value; }
        }
    }
}
