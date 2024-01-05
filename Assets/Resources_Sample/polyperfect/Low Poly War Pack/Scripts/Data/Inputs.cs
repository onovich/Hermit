using UnityEngine;

namespace Polyperfect.War
{
    
    public static partial class Inputs
    {
        public static readonly ButtonInputReference Run = new ButtonInputReference("Run");
        public static readonly ButtonInputReference Shoot = new ButtonInputReference("Shoot");
        public static readonly ButtonInputReference Reload = new ButtonInputReference("Reload");
        public static readonly ButtonInputReference Jump = new ButtonInputReference("Jump");
        public static readonly ButtonInputReference Aim = new ButtonInputReference("Aim");
        public static readonly ButtonInputReference Next = new ButtonInputReference("Next");
        public static readonly ButtonInputReference Prev = new ButtonInputReference("Prev");
        public static readonly ButtonInputReference Interact = new ButtonInputReference("Interact");
        public static readonly MultiAxisInputReference Tilt = new MultiAxisInputReference("Tilt");
    }
    
    public class ButtonInputReference
    {
        public string Name { get; }
        public ButtonInputReference(string name)
        {
            Name = name;
        }
        
    }

    public class MultiAxisInputReference
    {
        public string Name { get; }
        public MultiAxisInputReference(string name)
        {
            Name = name;
        }
    }
    public delegate void ActivationDelegate();
    public class BoolState
    {
        const float BUFFER_DURATION = .1f;
        public bool Value { get; private set; }
        public bool RecentlyActivated => Time.time - lastPressedTime < BUFFER_DURATION;
        float lastPressedTime = float.MinValue;
        public void Set(bool value)
        {
            if (Value == value)
                return;
            Value = value;
            if (value)
            {
                lastPressedTime = Time.time;
                OnActivate();
            }
            else
                OnDeactivate();
        }

        public event ActivationDelegate OnActivate = () => { };
        public event ActivationDelegate OnDeactivate = () => { };
        
    }

    public class AxisInput
    {
        public Vector2 DirectVector { get; private set; }
        public Vector3 WorldVector { get; private set; }

        public void Set(Vector2 directTilt, Vector3 worldTilt)
        {
            DirectVector = directTilt;
            WorldVector = worldTilt;
        }
    }
}