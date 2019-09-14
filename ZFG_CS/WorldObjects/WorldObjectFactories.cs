using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class WorldObjectFactories
    {
        public static Actor createThrowable(Level level, Point pos, string sprite, string liftSpriteName, string breakSprite, bool generatePickup, bool isBig, bool hookable, float damage, string breakSound)
        {
            if (isBig) pos += new Point(8, 8);
            Actor actor = new Actor(level, pos, sprite);
            actor.sprite.frameSpeed = 0;
            Throwable throwable = new Throwable(actor, liftSpriteName, breakSprite, generatePickup, damage, breakSound);
            actor.throwable = throwable;
            actor.hookable = hookable;
            return actor;
        }

        public static Actor createRupeeGreen(Level level, Point pos, bool bounceUp, int rupeeGain = 1)
        {
            Actor actor = new Actor(level, pos, "PickupRupeeGreen");
            Collectable collectable = new Collectable(actor, bounceUp);
            collectable.rupeeGain = rupeeGain;
            actor.collectable = collectable;
            return actor;
        }

        public static Actor createRupeeBlue(Level level, Point pos, bool bounceUp)
        {
            Actor actor = new Actor(level, pos, "PickupRupeeBlue");
            Collectable collectable = new Collectable(actor, bounceUp);
            collectable.rupeeGain = 5;
            actor.collectable = collectable;
            return actor;
        }

        public static Actor createRupeeRed(Level level, Point pos, bool bounceUp)
        {
            Actor actor = new Actor(level, pos, "PickupRupeeRed");
            Collectable collectable = new Collectable(actor, bounceUp);
            collectable.rupeeGain = 20;
            actor.collectable = collectable;
            return actor;
        }

        public static Actor createRecoveryHeart(Level level, Point pos, bool bounceUp)
        {
            Actor actor = new Actor(level, pos, "PickupHeart");
            Collectable collectable = new Collectable(actor, bounceUp, true);
            collectable.healthGain = 1;
            collectable.drift = true;
            collectable.driftSprite = "PickupHeartFall";
            actor.collectable = collectable;
            actor.name = "PickupHeart";
            return actor;
        }

        public static Actor createMagicJarSmall(Level level, Point pos, bool bounceUp)
        {
            Actor actor = new Actor(level, pos, "PickupMagicSmall");
            Collectable collectable = new Collectable(actor, bounceUp);
            collectable.magicGain = 8;
            actor.collectable = collectable;
            return actor;
        }

        public static Actor createMagicJarBig(Level level, Point pos, bool bounceUp)
        {
            Actor actor = new Actor(level, pos, "PickupMagicLarge");
            Collectable collectable = new Collectable(actor, bounceUp);
            collectable.magicGain = -1;
            actor.collectable = collectable;
            return actor;
        }

        public static Actor createBombPickup(Level level, Point pos, int count, bool bounceUp)
        {
            Actor actor = new Actor(level, pos, "PickupBomb");
            Collectable collectable = new Collectable(actor, bounceUp);
            collectable.bombGain = count;
            actor.collectable = collectable;
            return actor;
        }

        public static Actor createArrowPickup(Level level, Point pos, int count, bool bounceUp)
        {
            Actor actor = new Actor(level, pos, "PickupArrow");
            Collectable collectable = new Collectable(actor, bounceUp);
            collectable.arrowGain = count;
            actor.collectable = collectable;
            return actor;
        }

        public static Actor createRandomPickup(Level level, Point pos, bool bounceUp)
        {
            //return createRecoveryHeart(level, pos, true);

            int rand = Helpers.randomRange(0, 400);
            if (rand < 30)
            {
                return WorldObjectFactories.createRupeeGreen(level, pos, bounceUp);
            }
            else if (rand < 50)
            {
                int amount = 1;
                int amountDecider = Helpers.randomRange(1, 10);
                if (amountDecider < 6) amount = 1;
                else if (amountDecider < 9) amount = 4;
                else if (amountDecider <= 10) amount = 8;
                return createArrowPickup(level, pos, amount, bounceUp);
            }
            /*
            else if (rand < 50)
            {
                int amount = 0;
                int amountDecider = helpers::randomRange(1, 10);
                if (amountDecider < 6) amount = 1;
                else if (amountDecider < 8) amount = 4;
                else if (amountDecider < 9) amount = 8;
                return createBombPickup(level, pos, amount, bounceUp);
            }
            */
            else if (rand < 70)
            {
                return createRecoveryHeart(level, pos, bounceUp);
            }
            else if (rand < 90)
            {
                return createMagicJarSmall(level, pos, bounceUp);
            }
            else if (rand < 97)
            {
                return createRupeeBlue(level, pos, bounceUp);
            }
            else if (rand < 98)
            {
                return createRupeeRed(level, pos, bounceUp);
            }
            else if (rand < 99)
            {
                return createMagicJarBig(level, pos, bounceUp);
            }
            else if (rand < 100)
            {
                return new Fairy(level, pos);
            }
            else
            {
                return null;
            }
        }
    }
}
