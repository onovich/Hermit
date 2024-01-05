using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Polyperfect.Common;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Polyperfect.War
{
    public class SceneTargetsManager : PolyMono
    {
        public override string __Usage => "Manages tracking of targetable items. Only one should be in the scene, and will be created automatically if it does not exist";

        readonly Dictionary<Type, HashSet<ITargetable>> targetableTypeToTargetableCollectionLookup = new Dictionary<Type, HashSet<ITargetable>>();
        readonly Dictionary<TrackerEntry, HashSet<Tracker>> trackerEntryToTrackerCollectionLookup = new Dictionary<TrackerEntry, HashSet<Tracker>>();
        
        //the Keys are the tracker's Type
        readonly Dictionary<Type, HashSet<TrackerTargetPair>> typedExistingPairs = new Dictionary<Type, HashSet<TrackerTargetPair>>();
        readonly Dictionary<Type, List<TrackerTargetPair>> typedNewPairs = new Dictionary<Type, List<TrackerTargetPair>>();
        readonly Dictionary<Type, HashSet<TrackerTargetPair>> typedToRemove= new Dictionary<Type, HashSet<TrackerTargetPair>>();
        readonly Dictionary<Type, List<TrackerTargetPair>> typedAddPairs = new Dictionary<Type, List<TrackerTargetPair>>();

        struct TrackerEntry
        {
            public Type SeekerType;
            public Type TargetableType;

            public static TrackerEntry Create<T>(Tracker<T> source) where T: class, ITargetable
            {
                return new TrackerEntry {SeekerType = source.GetType(), TargetableType = typeof(T)};
            }
        }
        static SceneTargetsManager Instance => instance;
        static SceneTargetsManager instance;
        public static void RegisterTargetable(ITargetable targetable)
        {
            var testType = targetable.GetType();
            while (testType!=null&&typeof(ITargetable).IsAssignableFrom(testType))
            {
                EnsureTargetableCollectionExists(testType);
                Instance.targetableTypeToTargetableCollectionLookup[testType].Add(targetable);
                testType = testType.BaseType;
            }
        }

        public static void UnregisterTargetable(ITargetable targetable)
        {
            var testType = targetable.GetType();
            while (testType!=null&&typeof(ITargetable).IsAssignableFrom(testType)) 
            {
                Instance.targetableTypeToTargetableCollectionLookup[testType].Remove(targetable);
                testType = testType.BaseType;
            }
            
            
            var removeList = new List<TrackerTargetPair>();
            foreach (var entry in Instance.typedExistingPairs)
            {

                foreach (var thing in entry.Value)
                {
                    if (thing.Target == targetable)
                    {
                        thing.Tracker.DoRemoveTargetable(thing.Target);
                        removeList.Add(thing);
                    }
                }

                foreach (var item in removeList)
                {
                    entry.Value.Remove(item);
                }
            }
        }

        static void EnsureTargetableCollectionExists(Type type)
        {
            if (!Instance.targetableTypeToTargetableCollectionLookup.ContainsKey(type))
                Instance.targetableTypeToTargetableCollectionLookup.Add(type, new HashSet<ITargetable>());
        }

        public static void RegisterSeeker<T>(Tracker<T> tracker) where T:class,ITargetable
        {
            var entry = TrackerEntry.Create(tracker);
            EnsureSeekerCollectionExists(entry);
            Instance.trackerEntryToTrackerCollectionLookup[entry].Add(tracker);
        }

        static void EnsureSeekerCollectionExists(TrackerEntry entry)
        {
            if (!Instance.trackerEntryToTrackerCollectionLookup.ContainsKey(entry))
            {
                Instance.trackerEntryToTrackerCollectionLookup.Add(entry, new HashSet<Tracker>());
                Instance.typedExistingPairs.Add(entry.SeekerType, new HashSet<TrackerTargetPair>());
                Instance.typedNewPairs.Add(entry.SeekerType,new List<TrackerTargetPair>());
                Instance.typedToRemove.Add(entry.SeekerType,new HashSet<TrackerTargetPair>());
                Instance.typedAddPairs.Add(entry.SeekerType, new List<TrackerTargetPair>());
                Instance.typeList.Add(entry.SeekerType);
            }
        }

        public static void UnregisterSeeker<T>(Tracker<T> tracker) where T:class,ITargetable
        {
            Instance.trackerEntryToTrackerCollectionLookup[TrackerEntry.Create(tracker)].Remove(tracker);
            Instance.typedExistingPairs[tracker.GetType()].RemoveWhere(p => p.Tracker == tracker);
        }
        
        IEnumerable<ITargetable> TargetsInRange(TrackerEntry entry, Vector3 position, float radius)
        {
            var sqrDist = radius * radius;
            EnsureTargetableCollectionExists(entry.TargetableType);
            foreach (var item in targetableTypeToTargetableCollectionLookup[entry.TargetableType])
            {
                if ((item.Position - position).sqrMagnitude < sqrDist)
                {
                    yield return item;
                }
            }
        }
        

        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Init()
        {
            instance = new GameObject("Target Manager").AddComponent<SceneTargetsManager>();
            DontDestroyOnLoad(instance.gameObject);
        }



        readonly List<Type> typeList = new List<Type>();
        Type trackerType;
        int typeIndex = 0;
        void Update()
        {
            if (typeList.Count <= 0)
                return;
            typeIndex++;
            if (typeIndex >= typeList.Count)
                typeIndex = 0;
            trackerType = typeList[typeIndex];
            ConstructNewPairs();

            CopyRemoves();
            DetermineAddAndRemove();
            DoRemove();
            DoAdd();
            
            foreach (var item in typedAddPairs) 
                item.Value.Clear();
            foreach (var item in typedToRemove) 
                item.Value.Clear();
        }

        void CopyRemoves()
        {
            Profiler.BeginSample(nameof(CopyRemoves));
            foreach (var entry in typedExistingPairs)
            {
                if (entry.Key != trackerType)
                    continue;
                foreach (var item in entry.Value)
                {
                    typedToRemove[entry.Key].Add(item);
                }
            }
            Profiler.EndSample();
        }

        void DetermineAddAndRemove()
        {
            Profiler.BeginSample(nameof(DetermineAddAndRemove));
            foreach (var entry in typedNewPairs)
            {
                if (entry.Key != trackerType)
                    continue;
                
                foreach (var item in entry.Value)
                {
                    if (typedExistingPairs[entry.Key].Contains(item))
                        typedToRemove[entry.Key].Remove(item);
                    else
                        typedAddPairs[entry.Key].Add(item);
                }
            }
            Profiler.EndSample();
        }

        void DoAdd()
        {
            Profiler.BeginSample(nameof(DoAdd));
            foreach (var entry in typedAddPairs)
            {
                if (entry.Key != trackerType)
                    continue;
                foreach (var item in entry.Value)
                {
                    //Debug.LogError("Adding");
                    typedExistingPairs[entry.Key].Add(item);
                    item.Tracker.DoAddTargetable(item.Target);
                }
            }   
            Profiler.EndSample();
        }

        void DoRemove()
        {
            Profiler.BeginSample(nameof(DoRemove));
            foreach (var entry in typedToRemove)
            {
                if (entry.Key != trackerType)
                    continue;
                foreach (var item in entry.Value)
                {
                    typedExistingPairs[entry.Key].Remove(item);
                    item.Tracker.DoRemoveTargetable(item.Target);
                }
            }
            Profiler.EndSample();
        }

        void ConstructNewPairs()
        {
            Profiler.BeginSample(nameof(ConstructNewPairs));
            foreach (var item in typedNewPairs) 
                item.Value.Clear();
            foreach (var entry in trackerEntryToTrackerCollectionLookup)
            {
                if (entry.Key.SeekerType != trackerType)
                    continue;
                foreach (var tracker in entry.Value)
                {
                    typedNewPairs[entry.Key.SeekerType].AddRange(TargetsInRange(entry.Key,tracker.transform.position,tracker.Radius)
                        .Select(t=>new TrackerTargetPair(tracker,t))
                        .Where(t=>t.Tracker.ShouldTrack(t.Target)));
                }
            }
            Profiler.EndSample();
        }
    }
}