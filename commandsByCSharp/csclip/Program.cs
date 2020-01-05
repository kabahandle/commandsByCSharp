using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace csclip
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();
            string line = string.Empty;

            while ((line = Console.In.ReadLine()) != null)
            {
                if (sb != null)
                {
                    sb.Append(line + "\n");
                }
            }

            Clipboard.SetText(sb.ToString());
        }
    }
}
