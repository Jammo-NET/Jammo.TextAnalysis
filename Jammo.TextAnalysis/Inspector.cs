using System.Collections.Generic;

namespace Jammo.TextAnalysis
{
    public abstract class Inspector
    {
        protected readonly List<InspectionRule> InternalRules = new();
        protected readonly List<Diagnostic> InternalInspections = new();
        
        public IEnumerable<Diagnostic> Inspections => InternalInspections;
        
        public void AddInspection(Diagnostic diagnostic)
        {
            InternalInspections.Add(diagnostic);
        }

        public abstract void Inspect(AnalysisCompilation context);

        public void AddRule(InspectionRule rule)
        {
            InternalRules.Add(rule);
        }

        public void AddRules(params InspectionRule[] rules)
        {
            InternalRules.AddRange(rules);
        }

        public void AddRules(params IEnumerable<InspectionRule>[] sets)
        {
            foreach (var ruleSet in sets)
                AddRules(ruleSet);
        }
    }
}