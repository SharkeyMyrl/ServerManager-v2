using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server_Manager_v2._3
{
    public partial class PopUp : Form
    {
        String name;
        Boolean steam;
        int id;

        public PopUp()
        {
            InitializeComponent();
        }

        private void SaveData(object sender, EventArgs e)
        {
            name = textBox1.Text.ToString();
            steam = checkBox.Checked;
            id = Convert.ToInt32(textBox2.Text.ToString());
        }
    }
}
