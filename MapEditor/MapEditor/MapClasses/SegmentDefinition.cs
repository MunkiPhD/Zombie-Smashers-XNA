using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace MapEditor.MapClasses {
    class SegmentDefinition {
        private string _name;
        private int _sourceIndex;
        private Rectangle _sourceRect;
        private int _flags;

        public SegmentDefinition(string name, int sourceIndex, Rectangle sourceRectangle, int flags) {
            _name = name;
            _sourceIndex = sourceIndex;
            _sourceRect = sourceRectangle;
            _flags = flags;
        }


        public String Name {
            get { return _name; }
            set { _name = value; }
        }

        public int SourceIndex {
            get { return _sourceIndex; }
            set { _sourceIndex = value; }
        }

        public Rectangle SourceRectangle {
            get { return _sourceRect; }
            set { _sourceRect = value; }
        }

        public int Flags {
            get { return Flags; }
            set { Flags = value; }
        }
    }
}
