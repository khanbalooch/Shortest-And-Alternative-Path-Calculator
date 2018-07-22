using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Algo_Spring2016_TermProject_i13_1821_i13_1822;
namespace Algo_Spring2016_TermProject_i13_1821_i13_1822
{
    static class Program
    {
        
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PakistanMap());
        }
    }
}