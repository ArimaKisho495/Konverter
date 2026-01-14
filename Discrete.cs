using System;
using System.Collections.Generic;

namespace Konverter
{
    public class Discrete : IKonvertatorModule
    {
        public string Category => "Дискретные нелинейные операции";

        public IEnumerable<UnitDef> Units => new[]
        {
            new UnitDef("abs",   "Модуль числа |x|",     "abs"),
            new UnitDef("floor", "Округление вниз",       "floor"),
            new UnitDef("ceil",  "Округление вверх",      "ceil"),
            new UnitDef("round", "Округление к ближайшему","round"),
            new UnitDef("sign",  "Знак числа (-1,0,1)",    "sign"),
            new UnitDef("clamp", "Ограничение [-1;1]",     "clamp")
        };

        public double Convert(string from, string to, double value)
        {
            // Операции дискретны и нелинейны – но НЕ обратимы.
            // Поэтому мы просто применяем TO-функцию к исходному значению.
            return to switch
            {
                "abs" => Math.Abs(value),
                "floor" => Math.Floor(value),
                "ceil" => Math.Ceiling(value),
                "round" => Math.Round(value),
                "sign" => Math.Sign(value),
                "clamp" => Math.Clamp(value, -1, 1),
                _ => throw new Exception("Неизвестная дискретная операция")
            };
        }
    }
}
