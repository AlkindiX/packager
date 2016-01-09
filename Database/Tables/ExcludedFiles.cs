//
//  ExcludedFiles.cs
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
namespace Packager.Database.Tables
{
    /// <summary>
    /// Excluded files table class
    /// </summary>
    public class ExcludedFiles
    {
        /// <summary>
        /// Gets or sets the identifier of the row.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey(), AutoIncrement()]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the filename.
        /// </summary>
        /// <value>The filename.</value>
        [Column("Filename")]
        public string Filename { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.Tables.ExcludedFiles"/> class.
        /// </summary>
        /// <param name="file">File.</param>
        public ExcludedFiles(string file)
        {
            Filename = file;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.Tables.ExcludedFiles"/> class. This initation for <see cref="SQLite" />
        /// </summary>
        public ExcludedFiles()
        {
        }
    }

}
