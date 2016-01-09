//
//  Logging.cs
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
    /// Configuration.
    /// </summary>
    [Table("Configuration")]
    public class Configuration
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey(), AutoIncrement()]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the name of the configuration.
        /// </summary>
        /// <value>The name of the configuration.</value>
        [Column("ConfigurationName")]
        public string  ConfigurationName { get; set;}
        /// <summary>
        /// Gets or sets the configuration data.
        /// </summary>
        /// <value>The configuration data.</value>
        [Column("ConfigurationData")]
        public string ConfigurationData { get; set;}
        /// <summary>
        /// Gets or sets the configuration comment.
        /// </summary>
        /// <value>The configuration comment.</value>
        [Column("ConfigurationComment")]
        public string ConfigurationComment { get; set;}
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.Tables.Configuration"/> class.
        /// </summary>
        public Configuration()
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.Tables.Configuration"/> class.
        /// </summary>
        /// <param name="ConfigurationName">Configuration name.</param>
        /// <param name="ConfigurationData">Configuration data.</param>
        /// <param name="ConfigurationComment">Configuration comment.</param>
        public Configuration(string ConfigurationName, string ConfigurationData, string ConfigurationComment)
        {
            this.ConfigurationName = ConfigurationName;
            this.ConfigurationData = ConfigurationData;
            this.ConfigurationComment = ConfigurationComment;
        }
    }
}

