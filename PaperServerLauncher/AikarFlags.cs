using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaperServerLauncher
{
    public partial class AikarFlagsForm : Form
    {
        public AikarFlagsForm()
        {
            InitializeComponent();

            var source = new BindingSource();
            source.DataSource = Utils.AikarFlagData.flagsList;
            dgvFlags.DataSource = source;

        }

    }
}
