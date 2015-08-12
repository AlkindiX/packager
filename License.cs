using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Packager
{
    public partial class License : Form
    {
        public License()
        {
            InitializeComponent();
        }

        private void Lisence_Load(object sender, EventArgs e)
        {
            FileInfo lic = new FileInfo("LICENSE");
            if (!lic.Exists)
            {
                richTextBox1.Text = "License not found, you can view it at http://www.gnu.org/licenses/gpl-3.0.en.html";
            }
            else
            {
                StreamReader strmreader = new StreamReader(lic.OpenRead());
                richTextBox1.Text = strmreader.ReadToEnd();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start("url:" + e.LinkText);
        }
    }
}