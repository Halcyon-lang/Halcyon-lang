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
    public enum AssemblyLinker
    {
        HELP = 0,
        ALGID = 1,
        BASE = 2,
        BUGREPORT = 3,
        COMP = 4,
        CONFIG = 5,
        COPY = 6,
        C = 7,
        DELAY = 8,
        DESCR = 9,
        E = 10,
        FILEVERSION = 11,
        FLAGS = 12,
        FULLPATHS = 13,
        KEYF = 14,
        KEYN = 15,
        MAIN = 16,
        NOLOGO = 17,
        OUT = 18,
        PLATFORM = 19,
        PROD = 20,
        PRODUCTV = 21,
        SUBSYSTEMVERSION = 22,
        T = 23,
        TEMPLATE = 24,
        TITLE = 25,
        TRADE = 26,
        V = 27,
        WIN32ICON = 28,
        WIN32RES = 29,
        EMBED = 30,
        LINK = 31
    }
    public enum AssemblyLinkerRegular
    {
        HELP = 0,
        FULLPATHS = 13,
        NOLOGO = 17
    }
    public enum AssemblyLinkerExtra
    {
        ALGID = 1,
        BASE = 2,
        BUGREPORT = 3,
        COMP = 4,
        CONFIG = 5,
        COPY = 6,
        C = 7,
        DELAY = 8,
        DESCR = 9,
        E = 10,
        FILEVERSION = 11,
        FLAGS = 12,
        KEYF = 14,
        KEYN = 15,
        MAIN = 16,
        OUT = 18,
        PLATFORM = 19,
        PROD = 20,
        PRODUCTV = 21,
        SUBSYSTEMVERSION = 22,
        T = 23,
        TEMPLATE = 24,
        TITLE = 25,
        TRADE = 26,
        V = 27,
        WIN32ICON = 28,
        WIN32RES = 29,
        EMBED = 30,
        LINK = 31
    }
    public enum AssemblyLinkerAddResult
    {
        SUCCESS,
        FAIL,
        EXISTS,
        OVERWRITE
    }
    public enum ILdasm
    {
        OUT = 0
    }
    public enum ILdasmRegular
    {

    }
    public enum ILdasmExtra
    {
        OUT = 0
    }

    public enum ILdasmAddResult
    {
        SUCCESS,
        FAIL,
        EXISTS,
        OVERWRITE
    }
    public enum SpeechMode
    {
        Silent,
        Normal
    }
}