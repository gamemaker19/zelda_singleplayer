using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class BigFairy : Actor
    {
	    public HealSparkle healSparkle;
        public bool fade = false;
        public float fadeTime = 0;

        public BigFairy(Level level, Point pos) : base(level, pos, "BigFairy")
        {
            z = 15;
            useGravity = false;
            healSparkle = new HealSparkle(level, pos.addxy(0, -15));
        }

        public override void update()
        {
            base.update();
            if (fadeTime > 0)
            {
                fadeTime += Global.spf;
                if (fadeTime > 1)
                {
                    float fadeFactor = fadeTime - 1;
                    alpha = 1 - fadeFactor;
                }
                if (fadeTime > 2)
                {
                    remove();
                }
            }
        }

        public void heal(Character character)
        {
            if (fade) return;
            healSparkle.targetChar = character;
            fade = true;
            fadeTime = 0.01f;
            playSound("fairy4x");
        }
        
    }
}
