//This example shows a class with fields
.class public auto ansi beforefieldinit Halcyon.FieldExample
       extends [mscorlib]System.Object
{
  .field public static int32 Field
  .field public int32 AnotherField
  //Even though this class does not use constructor, this hidden constructor calls constructor of System.Object
  .method public hidebysig specialname rtspecialname 
          instance void  .ctor() cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
	IL_0000:  ldc.i4.0
    IL_0001:  stsfld     int32 Halcyon.FieldExample::Field
    IL_0006:  ret
  } 

  //Notice this. Static fields are actually assigned in the static constructor, which is here always as well. Not in enums tho
  .method private hidebysig specialname rtspecialname static 
          void  .cctor() cil managed
  {
    .maxstack  8
    IL_0000:  ldc.i4.0
    IL_0001:  stsfld     int32 Halcyon.FieldExample::Field
    IL_003c:  ret
  } 

} 
