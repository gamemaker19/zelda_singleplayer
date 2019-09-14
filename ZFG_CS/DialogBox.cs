using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using static SFML.Window.Keyboard;

namespace ZFG_CS
{
    public class DialogBox
    {
        public List<List<string>> dialogs;
        public Animation boxSprite;
        public int dialogIndex = 0;
        public int rowIndex = 0;
        public float letterIndex = 0;
        
        public List<string> clampDialog(string dialog)
        {
            var words = dialog.Split(' ');
            string currentDialog = "";
            List<string> lines = new List<string>();
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                string prevDialog = currentDialog;
                currentDialog += word + (i == words.Length - 1 ? "" : " ");

                Text text = new Text(currentDialog, Global.font, 48);
                float width = text.GetLocalBounds().Width;
                if (width > 160 * 4)
                {
                    lines.Add(prevDialog);
                    currentDialog = word + (i == words.Length - 1 ? "" : " ");
                }
                if (i == words.Length - 1)
                {
                    lines.Add(currentDialog);
                }
            }
            return lines;
        }

        public DialogBox(List<string> dialogs)
        {
	        boxSprite = Global.animations["HUDTextBox"];
	        this.dialogs = new List<List<string>>();
	        for (int i = 0; i < dialogs.Count; i++)
	        {
		        string dialog = dialogs[i];
		        this.dialogs.Add(clampDialog(dialog));
	        }
        }

        public void update()
        {
            if (dialogIndex >= dialogs.Count) return;
            letterIndex++;
            if (letterIndex > dialogs[dialogIndex][rowIndex].Length - 1)
            {
                if (rowIndex == dialogs[dialogIndex].Count - 1)
                {
                    letterIndex = dialogs[dialogIndex][rowIndex].Length;
                    if (Global.input.isPressed(Control.main.Action))
                    {
                        letterIndex = 0;
                        rowIndex = 0;
                        dialogIndex++;
                    }
                }
                else
                {
                    letterIndex = 0;
                    rowIndex++;
                }
            }
        }

        public void render()
        {
            Point boxPos = new Point(33, 154);
            Point textPos = boxPos + new Point(10, 8);
            boxSprite.draw(boxPos.x, boxPos.y, 1, 1, 0, 1, null, (int)ZIndex.HUD, false);

            for (int i = 0; i < rowIndex + 1; i++)
            {
                //Last one: draw substring
                if (i == rowIndex)
                {
                    string textStr = dialogs[dialogIndex][i].Substring(0, (int)Mathf.Floor(letterIndex));
                    Helpers.drawTextStd(textStr, textPos.x, textPos.y + (i * 16));
                }
                //Prev row: draw whole thing
                else
                {
                    string textStr = dialogs[dialogIndex][i];
                    Helpers.drawTextStd(textStr, textPos.x, textPos.y + (i * 16));
                }
            }
        }

        public bool isDone()
        {
            return dialogIndex >= dialogs.Count;
        }

    }
}
