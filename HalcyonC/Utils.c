#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#define Dll   __declspec( dllexport )
//using std;

Dll void printHelp() 
{
	cout << "Halcyon compiler help:\n";
	cout << "   -compile [File] - Compiles file to executable\n";
	cout << "   -convert [File] - Converts file to IL. File's will be written down here\n";
	cout << "   -result [Text] - how does given line look in IL\n";
	return;
}

Dll void printInfoHelp() 
{
	cout << "-info\n";
	cout << "   classes - Prints all classes\n");
	cout << "   elements - Prints currently loaded elements\n";
	cout << "   version - Prints current version of SlashButter\n";
	return;
}


