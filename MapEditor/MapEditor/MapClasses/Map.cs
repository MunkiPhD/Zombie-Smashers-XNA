using System.IO;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Graphics;

namespace MapEditor.MapClasses {
    class Map {
        SegmentDefinition[] segmentDefs;
        MapSegment[,] mapSegments;
        int[,] col;

        public Map() {
            segmentDefs = new SegmentDefinition[512];
            mapSegments = new MapSegment[3, 64];
            col = new int[20, 20];
            ReadSegmentDefinitions();
        }



        /// <summary>
        /// 
        /// </summary>
        private void ReadSegmentDefinitions() {
            StreamReader reader = new StreamReader(@"Content/maps.zdx");
            string t = "";
            int n;
            int currentTex = 0;
            int curDef = -1;
            Rectangle tRect = new Rectangle();
            string[] split;

            t = reader.ReadLine();

            while (!reader.EndOfStream) {
                t = reader.ReadLine();
                if (t.StartsWith("#")) {
                    if (t.StartsWith("#src")) {
                        split = t.Split(' ');
                        if (split.Length > 1) {
                            n = Convert.ToInt32(split[1]);
                            currentTex = n - 1;
                        }
                    }
                } else { // if it doesnt start with an ampersand
                    curDef++;
                    string name = t;

                    t = reader.ReadLine();
                    split = t.Split(' ');
                    if (split.Length > 3) {
                        tRect.X = Convert.ToInt32(split[0]);
                        tRect.Y = Convert.ToInt32(split[1]);
                        tRect.Width = Convert.ToInt32(split[2]) - tRect.X;
                        tRect.Height = Convert.ToInt32(split[3]) - tRect.Y;
                    } else {
                        Console.WriteLine("Read Fail: " + name);
                    }


                    int tex = currentTex;

                    t = reader.ReadLine();
                    int flags = Convert.ToInt32(t);

                    segmentDefs[curDef] = new SegmentDefinition(name, tex, tRect, flags);
                }
            }
        }


        public SegmentDefinition[] SegmentDefinitions {
            get { return segmentDefs; }
        }



        /// <summary>
        /// 
        /// </summary>
        public MapSegment[,] Segments {
            get { return mapSegments; }
        }


        public int[,] Grid {
            get { return col; }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public int AddSegment(int layer, int index) {
            for (int i = 0; i < 64; i++) {
                if (mapSegments[layer, i] == null) {
                    mapSegments[layer, i] = new MapSegment();
                    mapSegments[layer, i].Index = index;
                    return i;
                }
            }

            return -1;
        }



        public int GetHoveredSegment(int x, int y, int l, Vector2 scroll) {
            float scale = 1.0f;
            if (l == 0)
                scale = 0.75f;
            else if (l == 2)
                scale = 1.25f;
            scale *= 0.5f;

            for (int i = 63; i >= 0; i--) {
                if (mapSegments[l, i] != null) {
                    Rectangle sourceRect = segmentDefs[mapSegments[l, i].Index].SourceRectangle;
                    Rectangle destinationRect = new Rectangle(
                        (int)(mapSegments[l, i].Location.X - scroll.X * scale),
                        (int)(mapSegments[l, i].Location.Y - scroll.Y * scale),
                        (int)(sourceRect.Width * scale),
                        (int)(sourceRect.Height * scale));
                    if (destinationRect.Contains(x, y))
                        return i;
                }
            }

            return -1;
        }

        public void Draw(SpriteBatch sprite, Texture2D[] mapsTextures, Vector2 scroll) {
            Rectangle sourceRect = new Rectangle();
            Rectangle destRect = new Rectangle();

            sprite.Begin();
            for (int l = 0; l < 3; l++) {
                float scale = 1.0f;
                Color color = Color.White;
                if (l == 0) {
                    color = Color.Gray;
                    scale = 0.75f;
                } else if (l == 2) {
                    color = Color.DarkGray;
                    scale = 1.25f;
                }

                scale *= 0.5f;
                for (int i = 0; i < 64; i++) {
                    if (mapSegments[l, i] != null) {
                        sourceRect = segmentDefs[mapSegments[l, i].Index].SourceRectangle;
                        destRect.X = (int)(mapSegments[l, i].Location.X - scroll.X * scale);
                        destRect.Y = (int)(mapSegments[l, i].Location.Y - scroll.Y * scale);
                        destRect.Width = (int)(sourceRect.Width * scale);
                        destRect.Height = (int)(sourceRect.Height * scale);

                        int index = mapSegments[l, i].Index;
                        sprite.Draw(mapsTextures[segmentDefs[index].SourceIndex],
                            destRect,
                            sourceRect,
                            color);

                    }
                }
            }


            sprite.End();
        }
    }
}
