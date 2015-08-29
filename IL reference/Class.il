//This example shows a class with fields
.class public auto ansi beforefieldinit Halcyon.Class
       extends [mscorlib]System.Object
{
  //This is the constructor of this class. It is executed whenever you make new instance of this class
  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    // 
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ret
  } 

  //This is the *static* constructor of this class. It is executed only once, and that is when the class is first mentioned, regardless of where.
  //It is commonly used to init static fields (implicit in C# and VB) and do things that need to be done just once
  //Please note that you can't call a static constructor, and that it can not have any paramaters
  .method private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  {
    // 
    .maxstack  8
    IL_0000:  ldc.i4.0
    IL_0001:  stsfld     int32 Halcyon.ApiVersion::Field
    IL_0006:  ldc.i4.5
    IL_0007:  stsfld     int32 Halcyon.ApiVersion::AnotherField
    IL_003c:  ret
  } 

} 