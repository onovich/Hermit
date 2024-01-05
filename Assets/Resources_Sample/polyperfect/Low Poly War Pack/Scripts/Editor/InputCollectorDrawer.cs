using System;
using System.Collections.Generic;
using Polyperfect.Common;
using Polyperfect.Common.Edit;
using UnityEditor;
using UnityEngine.UIElements;

namespace Polyperfect.War
{
    [CustomEditor(typeof(InputCollector))]
    public class InputCollectorDrawer : PolyMonoEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var ve = new VisualElement();
            ve.Add(base.CreateInspectorGUI());
            var collector = (InputCollector) target;
            var controlContainer = new VisualElement();
            ve.Add(controlContainer);
            UpdateContainer(controlContainer, collector);
            ve.schedule.Execute(() => UpdateContainer(controlContainer, collector)).Every(2007);
            return ve;
        }

        static void UpdateContainer(VisualElement controlContainer, InputCollector collector)
        {
            controlContainer.Clear();
            foreach (var item in collector.Buttons)
            {
                var container = new VisualElement();
                container.style.flexGrow = 1f;
                container.style.flexDirection = FlexDirection.Row;
                container.style.justifyContent = Justify.SpaceBetween;
                container.Add(new Label());
                var label = new Label(item.Key.Name);
                label.style.flexGrow = 1f;
                var val = new Label();
                container.Add(label);
                container.Add(val);
                controlContainer.Add(container);
                UpdateValueText(val,()=>(item.Value.RecentlyActivated ? "Recent" : "") + item.Value.Value.ToString());
            }
            foreach (var item in collector.Axes)
            {
                var container = new VisualElement();
                container.style.flexGrow = 1f;
                container.style.flexDirection = FlexDirection.Row;
                container.style.justifyContent = Justify.SpaceBetween;
                container.Add(new Label());
                var label = new Label(item.Key.Name);
                label.style.flexGrow = 1f;
                var val = new Label();
                container.Add(label);
                container.Add(val);
                controlContainer.Add(container);
                UpdateValueText(val,()=>$"d:{item.Value.DirectVector}, w:{item.Value.WorldVector}");
            }
        }

        static void UpdateValueText(TextElement textElement,Func<string> func)
        {
            textElement.text = func();
            textElement.schedule.Execute(() => textElement.text = func()).Every(57);
        }
    }
}