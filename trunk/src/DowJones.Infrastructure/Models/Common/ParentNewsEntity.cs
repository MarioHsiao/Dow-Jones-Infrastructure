﻿using System.Runtime.Serialization;

namespace DowJones.Models.Common
{
    [DataContract(Name = "parentNewsEntity", Namespace = "")]
    public class ParentNewsEntity : NewsEntity
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "newsEntities", EmitDefaultValue = false)]
        public NewsEntities NewsEntities { get; set; }
    }
}