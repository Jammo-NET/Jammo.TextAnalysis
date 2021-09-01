using System.Collections.Generic;
using System.Linq;
using Jammo.CsAnalysis.CsFileAnalysis;

namespace Jammo.CsAnalysis
{
    public static class Extensions
    {
        public static bool TryGetModifier(this IEnumerable<MemberModifier> modifiers, DeclarationModifier modifier, out MemberModifier member)
        {
            member = modifiers.FirstOrDefault(m => m.Modifier == modifier);

            return member != null;
        }
    }
}