using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class TextGen
    {
        public List<string> dialogs;
        public Actor actor;

        public TextGen(Actor actor, List<string> dialogs)
        {
            this.actor = actor;
            this.dialogs = dialogs;
        }

        public void update()
        {

        }
    }
}
