using System.Collections.Generic;

namespace Jammo.TextAnalysis
{
    public abstract class Inspector<TInspectionRule, TDiagnostic, TAnalysisCompilation>
        where TInspectionRule : InspectionRule
        where TDiagnostic : Diagnostic
        where TAnalysisCompilation : AnalysisCompilation
    {
        protected readonly List<TInspectionRule> InternalRules = new();
        protected readonly List<TDiagnostic> InternalDiagnostics = new();
        
        public IEnumerable<TDiagnostic> Diagnostics => InternalDiagnostics;

        public void AddDiagnostic(TDiagnostic diagnostic)
        {
            InternalDiagnostics.Add(diagnostic);
        }

        public abstract void Inspect(TAnalysisCompilation context);

        public void AddRule(TInspectionRule rule)
        {
            InternalRules.Add(rule);
        }

        public void AddRules(params TInspectionRule[] rules)
        {
            InternalRules.AddRange(rules);
        }

        public void AddRules(params IEnumerable<TInspectionRule>[] sets)
        {
            foreach (var ruleSet in sets)
                AddRules(ruleSet);
        }
    }
}