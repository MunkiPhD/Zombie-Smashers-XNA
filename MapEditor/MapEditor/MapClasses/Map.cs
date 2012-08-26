using System.IO;
using Microsoft.Xna.Framework;
using System;

namespace MapEditor.MapClasses {
    class Map {
        SegmentDefinition[] segmentDefs;

        public Map() {
            segmentDefs = new SegmentDefinition[512];
            ReadSegmentDefinitions();
        }

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
    }
}
