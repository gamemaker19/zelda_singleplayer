using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class FieldItem : Actor
    {
        public InventoryItem inventoryItem;
        public Color glowColor;
        //public Shader* outlineShader = nullptr;
        public float glowAngle = 0;

        public FieldItem(Level level, Point pos, InventoryItem inventoryItem, bool bounceIn) : base(level, pos, "FieldItem")
        {
            this.inventoryItem = inventoryItem;
            this.sprite.frameSpeed = 0;
            this.collectable = new Collectable(this, bounceIn, false);
            this.collectable.inventoryItem = inventoryItem;
            this.collectable.shouldFade = false;
            this.collectable.collectOnTouch = false;
            this.sprite.frameIndex = inventoryItem.item.spriteIndex;

            isStatic = true;
            checkWadables = true;

            if (inventoryItem.item.spawnOddsWeight == 100) glowColor = new Color(0, 255, 81);
            else if (inventoryItem.item.spawnOddsWeight == 50) glowColor = new Color(33, 237, 255);
            else if (inventoryItem.item.spawnOddsWeight == 25) glowColor = new Color(255, 23, 92);
            else if (inventoryItem.item.spawnOddsWeight == 10) glowColor = new Color(174, 23, 255);
            else glowColor = new Color(243, 255, 135);

            /*
            if (inventoryItem.item.spawnOddsWeight == 100)
                outlineShader = new OutlineShader(global.shaders["outline"].shader, new float[3]{ 0, 255, 64 });
            else if (inventoryItem.item.spawnOddsWeight == 50)
                outlineShader = new OutlineShader(global.shaders["outline"].shader, new float[3]{ 0, 145, 255 });
            else if (inventoryItem.item.spawnOddsWeight == 25)
                outlineShader = new OutlineShader(global.shaders["outline"].shader, new float[3]{ 255, 0, 0 });
            else if (inventoryItem.item.spawnOddsWeight == 10)
                outlineShader = new OutlineShader(global.shaders["outline"].shader, new float[3]{ 255, 65, 255 });
            else
                outlineShader = new OutlineShader(global.shaders["outline"].shader, new float[3]{ 252, 255, 64 });

            if(outlineShader)
                shaders.push_back(outlineShader);
                */

            //Global.fieldItems.Add(this);
        }

        public override void update()
        {
            base.update();

            /*
            if (global.frameCount % 60 == 0)
            {
                Point randPos(helpers::randomRange(-8, 8), helpers::randomRange(-8, 8));
                Anim * anim = new Anim(level, pos + randPos, "SwordSparkle");
                anim.sprite.frameSpeed *= 0.25;

                ALLEGRO_COLOR color;
                if (inventoryItem.item.spawnOddsWeight == 100) color = new Color(0, 255, 81, 255);
                else if (inventoryItem.item.spawnOddsWeight == 50) color = new Color(33, 237, 255, 255);
                else if (inventoryItem.item.spawnOddsWeight == 25) color = new Color(255, 84, 84, 255);
                else if (inventoryItem.item.spawnOddsWeight == 10) color = new Color(157, 128, 255, 255);
                else color = new Color(243, 255, 135, 255);

                anim.tint = color;
            }
            */
            glowAngle += 45 * Global.spf;
        }

        public override void render()
        {
            //Point spos = getScreenPos();
            //global.sprites["FieldItemGlow"].draw(spos.x, spos.y, 1, 1, glowAngle, 1, {}, zIndex, true, 0, 0, {}, glowColor);
            base.render();
        }

    }
}
