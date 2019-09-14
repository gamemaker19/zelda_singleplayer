using System;
using System.Collections.Generic;
using System.Text;

namespace ZFG_CS
{
    public class Stat
    {
        public float value = 0;
        public float maxValue = 0;
        public float amountToAdd = 0;
        public float addRate = 0;
        public float addTime = 0;
        public float maxValueCap = 1000000;
        public StatType statType;
        public Actor actor;
        
        public Stat()
        {
        }

        public Stat(StatType statType, float maxValue, float addRate, Actor actor)
        {
            this.value = maxValue;
            this.maxValue = maxValue;
            this.addRate = addRate;
            this.statType = statType;
            this.actor = actor;
        }

        public Stat(StatType statType, float value, float maxValue, float addRate, Actor actor)
        {
            this.value = value;
            this.maxValue = maxValue;
            this.addRate = addRate;
            this.statType = statType;
            this.actor = actor;
        }

        public void update()
        {
            if (amountToAdd > 0)
            {
                addTime += Global.spf;
                if (statType == StatType.Rupee) actor.playLoopingSound("wallet 1", -0.25f);
                if (addTime >= 1 / addRate)
                {
                    addTime = 0;
                    amountToAdd--;
                    value++;
                    if (statType == StatType.Health) actor.playSound("life refill");
                    else if (statType == StatType.Magic) actor.playSound("magic meter 1");
                    if (value > maxValue)
                    {
                        value = maxValue;
                        amountToAdd = 0;

                    }
                }
            }
        }

        public void addImmediate(float amount)
        {
            value += amount;
            if (value > maxValue) value = maxValue;
        }

        public void add(float amount)
        {
            amountToAdd += amount;
            addTime = (1 / addRate);
        }

        public void deduct(float amount)
        {
            value -= amount;
            if (value < 0) value = 0;
        }

        public bool tryDeduct(float amount)
        {
            if (amount > value)
            {
                if (actor.getChar() == Global.game.character)
                {
                    if (statType == StatType.Arrow) Global.game.setCurrentMessage("Not enough arrows", 5);
                    else if (statType == StatType.Magic) Global.game.setCurrentMessage("Not enough magic", 5);
                    actor.playSound("error");
                }
                return false;
            }
            deduct(amount);
            return true;
        }

        public void fillMax()
        {
            float rest = maxValue - value;
            add(rest);
        }

        public bool isMax()
        {
            return value == maxValue;
        }

        public bool isChanging()
        {
            return amountToAdd > 0;
        }

        public void increaseMaxValue(float amount)
        {
            if (maxValue >= maxValueCap) return;
            maxValue += amount;
        }
    }
}
