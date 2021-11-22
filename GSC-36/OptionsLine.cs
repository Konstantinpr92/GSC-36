using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GSC_36
{
    public partial class OptionsLine : Form
    {
        public OptionsLine()
        {
            InitializeComponent();
        }

        public string ReturnColorStr
        { 
           get { return cboColor.Text; } 
        }

        public string ReturnWStr
        {
            get { return txbW.Text; }
        }



    }
}
