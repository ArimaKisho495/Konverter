using System;
using System.Windows.Forms;
// Это у нас инициализация программы.
namespace Konverter
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Menu());
        }
    }
}

