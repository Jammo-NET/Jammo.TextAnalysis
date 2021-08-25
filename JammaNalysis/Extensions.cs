using System.Collections.Generic;
using System.Linq;
using JammaNalysis.CsFileAnalysis;

namespace JammaNalysis
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