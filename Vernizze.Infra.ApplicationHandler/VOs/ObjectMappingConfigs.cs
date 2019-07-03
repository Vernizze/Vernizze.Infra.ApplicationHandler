using System.Collections.Generic;

namespace Vernizze.Infra.ApplicationHandler.VOs
{
    internal class ObjectMappingConfigs
    {
        public List<ObjectMappingConfig> configs { get; set; }
    }

    internal class ObjectMappingConfig
    {
        public string object_name_origin { get; set; }

        public string object_name_destiny { get; set; }

        public List<ObjectMappingRelationsConfigs> relations { get; set; }
    }

    internal class ObjectMappingRelationsConfigs
    {
        public string atribute_origin { get; set; }
        public string atribute_destiny { get; set; }
    }
}
