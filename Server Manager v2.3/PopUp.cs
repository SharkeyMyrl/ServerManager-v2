using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerManager
{
    public partial class PopUp : Form
    {
        public String name;
        public Boolean steam;
        public int id;

        public PopUp()
        {
            InitializeComponent();
        }

        private void SaveData(object sender, EventArgs e)
        {
            name = textBox1.Text.ToString();
            steam = checkBox.Checked;
            if (checkBox.Checked)
            {
                try
                {
                    id = Convert.ToInt32(textBox2.Text.ToString());
                }
                catch (Exception except)
                {
                    MessageBox.Show("Invalid Steam ID", "Steam Interface", MessageBoxButtons.OK);
                }            
            }
            Close();
        }
    }
}
