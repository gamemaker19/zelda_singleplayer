﻿namespace GameEditor.Editor
{
    public class GlobalInput
    {
        /*
        keysHeld: Set<KeyCode> = new Set();

        constructor()
        {
            document.onkeydown = (e: KeyboardEvent) => {
                if (this.isTextBox(document.activeElement)) return;
                let keyCode = < KeyCode > e.keyCode;
                this.onKeyDown(keyCode, !this.keysHeld.has(keyCode));
                this.keysHeld.add(keyCode);
                //e.preventDefault();
            }

            document.onkeyup = (e: KeyboardEvent) => {
                if (this.isTextBox(document.activeElement)) return;
                let keyCode = < KeyCode > e.keyCode;
                this.keysHeld.delete(keyCode);
                this.onKeyUp(e.keyCode);
                //e.preventDefault();
            }
        }

        onKeyDown(keyCode: KeyCode, firstFrame: boolean)
        {
        }

        onKeyUp(keyCode: KeyCode)
        {

        }

        isTextBox(element: Element)
        {
            if (!element) return false;
            var tagName = element.tagName.toLowerCase();
            if (tagName === 'textarea') return true;
            if (tagName !== 'input') return false;
            var type = element.getAttribute('type').toLowerCase(),
                // if any of these input types is not supported by a browser, it will behave as input type text.
                inputTypes = ['text', 'password', 'number', 'email', 'tel', 'url', 'search', 'date', 'datetime', 'datetime-local', 'time', 'month', 'week']
          return inputTypes.indexOf(type) >= 0;
        }
        */
    }
}
