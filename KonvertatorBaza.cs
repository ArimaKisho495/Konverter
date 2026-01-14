using System;
using System.Collections.Generic;
using System.Linq;


namespace Konverter
{
    public class KonvertatorBaza
    {
        private readonly List<IKonvertatorModule> modules = new();


        public IEnumerable<string> Categories =>
        modules.Select(m => m.Category).Distinct();


        public void RegisterModule(IKonvertatorModule module)
        {
            modules.Add(module);
        }


        public IEnumerable<UnitDef> GetUnits(string category)
        {
            return modules
            .Where(m => m.Category == category)
            .SelectMany(m => m.Units);
        }


        public double Convert(string category, string from, string to, double value)
        {
            var module = modules.FirstOrDefault(m => m.Category == category);
            if (module == null)
                throw new Exception($"Нет модуля для категории: {category}");


            return module.Convert(from, to, value);
        }
    }
}
