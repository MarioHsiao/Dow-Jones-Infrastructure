using System;
using System.Collections;
using System.Data;
using DowJones.Utilities.DTO.Web.LOB;
using DowJones.Utilities.DTO.Web.Request;

namespace DowJones.Utilities.DTO.Web
{
    public class DTOKeyMapper
    {
        protected static Hashtable _dtoToKeyMapper = new Hashtable(13);
        private static int _lastInsertedIdentity;

        static DTOKeyMapper()
        {
            InsertDTOMapping(typeof (SessionRequestDTO));
            InsertDTOMapping(typeof (AbstractRequestDTO));
        }

        public static void InsertDTOMapping(Type type)
        { 
            if (!_dtoToKeyMapper.ContainsKey(type))
            {
                _dtoToKeyMapper.Add(type, _lastInsertedIdentity++);
            }
        }

        public static string GetDtoId(Type type)
        {
            object obj = _dtoToKeyMapper[type];
            if (obj != null)
            {
                return obj.ToString();
            }
            throw new NoNullAllowedException("Missing DTO id for: " + type.FullName);
        }
    }
}