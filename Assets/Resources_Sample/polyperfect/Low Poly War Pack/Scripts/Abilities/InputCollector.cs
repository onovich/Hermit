using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Polyperfect.Common;
using UnityEngine;

namespace Polyperfect.War
{

    [DisallowMultipleComponent]
    public class InputCollector : PolyMono
    {
        public override string __Usage => "Central location for accessing inputs from scripts, whether they come from the Player, AI, or otherwise.";

        readonly Dictionary<ButtonInputReference, BoolState> buttons = new Dictionary<ButtonInputReference, BoolState>();

        readonly Dictionary<MultiAxisInputReference, AxisInput> axes = new Dictionary<MultiAxisInputReference, AxisInput>();
        public IReadOnlyDictionary<ButtonInputReference, BoolState> Buttons => buttons;
        public IReadOnlyDictionary<MultiAxisInputReference, AxisInput> Axes => axes;

        public event Action<InputCollector> OnUpdate;
        
        void Update()
        {
            OnUpdate?.Invoke(this);
        }
        public void CopyFrom(InputCollector source)
        {
            foreach (var buttonInput in buttons)
            {
                if (source.buttons.TryGetValue(buttonInput.Key, out var copy))
                    buttonInput.Value.Set(copy.Value);
            }
            foreach (var axesInput in axes)
            {
                if (source.axes.TryGetValue(axesInput.Key, out var copy))
                    axesInput.Value.Set(copy.DirectVector,copy.WorldVector);
            }
        }

        public void ResetToDefaults()
        {
            foreach (var item in buttons) 
                item.Value.Set(false);
            foreach (var item in axes) 
                item.Value.Set(Vector2.zero, Vector3.zero);
        }

        public BoolState this[ButtonInputReference that]
        {
            get
            {
                if (!buttons.ContainsKey(that))
                    buttons.Add(that,new BoolState());
                return buttons[that];
            }
        }

        public AxisInput this[MultiAxisInputReference that]
        {
            get
            {
                if (!axes.ContainsKey(that))
                    axes.Add(that,new AxisInput());
                return axes[that]; 
                
            }
        }
    }
}