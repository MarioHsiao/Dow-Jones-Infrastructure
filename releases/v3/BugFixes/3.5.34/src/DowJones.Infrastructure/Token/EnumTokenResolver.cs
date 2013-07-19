using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DowJones.Attributes;
using DowJones.DependencyInjection;
using DowJones.Extensions;
using DowJones.Infrastructure;

namespace DowJones.Token
{
    public class EnumTokenResolver
    {
        private readonly IDictionary<Type, IDictionary<string, string>> _assignedTokensByEnumType;

        public EnumTokenResolver(IDictionary<Type, IDictionary<string, string>> assignedTokensByEnumType)
        {
            _assignedTokensByEnumType = assignedTokensByEnumType ?? new Dictionary<Type, IDictionary<string, string>>();
        }

        [Inject("Disambiguating multiple constructors")]
        public EnumTokenResolver(IAssemblyRegistry assemblyRegistry)
        {
            _assignedTokensByEnumType = GetAssignedTokensByEnumType(assemblyRegistry);
        }

        public string GetTokenName(Enum value)
        {
            if(value == null)
                return null;

            string tokenName = null;
            var enumMemberName = value.ToString();
            
            IDictionary<string, string> memberTokens;
            if (_assignedTokensByEnumType.TryGetValue(value.GetType(), out memberTokens))
                memberTokens.TryGetValue(enumMemberName, out tokenName);

            return tokenName ?? enumMemberName;
        }

        private IDictionary<Type, IDictionary<string, string>> GetAssignedTokensByEnumType(IAssemblyRegistry assemblyRegistry)
        {
            var knownEnums = assemblyRegistry.ExportedTypes.Where(x => x.IsEnum);

            var tokenNamesByEnumType =
                from enumType in knownEnums
                from member in enumType.GetMembers()
                let token = GetAssignedToken(member)
                where token.HasValue()
                let memberTokenPair = new KeyValuePair<string, string>(member.Name, token)
                group memberTokenPair by enumType into enumTypeMemberTokens
                select enumTypeMemberTokens;

            return tokenNamesByEnumType.ToDictionary();
        }

        private static string GetAssignedToken(MemberInfo member)
        {
            var attributes = member.GetCustomAttributes(typeof(AssignedToken),false);
            var assignedToken = attributes.OfType<AssignedToken>().FirstOrDefault();
            var hasAssignedToken = assignedToken != null && assignedToken.Token.HasValue();
            return hasAssignedToken ? assignedToken.Token : null;
        }
    }
}
