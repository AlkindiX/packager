using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packager.VersioControl
{
    internal class Version
    {
        public int MajorVersion { get; set; }
        public int MinorVersion { get; set; }
        public int MajorBuild { get; set; }
        public int MinorBuild { get; set; }
        public bool Beta { get; set; }
        public bool Alpha { get; set; }

        // Convert v*.*.*.*[-[beta|Alfa]] into *.*.*.* format
        /*
         * v        =>          0
         * v*       =>          1
         * v*.      =>          2
         * v*.      =>          3
         * v*.*     =>          4
         * v*.*.    =>          5
         * v*.*.*   =>          6
         * v*.*.*.  =>          7
         * v*.*.*.* =>          8
         * v*.*.*.*-    =>      9
         * v*.*.*.*-beta    =>  10-13
         * v*.*.*.*-alpha   =>  10-14
         */
    }
}