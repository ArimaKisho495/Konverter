using System;
using System.Collections.Generic;
// функции тригонометрии
namespace Konverter
{
    public class Trigonometria : IKonvertatorModule
    {
        public string Category => "Тригонометрия";

        public IEnumerable<UnitDef> Units => new[]
        {
            new UnitDef("sin", "sin(x)", "sin"),
            new UnitDef("cos", "cos(x)", "cos"),
            new UnitDef("tan", "tan(x)", "tan")
        };

        public double Convert(string from, string to, double value)
        {
            double angle = from switch
            {
                "sin" => Math.Asin(Math.Clamp(value, -1, 1)),
                "cos" => Math.Acos(Math.Clamp(value, -1, 1)),
                "tan" => Math.Atan(value),
                _ => throw new Exception("Неизвестная единица")
            };

            return to switch
            {
                "sin" => Math.Sin(angle),
                "cos" => Math.Cos(angle),
                "tan" => Math.Tan(angle),
                _ => throw new Exception("Неизвестная единица")
            };
        }
    }
}