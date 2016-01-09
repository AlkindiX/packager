//
//  NewExcludeFileDialog.cs
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
using System.Windows.Forms;

namespace Packager.UI
{
    public partial class NewExcludeFileDialog : Form
    {
        public NewExcludeFileDialog()
        {
            InitializeComponent();
        }

        private void NewExcludeFileDialog_Load(object sender, EventArgs e)
        {
            textBox1.MaxLength = Int32.MaxValue - 1;
        }

        public List<string> ExcludedFileNames { get; set; }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please enter some excluded files to be registered");
                return;
            }
            ExcludedFileNames = new List<string>();
            ExcludedFileNames.AddRange(textBox1.Lines);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}