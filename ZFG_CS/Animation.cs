using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ZFG_CS
{
    public class Animation
    {
        //Populated from JSON prototype
        public string name = "";
        public List<Collider> hitboxes = new List<Collider>();
        public List<Frame> frames = new List<Frame>();
        public int loopStartFrame = 0;
        public int frameIndex = 0;
        public float frameSpeed = 1;
        public float frameTime = 0;
        public float animTime = 0;
        public int zIndex = 0;
        public bool isVisible = true;
        public bool useBluePalette = false;
        public Texture bitmap;
        public string wrapMode = "";
        public string alignment = "";
        public int loopCount = 0;
        public float xScale = 1;
        public float yScale = 1;
        
        public Animation(string path)
        {
            string spriteJsonStr = File.ReadAllText(path);
            
            dynamic spriteJson = JsonConvert.DeserializeObject(spriteJsonStr);
            
	        name = Path.GetFileNameWithoutExtension(path);
            loopStartFrame = Convert.ToInt32(spriteJson.loopStartFrame);
            wrapMode = Convert.ToString(spriteJson.wrapMode);
            alignment = Convert.ToString(spriteJson.alignment);

            string spritesheetPath = Convert.ToString(spriteJson.spritesheetPath);
            bitmap = Global.textures[Path.GetFileNameWithoutExtension(spritesheetPath)];
	
	        float spriteW = 0;
            float spriteH = 0;

            Image tempImage = bitmap.CopyToImage();

            //Handle frames here
            JArray frameJsons = spriteJson["frames"];
	        for (int i = 0; i < frameJsons.Count; i++)
	        {
                dynamic frameJson = frameJsons[i];
		        float x1 = (float)Convert.ToDouble(frameJson["rect"]["topLeftPoint"]["x"]);
                float y1 = (float)Convert.ToDouble(frameJson["rect"]["topLeftPoint"]["y"]);
                float x2 = (float)Convert.ToDouble(frameJson["rect"]["botRightPoint"]["x"]);
                float y2 = (float)Convert.ToDouble(frameJson["rect"]["botRightPoint"]["y"]);
                float duration = (float)Convert.ToDouble(frameJson["duration"]);
                float offsetX = (float)Convert.ToDouble(frameJson["offset"]["x"]);
                float offsetY = (float)Convert.ToDouble(frameJson["offset"]["y"]);
                string frameTags = Convert.ToString(frameJson["tags"]);
		        
                int xDir = 1;
                int yDir = 1;
		
		        xDir = Convert.ToInt32(frameJson["xDir"]);
		        yDir = Convert.ToInt32(frameJson["yDir"]);

                Rect rect = new Rect(x1, y1, x2, y2);
                Point offset = new Point(offsetX, offsetY);

		        if (spriteW == 0) spriteW = rect.w();
		        if (spriteH == 0) spriteH = rect.h();
		
		        List<Point> POIs = new List<Point>();
		        if (frameJson["POIs"] != null)
		        {
			        dynamic poisJson = frameJson["POIs"];
			        for (int j = 0;  j < poisJson.Count; j++)
			        {
				        float poiX = (float)Convert.ToDouble(poisJson[j]["x"]);
                        float poiY = (float)Convert.ToDouble(poisJson[j]["y"]);
                        POIs.Add(new Point(poiX, poiY));
			        }
		        }

		        List<Collider> hitboxes2 = new List<Collider>();

                //SPRITE_SETUP

                List<Frame> childFrames = new List<Frame>();
		        if (frameJson["childFrames"] != null)
		        {
			        JArray childFramesJson = frameJson["childFrames"];
			        for (int j = 0; j < childFramesJson.Count; j++)
			        {
                        dynamic childFrameJson = childFramesJson[j];
                        float cx1 = (float)Convert.ToDouble(childFrameJson["rect"]["topLeftPoint"]["x"]);
                        float cy1 = (float)Convert.ToDouble(childFrameJson["rect"]["topLeftPoint"]["y"]);
                        float cx2 = (float)Convert.ToDouble(childFrameJson["rect"]["botRightPoint"]["x"]);
                        float cy2 = (float)Convert.ToDouble(childFrameJson["rect"]["botRightPoint"]["y"]);
                        string tags = childFrameJson["tags"];
                        processChildFrame(childFrameJson, offset, cx1, cy1, cx2, cy2, tags, POIs, childFrames, tempImage);
                    }
                }
                
		        if (frameJson["hitboxes"] != null)
		        {
			        JArray hitboxFramesJson = frameJson["hitboxes"];
			        for (int j = 0; j < hitboxFramesJson.Count; j++)
			        {
				        dynamic hitboxFrameJson = hitboxFramesJson[j];
				        Collider hitbox = getHitboxFromJson(hitboxFrameJson, spriteW, spriteH);
				        if (name.Contains("sword"))
				        {
					        hitbox.tags = "sword";
				        }
				        if (hitboxFrameJson["flag"] != null)
				        {
					        hitbox.tags = hitboxFrameJson["flag"];
				        }
				        hitboxes2.Add(hitbox);
			        }
		        }

		        Frame frame = new Frame(rect, duration, offset, hitboxes2, POIs, childFrames);
                frame.xDir = xDir == 0 ? 1 : xDir;
		        frame.yDir = yDir == 0 ? 1 : yDir;
		        frame.tags = frameTags;
		        frames.Add(frame);
	        }

	        //Handle Global hitboxes here
	        List<Collider> hitboxes = new List<Collider>();
	        if (spriteJson["hitboxes"] != null)
	        {
		        dynamic hitboxFramesJson = spriteJson["hitboxes"];
		        for (int j = 0;  j < hitboxFramesJson.Count; j++)
		        {
			        dynamic hitboxFrameJson = hitboxFramesJson[j];
			        Collider hitbox = getHitboxFromJson(hitboxFrameJson, spriteW, spriteH);

			        if (hitboxFrameJson["flag"] != null)
			        {
				        hitbox.tags = hitboxFrameJson["flag"];
			        }

			        hitboxes.Add(hitbox);
		        }
	        }

            this.hitboxes = hitboxes;

            tempImage.Dispose();
        }
        
        public Animation clone()
        {
            var clonedAnim = (Animation)MemberwiseClone();
            clonedAnim.hitboxes = new List<Collider>();
            foreach(Collider collider in hitboxes)
            {
                clonedAnim.hitboxes.Add(collider.clone());
            }
            clonedAnim.frames = new List<Frame>();
            foreach(Frame frame in frames)
            {
                clonedAnim.frames.Add(frame.clone());
            }
            return clonedAnim;
        }

        public void processChildFrame(dynamic childFrameJson, Point offset, float x1, float y1, float x2, float y2, string tags, List<Point> POIs, List<Frame> childFrames, Image tempImage)
        {
            //repeat:
            float offsetX = (float)Convert.ToDouble(childFrameJson["offset"]["x"]);
            float offsetY = (float)Convert.ToDouble(childFrameJson["offset"]["y"]);
            int xDir = Convert.ToInt32(childFrameJson["xDir"]);
            int yDir = Convert.ToInt32(childFrameJson["yDir"]);
            float zIndex = (float)Convert.ToDouble(childFrameJson["zIndex"]);

            Rect rect = new Rect(x1, y1, x2, y2);
            Point childOffset = new Point(offsetX, offsetY);
            Frame childFrame = new Frame(rect, 0, childOffset, new List<Collider>(), new List<Point>(), new List<Frame>());
            childFrame.zIndex = zIndex;
            childFrame.xDir = xDir;
            childFrame.yDir = yDir;
            childFrame.tags = tags;

            bool hasSword = tags.Contains("sword");
            bool hasShield = tags.Contains("shield");
            if (hasSword || hasShield)
            {
                if (hasSword && zIndex >= 0)
                {
                    childFrame.zIndex = 6;
                }

                int minX = int.MaxValue;
                int minY = int.MaxValue;
                int maxX = int.MinValue;
                int maxY = int.MinValue;
                for (int x = (int)x1; x < (int)x2; x++)
                {
                    for (int y = (int)y1; y < (int)y2; y++)
                    {
                        Color color = tempImage.GetPixel((uint)x, (uint)y);
                        if (color.A > 0)
                        {
                            if (x < minX) minX = x;
                            if (y < minY) minY = y;
                            if (x > maxX) maxX = x;
                            if (y > maxY) maxY = y;
                        }
                    }
                }

                if (minX == int.MaxValue) minX = (int)x1;
                if (minY == int.MaxValue) minY = (int)y1;
                if (maxX == int.MinValue) maxX = (int)x2;
                if (maxY == int.MinValue) maxY = (int)y2;

                float topLeftOffsetX = minX - x1;
                float topLeftOffsetY = minY - y1;

                float botRightOffsetX = x2 - maxX;
                float botRightOffsetY = y2 - maxY;

                float w = x2 - x1;
                float h = y2 - y1;

                if (xDir == -1)
                {
                    topLeftOffsetX = w - topLeftOffsetX;
                    botRightOffsetX = w - botRightOffsetX;
                }
                if (yDir == -1)
                {
                    topLeftOffsetY = h - topLeftOffsetY;
                    botRightOffsetY = h - botRightOffsetY;
                }

                childFrame.topLeftOffset = new Point(topLeftOffsetX, topLeftOffsetY);
                childFrame.botRightOffset = new Point(-botRightOffsetX, -botRightOffsetY);
            }

            if (tags == "bush")
            {
                POIs.Add(offset + new Point(offsetX + rect.w() / 2, offsetY + rect.h() / 2));
            }
            else if (tags != "itemshadow" && tags != "hook")
            {
                childFrames.Add(childFrame);
            }

            if (tags == "sword")
            {
                tags = "sword2"; y1 += 32; y2 += 32;
                processChildFrame(childFrameJson, offset, x1, y1, x2, y2, tags, POIs, childFrames, tempImage);
            }
            else if (tags == "sword2")
            {
                tags = "sword3"; y1 += 32; y2 += 32;
                processChildFrame(childFrameJson, offset, x1, y1, x2, y2, tags, POIs, childFrames, tempImage);
            }
            else if (tags == "sword3")
            {
                tags = "sword4"; y1 += 32; y2 += 32;
                processChildFrame(childFrameJson, offset, x1, y1, x2, y2, tags, POIs, childFrames, tempImage);
            }

            if (tags == "shield")
            {
                tags = "shield2"; y1 += 16; y2 += 16;
                processChildFrame(childFrameJson, offset, x1, y1, x2, y2, tags, POIs, childFrames, tempImage);
            }
            else if (tags == "shield2")
            {
                tags = "shield3"; y1 += 16; y2 += 16;
                processChildFrame(childFrameJson, offset, x1, y1, x2, y2, tags, POIs, childFrames, tempImage);
            }
        }

        public Collider getHitboxFromJson(dynamic hitboxFrameJson, float spriteW, float spriteH)
        {
            float cx = 0;
            float cy = 0;

	        if (alignment == "topleft") {
		        cx = 0; cy = 0;
	        }
	        else if (alignment == "topmid") {
		        cx = 0.5f; cy = 0;
	        }
	        else if (alignment == "topright") {
		        cx = 1; cy = 0;
	        }
	        else if (alignment == "midleft") {
		        cx = 0; cy = 0.5f;
	        }
	        else if (alignment == "center") {
		        cx = 0.5f; cy = 0.5f;
	        }
	        else if (alignment == "midright") {
		        cx = 1; cy = 0.5f;
	        }
	        else if (alignment == "botleft") {
		        cx = 0; cy = 1;
	        }
	        else if (alignment == "botmid") {
		        cx = 0.5f; cy = 1;
	        }
	        else if (alignment == "botright") {
		        cx = 1; cy = 1;
	        }

	        cx *= spriteW;
	        cy *= spriteH;

	        float w = (float)Convert.ToDouble(hitboxFrameJson["width"]);
            float h = (float)Convert.ToDouble(hitboxFrameJson["height"]);
            
            float x1 = (float)Convert.ToDouble(hitboxFrameJson["offset"]["x"]);
            float y1 = (float)Convert.ToDouble(hitboxFrameJson["offset"]["y"]);
            float x2 = x1 + w;
            float y2 = y1 + h;
            Collider hitbox = new Collider(new Rect(x1, y1, x2, y2));
	        if(hitboxFrameJson["isTrigger"] != null)
	        {
		        string isTriggerStr = hitboxFrameJson["isTrigger"];
                hitbox.isTrigger = isTriggerStr == "true" ? true : false;
	        }
	        string tags = hitboxFrameJson["tags"];
            hitbox.tags = tags;

	        return hitbox;
        }

        //Draws a sprite immediately in screen coords. Good for HUD sprites whose z-index must be more fine grain controlled
        public void drawImmediate(float x, float y)
        {
            Frame currentFrame = getCurrentFrame();

            float cx = 0;
            float cy = 0;

            if (alignment == "topleft")
            {
                cx = 0; cy = 0;
            }
            else if (alignment == "topmid")
            {
                cx = 0.5f; cy = 0;
            }
            else if (alignment == "topright")
            {
                cx = 1; cy = 0;
            }
            else if (alignment == "midleft")
            {
                cx = 0; cy = 0.5f;
            }
            else if (alignment == "center")
            {
                cx = 0.5f; cy = 0.5f;
            }
            else if (alignment == "midright")
            {
                cx = 1; cy = 0.5f;
            }
            else if (alignment == "botleft")
            {
                cx = 0; cy = 1;
            }
            else if (alignment == "botmid")
            {
                cx = 0.5f; cy = 1;
            }
            else if (alignment == "botright")
            {
                cx = 1; cy = 1;
            }

            cx = cx * currentFrame.rect.w();
            cy = cy * currentFrame.rect.h();

            DrawWrappers.DrawTextureImmediate(bitmap, currentFrame.rect.x1, currentFrame.rect.y1, currentFrame.rect.w(), currentFrame.rect.h(), x - cx, y - cy);
        }

        public void draw(float x, float y, int xDir, int yDir, float angle, float alpha, Shader shader, float zIndex, bool isWorldPos, float offsetX = 0, float offsetY = 0, HashSet<string> childFrameTagsToHide = null, Color? tint = null)
        {
            if (angle != 0) xDir = 1;

            Frame currentFrame = getCurrentFrame();

            float cx = 0;
            float cy = 0;

            if (alignment == "topleft")
            {
                cx = 0; cy = 0;
            }
            else if (alignment == "topmid")
            {
                cx = 0.5f; cy = 0;
            }
            else if (alignment == "topright")
            {
                cx = 1; cy = 0;
            }
            else if (alignment == "midleft")
            {
                cx = 0; cy = 0.5f;
            }
            else if (alignment == "center")
            {
                cx = 0.5f; cy = 0.5f;
            }
            else if (alignment == "midright")
            {
                cx = 1; cy = 0.5f;
            }
            else if (alignment == "botleft")
            {
                cx = 0; cy = 1;
            }
            else if (alignment == "botmid")
            {
                cx = 0.5f; cy = 1;
            }
            else if (alignment == "botright")
            {
                cx = 1; cy = 1;
            }

            cx = cx * currentFrame.rect.w();
            cy = cy * currentFrame.rect.h();

            float frameOffsetX = (currentFrame.offset.x + offsetX) * xDir * currentFrame.xDir;
            float frameOffsetY = currentFrame.offset.y + offsetY;

            if (useBluePalette && (currentFrame.tags.Contains("rod") || currentFrame.tags.Contains("cane")))
            {
                shader = Global.shaders["replaceRedBlue"];
            }

            //DrawWrappers.DrawTexture(bitmap, currentFrame.rect.x1, currentFrame.rect.y1, currentFrame.rect.w(), currentFrame.rect.h(), tint, alpha, cx, cy, x + frameOffsetX, y + frameOffsetY, xDir * currentFrame.xDir * xScale, yDir * yScale, angle, 0, Global.drawLink, zIndex, camOffset, shaders);

            float xDirArg = xDir * currentFrame.xDir * xScale;
            float yDirArg = yDir * yScale;

            DrawWrappers.DrawTexture(bitmap, currentFrame.rect.x1, currentFrame.rect.y1, currentFrame.rect.w(), currentFrame.rect.h(), x + frameOffsetX, y + frameOffsetY, zIndex, cx, cy, xDirArg, yDirArg, angle, alpha, shader, isWorldPos);

            foreach (Frame childFrame in currentFrame.childFrames)
            {
                if (childFrameTagsToHide != null && childFrameTagsToHide.Contains(childFrame.tags))
                {
                    continue;
                }
                if (!childFrame.enabled) continue;

                //Child frames set to 0 get drawn at 0.5
                //Wadable drawn at 5
                float childZ = zIndex + (childFrame.zIndex * 0.1f);
                if (childFrame.zIndex == 0)
                {
                    childZ = zIndex + (childFrame.zIndex + 0.5f) * 0.1f;
                }

                int childDirX = childFrame.xDir * xDir;

                float flipOffsetX = 0;
                float flipOffsetY = (childFrame.yDir == -1 ? childFrame.rect.h() : 0);

                float childOffsetX = (currentFrame.offset.x + childFrame.offset.x + offsetX) * xDir;
                float childOffsetY = currentFrame.offset.y + childFrame.offset.y + offsetY + flipOffsetY;

                if (xDir == 1 && childFrame.xDir == -1)
                {
                    flipOffsetX = childFrame.rect.w();
                    childOffsetX = (currentFrame.offset.x + childFrame.offset.x + offsetX) + flipOffsetX;
                }
                else if (xDir == -1 && childFrame.xDir == -1)
                {
                    flipOffsetX = childFrame.rect.w();
                    childOffsetX = (currentFrame.offset.x + childFrame.offset.x + offsetX) + flipOffsetX;
                    childOffsetX *= -1;
                }

                Shader childShader = shader;
                if (useBluePalette && (childFrame.tags.Contains("cane") || childFrame.tags.Contains("rod")))
                {
                    childShader = Global.shaders["replaceRedBlue"];
                }

                //DrawWrappers.DrawTexture(bitmap, childFrame.rect.x1, childFrame.rect.y1, childFrame.rect.w(), childFrame.rect.h(), tint, alpha, 0, 0, x + childOffsetX, y + childOffsetY, childDirX, childFrame.yDir, angle, 0, Global.drawLink, childZ, camOffset, childShaders);
                DrawWrappers.DrawTexture(bitmap, childFrame.rect.x1, childFrame.rect.y1, childFrame.rect.w(), childFrame.rect.h(), x + childOffsetX, y + childOffsetY, childZ, 0, 0, childDirX, childFrame.yDir, angle, alpha, childShader, isWorldPos);
            }
        }

        public bool update()
        {
            frameTime += Global.spf * frameSpeed;
            animTime += Global.spf * frameSpeed;
            if (frameTime >= getCurrentFrame().duration)
            {
                bool onceEnd = wrapMode == "once" && frameIndex == frames.Count - 1;
                if (!onceEnd)
                {
                    frameTime = loopStartFrame;
                    frameIndex++;
                    if (frameIndex >= frames.Count)
                    {
                        frameIndex = 0;
                        animTime = 0;
                        loopCount++;
                    }
                    return true;
                }
            }
            return false;
        }

        public Frame getCurrentFrame()
        {
	        return frames[frameIndex];
        }

        public bool isAnimOver()
        {
            return (frameIndex == frames.Count - 1 && frameTime >= getCurrentFrame().duration) || loopCount > 0;
        }

        public float getAnimLength()
        {
            float total = 0;
            foreach (Frame frame in frames)
            {
                total += frame.duration;
            }
            return total;
        }
    }
}
