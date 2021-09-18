using System.Collections.Generic;

namespace Jammo.TextAnalysis
{
    public abstract class Inspector<TInspection, TInspectionRule, TAnalysisCompilation>
        where TInspection : Inspection<TInspectionRule>
        where TInspectionRule : InspectionRule
        where TAnalysisCompilation : AnalysisCompilationBase
    {
        protected readonly List<TInspectionRule> InternalRules = new();
        protected readonly List<TInspection> InternalInspections = new();
        
        public IEnumerable<TInspection> Inspections => InternalInspections;
        
        public void AddInspection(TInspection cSharpInspection)
        {
            InternalInspections.Add(cSharpInspection);
        }

        public abstract void Inspect(TAnalysisCompilation context);

        public void AddRules(params IEnumerable<TInspectionRule>[] sets)
        {
            foreach (var ruleSet in sets)
                InternalRules.AddRange(ruleSet);
        }
    }
}