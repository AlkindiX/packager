//
//  AddNewReleaseDirDialog.cs
//
//  Author:
//       Mohammed Alkindi <mohammedalkindi2015@yahoo.com>
//
//  Copyright (c) 2016 mohammed
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Packager.UI
{

    public partial class AddNewReleaseDirDialog : Form
    {
        public string ReleaseDirectoryLocation { get; set; }
        public string ReleaseZipFilename { get; set; }
        public AddNewReleaseDirDialog()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show (this, "You must enter the release directory name", "Warning" ,MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show (this, "You must enter the zip file name", "Warning", MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (!Directory.Exists(textBox1.Text)) 
            {
                MessageBox.Show("The directory you entered is not exist on the disk", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning) ;
                return;
            }
            if (!textBox2.Text.ToLower().Contains(".zip".ToLower()))
            {
                MessageBox.Show("You MUST add .zip suffix at the end of the file", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ReleaseDirectoryLocation = textBox1.Text;
            ReleaseZipFilename = textBox2.Text;
            DialogResult = DialogResult.OK;
            Close();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var temp = new FolderBrowserDialog())
            {
                if (temp.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = temp.SelectedPath;

                }
            }
        }

        private void AddNewReleaseDirDialog_Load(object sender, EventArgs e)
        {
            textBox1.Select();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Select(0, textBox2.Text.IndexOf("."));
        }
    }
}