using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halcyon
{
    /// <summary>
    /// Contains versions of various Halcyon components and Halcyon itself
    /// </summary>
    public class ApiVersion
    {
        /// <summary>
        /// Halcyon's major version
        /// </summary>
        public static int Major = 0;
        /// <summary>
        /// Halcyon's minor version
        /// </summary>
        public static int Minor = 3;
        /// <summary>
        /// IL Assembler (ILasm) wrapper major version
        /// </summary>
        public static int ILasmMajor = 0;
        /// <summary>
        /// IL Assembler (ILasm) wrapper minor version
        /// </summary>
        public static int ILasmMinor = 2;
        /// <summary>
        /// AssemblyLinker (AL) wrapper major version
        /// </summary>
        public static int ALMajor = 0;
        /// <summary>
        /// AssemblyLinker (AL) wrapper minor version
        /// </summary>
        public static int ALMinor = 1;
        /// <summary>
        /// WrapperSelector's major version.
        /// </summary>
        public static int WrapperMajor = 0;
        /// <summary>
        /// WrapperSelector's minor version
        /// </summary>
        public static int WrapperMinor = 1;
    }
}