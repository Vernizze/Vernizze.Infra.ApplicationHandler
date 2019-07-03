using Mapster;
using Newtonsoft.Json;
using Vernizze.Infra.ApplicationHandler.VOs;
using Vernizze.Infra.CrossCutting.Utils;
using Vernizze.Infra.CrossCutting.Extensions;
using System;
using System.Reflection;
using System.Linq;

namespace Vernizze.Infra.ApplicationHandler.ObjectMapping
{
    public class Mapper
    {
        static Mapper()
        {
            new Mapper().InitConfigs();
        }

        private void InitConfigs()
        {
            var mapping_configs = ReadConfigs();

            if (mapping_configs.IsNotNull() && mapping_configs.configs.HaveAny())
            {
                mapping_configs.configs.ForEach(i =>
                {
                    string typeName01 = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".", i.object_name_origin);
                    Type MyType01 = Type.GetType(typeName01);

                    string typeName02 = string.Concat(Assembly.GetExecutingAssembly().GetName().Name, ".", i.object_name_destiny);
                    Type MyType02 = Type.GetType(typeName02);

                    var typeOfContext = this.GetType();

                    var method = typeOfContext.GetMethod("Config");

                    var genericMethod = method.MakeGenericMethod(MyType01, MyType02);

                    genericMethod.Invoke(this, new object[] { i.relations.ToArray() });
                });
            }
        }

        public static void Init() { }

        public void Config<TOrigin, TDestiny>(object[] parameters)
        {
            var relations = parameters.ToList();

            var adapter = TypeAdapterConfig<TOrigin, TDestiny>.NewConfig();

            if (relations.HaveAny())
                relations.ForEach(r =>
                {
                    adapter.Map((r as ObjectMappingRelationsConfigs).atribute_destiny, (r as ObjectMappingRelationsConfigs).atribute_origin);
                });
        }

        public static TDestiny Map<TOrigin, TDestiny>(TOrigin origin)
        {
            return origin.Adapt<TDestiny>();
        }

        private static ObjectMappingConfigs ReadConfigs()
        {
            var configs_string_content = FilesAndFolders.GetFileContent("object_mapping.json");

            return JsonConvert.DeserializeObject<ObjectMappingConfigs>(configs_string_content); ;
        }
    }
}