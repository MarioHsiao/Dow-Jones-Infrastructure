using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Extensions;
using DowJones.Mapping;

namespace DowJones.Web.Mvc.Diagnostics.Mapping
{
    public class MapperDebuggerViewModel
    {
        public IEnumerable<TypeMapperViewModel> TypeMappers { get; set; }

        public int TypeMapperCount
        {
            get { return TypeMappers.Count(); }
        }

        public MapperDebuggerViewModel()
        {
            TypeMappers = Enumerable.Empty<TypeMapperViewModel>();
        }

        public MapperDebuggerViewModel(IEnumerable<TypeMapperDefinition> typeMappers)
        {
            TypeMappers = typeMappers.Select(x => new TypeMapperViewModel(x));
        }
    }

    public class TypeMapperViewModel
    {
        public string MapperName { get; set; }

        public string SourceTypeName { get; set; }
        
        public string TargetTypeName { get; set; }

        public TypeMapperViewModel()
        {
        }
        public TypeMapperViewModel(TypeMapperDefinition definition)
        {
            SourceTypeName = definition.SourceType == null ? "[Unspecified]" : definition.SourceType.FullName;

            if (TargetTypeName == null || (TargetTypeName.IsEmpty() && definition.TargetType != null))
                TargetTypeName = definition.TargetType.ToString();

            MapperName = MapperName ?? "[Unspecified]";

            try
            {
                var mapperFactory = definition.Factory;
                if (mapperFactory != null)
                {
                    var mapper = mapperFactory();
                    MapperName = mapper.ToString();
                }
            }
            catch(Exception ex)
            {
                MapperName = string.Format("[EXCEPTION: {0}]", ex.Message);
            }
        }
    }
}