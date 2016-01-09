//
//  CommandRun.cs
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
    /// Command run.
    /// </summary>
    public class CommandRun
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [PrimaryKey(), AutoIncrement()]
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        public int uniqueId { get; set; }
        /// <summary>
        /// Gets or sets the command.
        /// </summary>
        /// <value>The command.</value>
        public string Command { get; set; }
        /// <summary>
        /// Gets or sets the execute event.
        /// </summary>
        /// <value>The execute event.</value>
        public ExecuteEvent ExecuteEvent { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.Tables.CommandRun"/> class.
        /// </summary>
        /// <param name="commandstring">Commandstring.</param>
        /// <param name="excecuteevent">Excecuteevent.</param>
        /// <param name="uniqueid">Uniqueid.</param>
        public CommandRun(string commandstring, ExecuteEvent excecuteevent, int uniqueid)
        {
            Command = commandstring;
            ExecuteEvent = excecuteevent;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.Tables.CommandRun"/> class.
        /// </summary>
        public CommandRun()
        {
        }
    }

}
