using System;
using System.Collections.Generic;
using System.Linq;
using DowJones.Mapping;

namespace DowJones.Web.Mvc.UI
{
    public class ViewComponentModelMapper
    {
        private readonly GenericTypeMapper _mapper;

        public ViewComponentModelMapper(IEnumerable<ViewComponentModelMapping> mappings)
        {
            _mapper = new GenericTypeMapper(mappings.Select(x => x.GenericTypeMapping));
        }

        public Type GetComponentTypeFromDataType(Type dataType)
        {
            return _mapper.GetDeclaringTypeByGenericType(dataType);
        }
    }
}