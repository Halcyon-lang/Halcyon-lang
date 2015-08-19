#include "dllheader.h"

Dll void printHelp() 
{
	printf("SlashButter compiler help:\n");
	printf("   -compile [File] - Compiles file to executable\n");
	printf("   -convert [File] - Converts file to IL. File's will be written down here\n");
	printf("   -result [Text] - how does given line look in IL\n");
	return;
}

Dll void printInfoHelp() 
{
	printf("-info\n");
	printf("   classes - Prints all classes\n");
	printf("   elements - Prints currently loaded elements\n");
	printf("   version - Prints current version of SlashButter\n");
	return;
}


