namespace Polyperfect.War
{
    public class ObjectiveSeeker_AI : SeekerBase_AI<Objective_Tracker,Objective_Target>
    {
        public override string __Usage => "Makes the unit seek out the objectives in range.";
        protected override FuzzyLayer FuzzyLayer => FuzzyLayer.Movement;
    }
}