using System;
using System.Collections.Generic;

using System.IO;
using Newtonsoft.Json;

namespace DataAccess
{
    public class ProvinciasJSON
    {
        public string Path { get; set; }

        public ProvinciasJSON(string path)
        {
            Path = path;
        }

        public List<Provincia> GetProvincias()
        {
            string jsonString = File.ReadAllText(Path);

            try
            {
                // Read provincias from local json
                List<Provincia> provincias = JsonConvert.DeserializeObject<List<Provincia>>(jsonString);
                return provincias;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        public bool saveProvincias(List<Provincia>? provincias)
        {
            try
            {
                // Serialize current provincias
                string jsonString = JsonConvert.SerializeObject(provincias);
                // Save current provincias
                File.WriteAllText(Path, jsonString);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
