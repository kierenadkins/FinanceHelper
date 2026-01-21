using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using FinanceHelper.Application.Interfaces;
using Newtonsoft.Json;

namespace FinanceHelper.Application.Services.Sterializer
{
    public class SterializerService : ISterializerService
    {
        public string SerializeObject(object obj)
        {
            var jsonTypeNameAll = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
            return jsonTypeNameAll;
        }

        public T DeserializeObject<T>(string obj)
        {
            var item = JsonConvert.DeserializeObject<T>(obj, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            });

            return item;
        }
    }
}
