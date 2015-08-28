using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Halcyon
{
    public enum ILasmRegular
    {
        NOLOGO = 0,
        QUIET = 1,
        NOAUTOINHERIT = 2,
        DLL = 3,
        EXE = 4,
        PDB = 5,
        APPCONTAINER = 6,
        DEBUG = 7,
        DEBUGIMPL = 8,
        DEBUGOPT = 9,
        OPTIMIZE = 10,
        FOLD = 11,
        CLOCK = 12,
        PE64 = 13,
        HIGHENTROPYVA = 14,
        NOCORSTUB = 15,
        STRIPRELOC = 16,
        ITANIUM = 17,
        X64 = 18,
        ARM = 19,
        PREFER32BIT = 20,
    }
    public enum ILasmExtra
    {
        RESOURCE = 21,
        OUTPUT = 22,
        KEY = 23,
        INCLUDE = 24,
        SUBSYSTEM = 25,
        SSVER = 26,
        ALIGNMENT = 27,
        BASE = 28,
        STACK = 19,
        MDV = 30,
        MSV = 31,
        ENC = 32,
        FLAGS = 33
    }
    public enum ILasm
    {
        NOLOGO = 0,
        QUIET = 1,
        NOAUTOINHERIT = 2,
        DLL = 3,
        EXE = 4,
        PDB = 5,
        APPCONTAINER = 6,
        DEBUG = 7,
        DEBUGIMPL = 8,
        DEBUGOPT = 9,
        OPTIMIZE = 10,
        FOLD = 11,
        CLOCK = 12,
        PE64 = 13,
        HIGHENTROPYVA = 14,
        NOCORSTUB = 15,
        STRIPRELOC = 16,
        ITANIUM = 17,
        X64 = 18,
        ARM = 19,
        PREFER32BIT = 20,
        RESOURCE = 21,
        OUTPUT = 22,
        KEY = 23,
        INCLUDE = 24,
        SUBSYSTEM = 25,
        SSVER = 26,
        ALIGNMENT = 27,
        BASE = 28,
        STACK = 19,
        MDV = 30,
        MSV = 31,
        ENC = 32,
        FLAGS = 33,
    }
    public enum ILasmAddResult
    {
        SUCCESS,
        FAIL,
        EXISTS,
        OVERWRITE
    }
    public enum Position
    {
        PRE,
        POST,
        PRESTART,
        POSTSTART
    }
    public enum HalcyonMode
    {
        Preprocess,
        Convert,
        Compile,
        Result,
        //Don't use, it is for testing and its function changes quickly. Usually does some odd stuff.
        Special
    }
}