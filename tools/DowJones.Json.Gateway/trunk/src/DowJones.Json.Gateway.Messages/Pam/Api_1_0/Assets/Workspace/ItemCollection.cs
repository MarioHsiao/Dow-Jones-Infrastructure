using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace DowJones.Json.Gateway.Messages.Pam.Api_1_0.Assets.Workspace
{
    [JsonArray]
    [Serializable]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public class ItemCollection : List<Item>
    {
    }
}
