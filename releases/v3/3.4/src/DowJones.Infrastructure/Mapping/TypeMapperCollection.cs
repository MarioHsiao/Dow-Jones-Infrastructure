using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DowJones.Mapping
{
    public class TypeMapperCollection : IEnumerable<TypeMapperDefinition>
    {
        private readonly Dictionary<string, TypeMapperDefinition> _sources;

        public TypeMapperCollection()
        {
            _sources = new Dictionary<string, TypeMapperDefinition>();
        }

        public TypeMapperCollection(IEnumerable<TypeMapperDefinition> definitions)
        {
            foreach (var definition in definitions)
            {
                Add(definition);
            }
        }

        public void Add(TypeMapperDefinition definition)
        {
            if (!_sources.ContainsKey(definition.SourceType.FullName))
            {
                _sources.Add(definition.SourceType.FullName, definition);
            }
        }
        
        public IEnumerable<TypeMapperDefinition> FindBySourceType(Type sourceType)
        {
            if (sourceType == null)
                return new List<TypeMapperDefinition>();

            TypeMapperDefinition temp;
            return (_sources.TryGetValue(sourceType.FullName, out temp)) ?
                       new List<TypeMapperDefinition>(new[] {temp}) :
                       new List<TypeMapperDefinition>();
        }

        public IEnumerable<TypeMapperDefinition> FindBySourceTypeAndTargetType(Type sourceType, Type targetType)
        {
            TypeMapperDefinition temp;
            return (_sources.TryGetValue(sourceType.FullName, out temp) && temp.TargetType == targetType) ?
                       new List<TypeMapperDefinition>(new[] {temp}) :
                       new List<TypeMapperDefinition>();
        }

        public IEnumerator<TypeMapperDefinition> GetEnumerator()
        {
            return _sources.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _sources.Values.GetEnumerator();
        }
    }
}