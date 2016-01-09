//
//  ConsoleColor.cs
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
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace Packager.Tools
{
    /// <summary>
    /// Console color.
    /// </summary>
    public sealed class ConsoleColor
    {
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="">.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg0">Arg0.</param>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        /// <para> name="__arglist">Arg lists</para>
        public static void WriteLine (System.ConsoleColor color, string format, object arg0, object arg1, object arg2, object arg3, __arglist)
        {
            ArgIterator argIterator = new ArgIterator (__arglist);
            int remainingCount = argIterator.GetRemainingCount ();
            object[] array = new object[remainingCount + 4];
            array [0] = arg0;
            array [1] = arg1;
            array [2] = arg2;
            array [3] = arg3;
            for (int i = 0; i < remainingCount; i++) {
                TypedReference nextArg = argIterator.GetNextArg ();
                array [i + 4] = TypedReference.ToObject (nextArg);
            }
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (string.Format (format, array));
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg0">Arg0.</param>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        public static void WriteLine (System.ConsoleColor color, string format, object arg0, object arg1, object arg2)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (format, arg0, arg1, arg2);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg0">Arg0.</param>
        /// <param name="arg1">Arg1.</param>
        public static void WriteLine (System.ConsoleColor color, string format, object arg0, object arg1)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (format, arg0, arg1);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, int value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, double value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, decimal value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="buffer">Buffer.</param>
        public static void WriteLine (System.ConsoleColor color, char[] buffer)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (buffer);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, char value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void WriteLine (System.ConsoleColor color, bool value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        public static void WriteLine (System.ConsoleColor color)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine ();
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, long value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="buffer">Buffer.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        public static void WriteLine (System.ConsoleColor color, char[] buffer, int index, int count)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (buffer, index, count);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg">Argument.</param>
        public static void WriteLine (System.ConsoleColor color, string format, params object[] arg)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (arg == null) {
                Console.WriteLine (format);
            } else {
                Console.WriteLine (format, arg);
            }
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg0">Arg0.</param>
        public static void WriteLine (System.ConsoleColor color, string format, object arg0)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (format, arg0);
            Console.ForegroundColor = old_font;
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, ulong value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, uint value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, string value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, float value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void WriteLine (System.ConsoleColor color, object value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color, format, arg0, arg1, arg2, arg3 and .
        /// </summary>
        /// <param name="">.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg0">Arg0.</param>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        /// <param name="arg3">Arg3.</param>
        public static void Write (System.ConsoleColor color, string format, object arg0, object arg1, object arg2, object arg3, __arglist
        )
        {
            ArgIterator argIterator = new ArgIterator (__arglist);
            int remainingCount = argIterator.GetRemainingCount ();
            object[] array = new object[remainingCount + 4];
            array [0] = arg0;
            array [1] = arg1;
            array [2] = arg2;
            array [3] = arg3;
            for (int i = 0; i < remainingCount; i++) {
                TypedReference nextArg = argIterator.GetNextArg ();
                array [i + 4] = TypedReference.ToObject (nextArg);
            }
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (string.Format (format, array));
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color, format, arg0, arg1 and arg2.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg0">Arg0.</param>
        /// <param name="arg1">Arg1.</param>
        /// <param name="arg2">Arg2.</param>
        public static void Write (System.ConsoleColor color, string format, object arg0, object arg1, object arg2)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (format, arg0, arg1, arg2);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color, format, arg0 and arg1.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg0">Arg0.</param>
        /// <param name="arg1">Arg1.</param>
        public static void Write (System.ConsoleColor color, string format, object arg0, object arg1)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (format, arg0, arg1);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, long value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, object value){
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, float value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, string value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }

        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, uint value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }

        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, ulong value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color, format and arg0.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg0">Arg0.</param>
        public static void Write (System.ConsoleColor color, string format, object arg0)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (format, arg0);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, int value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color, buffer, index and count.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="buffer">Buffer.</param>
        /// <param name="index">Index.</param>
        /// <param name="count">Count.</param>
        public static void Write (System.ConsoleColor color, char[] buffer, int index, int count)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (buffer, index, count);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color, format and arg.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="format">Format.</param>
        /// <param name="arg">Argument.</param>
        public static void Write (System.ConsoleColor color, string format, params object[] arg)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            if (arg == null) {
                Console.Write (format);
            } else {
                Console.Write (format, arg);
            }
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public static void Write (System.ConsoleColor color, bool value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, char value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and buffer.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="buffer">Buffer.</param>
        public static void Write (System.ConsoleColor color, char[] buffer)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (buffer);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, decimal value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
        /// <summary>
        /// Write the specified color and value.
        /// </summary>
        /// <param name="color">Color.</param>
        /// <param name="value">Value.</param>
        public static void Write (System.ConsoleColor color, double value)
        {
            var old_font = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write (value);
            Console.ForegroundColor = old_font;
        }
    }
}


