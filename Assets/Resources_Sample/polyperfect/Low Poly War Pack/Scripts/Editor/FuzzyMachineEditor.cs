using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Polyperfect.Common;
using Polyperfect.Common.Edit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Polyperfect.War
{
    [CustomEditor(typeof(FuzzyMachine))]
    public class FuzzyMachineEditor : PolyMonoEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var ve = new VisualElement();
            ve.Add(base.CreateInspectorGUI());
            if (!Application.isPlaying)
            {
                ve.Add(new NoteBox($"Enter play mode to see weights of attached {nameof(FuzzyComponent)}s."));
                return ve;
            }

            var fuzzyMachine = (FuzzyMachine) target;
            foreach (var entry in fuzzyMachine.FuzzyStates)
            {
                var layer = entry.Key;
                var layerVe = new VisualElement();
                var layerLabel = new Label(layer.ToString());
                layerLabel.style.marginTop = 10f;
                layerLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                layerVe.Add(layerLabel);
                foreach (var component in entry.Value)
                {
                    var componentContainer = new VisualElement();
                    componentContainer.style.flexDirection = FlexDirection.Row;
                    var componentLabel = new Label(component.GetType().Name);
                    componentLabel.style.flexGrow = 1f;
                    componentContainer.Add(componentLabel);
                    var weightLabel = new Label();
                    weightLabel.schedule.Execute(() => { weightLabel.text = component.GetWeight().ToString(CultureInfo.InvariantCulture); }).Every(100);
                    componentContainer.Add(weightLabel);
                    layerVe.Add(componentContainer);
                } 

                ve.Add(layerVe);
            }

            return ve;
        }
    }

    [CustomEditor(typeof(Tracker), true)]
    public class TrackerEditor : PolyMonoEditor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var ve = new VisualElement();
            ve.Add(base.CreateInspectorGUI());

            var tracker = (Tracker) target;
            var trackerContainer = new VisualElement();
            trackerContainer.schedule.Execute(() => UpdateTrackerContainer(trackerContainer, tracker)).Every(133);
            
            ve.Add(trackerContainer);
            return ve;
        }

        void UpdateTrackerContainer(VisualElement ve,Tracker tracker)
        {
            if (!ve.enabledInHierarchy)
                return;
            ve.Clear();
            var position = tracker.transform.position;
            foreach (var item in tracker.TrackedTargetables)
            {
                Debug.DrawLine(position,item.Position,Color.yellow,.133f);
                ve.Add(new Label(item.gameObject.name));
            }
        }
    }
}