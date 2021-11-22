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
    public partial class AskAngForm : Form
    {
        public AskAngForm()
        {
            InitializeComponent();
        }
        public string ReturnAnng
        {
            get { return txbAngl.Text; }
        }
    }
}
