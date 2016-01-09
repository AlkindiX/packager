//
//  License.cs
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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace Packager.UI
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }
    }
}