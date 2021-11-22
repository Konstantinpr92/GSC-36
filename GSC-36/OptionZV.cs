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
    public partial class OptionZV : Form
    {
        public OptionZV()
        {
            InitializeComponent();
        }
        public string ReturnColorStr
        {
            get { return cboColor.Text; }
        }

        public string ReturnN
        {
            get { return tbxN.Text; }
        }

        public string ReturnR
        {
            get { return txbR.Text; }
        }
    }
}
