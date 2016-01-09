//
//  ReleaseDirectory.cs
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


using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packager.Database.Tables
{
    /// <summary>
    /// Relase directoy.
    /// </summary>
    [Table("RelaseDirectories")]
    public class RelaseDirectoy
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey(), AutoIncrement()]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the release directory location.
        /// </summary>
        /// <value>The release directory location.</value>
        [Column("ReleaseDirectoryLocation")]
        public string ReleaseDirectoryLocation { get; set; }
        /// <summary>
        /// Gets or sets the command run identifier.
        /// </summary>
        /// <value>The command run identifier.</value>
        public int CommandRunId { get; set; }
        /// <summary>
        /// Gets or sets the name of the release.
        /// </summary>
        /// <value>The name of the release.</value>
        public string ReleaseName { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.Tables.RelaseDirectoy"/> class.
        /// </summary>
        /// <param name="releaselocation">Releaselocation.</param>
        /// <param name="commandrunid">Commandrunid.</param>
        /// <param name="releasename">Releasename.</param>
        public RelaseDirectoy(string releaselocation, int commandrunid, string releasename)
        {
            ReleaseDirectoryLocation = releaselocation;
            CommandRunId = commandrunid;
            ReleaseName = releasename;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.Tables.RelaseDirectoy"/> class.
        /// </summary>
        public RelaseDirectoy()
        {
        }
    }

}
