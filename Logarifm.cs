using System;
using System.Collections.Generic;
// функции логарифма
namespace Konverter
{
    public class Logarifm : IKonvertatorModule
    {
        public string Category => "Логарифмы";

        public IEnumerable<UnitDef> Units => new[]
        {
            new UnitDef("log10", "Log base 10", "log10"),
            new UnitDef("ln", "Natural Log", "ln"),
            new UnitDef("log2", "Log base 2", "log2")
        };

        public double Convert(string from, string to, double value)
        {
            // Перевод в линейное пространство
            double linear = from switch
            {
                "log10" => Math.Pow(10, value),
                "ln" => Math.Exp(value),
                "log2" => Math.Pow(2, value),
                _ => throw new Exception("Неизвестная единица")
            };

            if (linear <= 0)
                throw new Exception("Аргумент логарифма должен быть > 0");

            return to switch
            {
                "log10" => Math.Log10(linear),
                "ln" => Math.Log(linear),
                "log2" => Math.Log(linear, 2),
                _ => throw new Exception("Неизвестная единица")
            };
        }
    }
}