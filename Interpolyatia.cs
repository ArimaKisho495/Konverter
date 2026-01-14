using System;
using System.Collections.Generic;
// функции интерполяции
namespace Konverter
{
    public class Interpolyatia : IKonvertatorModule
    {
        public string Category => "Интерполяция";

        public IEnumerable<UnitDef> Units => new[]
        {
            new UnitDef("0-1",   "0 → 1",   "0→1"),
            new UnitDef("0-100", "0 → 100", "0→100"),
            new UnitDef("0-255", "0 → 255", "0→255")
        };

        public double Convert(string from, string to, double value)
        {
            double fromMax = from switch
            {
                "0-1" => 1,
                "0-100" => 100,
                "0-255" => 255,
                _ => throw new Exception("Неизвестный диапазон")
            };

            double toMax = to switch
            {
                "0-1" => 1,
                "0-100" => 100,
                "0-255" => 255,
                _ => throw new Exception("Неизвестный диапазон")
            };

            if (value < 0 || value > fromMax)
                throw new Exception("Значение выходит за пределы диапазона.");

            double norm = value / fromMax;
            return norm * toMax;
        }
    }
}