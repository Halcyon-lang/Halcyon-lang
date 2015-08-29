//This is a simple generic class of Halcyon
//Class parameters need to be provided with ! before their name, possibly to differentiate them from normal types.
.class public auto ansi beforefieldinit Halcyon.ValuePair`2<Left,Right>
       extends [mscorlib]System.Object
{
  .field private !Left leftValue
  .field private !Right rightValue
  .method public hidebysig specialname instance !Left 
          get_LeftValue() cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      !0 class Halcyon.ValuePair`2<!Left,!Right>::leftValue
    IL_0006:  ret
  } 

  .method public hidebysig specialname instance void 
          set_LeftValue(!Left 'value') cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      !0 class Halcyon.ValuePair`2<!Left,!Right>::leftValue
    IL_0007:  ret
  } 

  .method public hidebysig specialname instance !Right 
          get_RightValue() cil managed
  {
    // 
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldfld      !1 class Halcyon.ValuePair`2<!Left,!Right>::rightValue
    IL_0006:  ret
  } 

  .method public hidebysig specialname instance void 
          set_RightValue(!Right 'value') cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  ldarg.1
    IL_0002:  stfld      !1 class Halcyon.ValuePair`2<!Left,!Right>::rightValue
    IL_0007:  ret
  } /

  .method public hidebysig specialname rtspecialname 
          instance void  .ctor(!Left l,
                               !Right r) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.0
    IL_0001:  call       instance void [mscorlib]System.Object::.ctor()
    IL_0006:  ldarg.0
    IL_0007:  ldarg.1
    IL_0008:  call       instance void class Halcyon.ValuePair`2<!Left,!Right>::SetLeft(!0)
    IL_000d:  ldarg.0
    IL_000e:  ldarg.2
    IL_000f:  call       instance void class Halcyon.ValuePair`2<!Left,!Right>::SetRight(!1)
    IL_0014:  ret
  } 

  .method public hidebysig instance void 
          SetRight(!Right r) cil managed
  {
    .maxstack  8
    IL_0000:  ldarg.1
    IL_0001:  box        !Right
    IL_0006:  brfalse.s  IL_0010

    IL_0008:  ldarg.0
    IL_0009:  ldarg.1
    IL_000a:  stfld      !1 class Halcyon.ValuePair`2<!Left,!Right>::rightValue
    IL_000f:  ret

    IL_0010:  newobj     instance void [mscorlib]System.NullReferenceException::.ctor()
    IL_0015:  throw
  } 

  .method public hidebysig instance void 
          SetLeft(!Left l) cil managed
  {
    // 
    .maxstack  8
    IL_0000:  ldarg.1
    IL_0001:  box        !Left
    IL_0006:  brfalse.s  IL_0010

    IL_0008:  ldarg.0
    IL_0009:  ldarg.1
    IL_000a:  stfld      !0 class Halcyon.ValuePair`2<!Left,!Right>::leftValue
    IL_000f:  ret

    IL_0010:  newobj     instance void [mscorlib]System.NullReferenceException::.ctor()
    IL_0015:  throw
  } 

  .property instance !Left LeftValue()
  {
    .get instance !Left Halcyon.ValuePair`2::get_LeftValue()
    .set instance void Halcyon.ValuePair`2::set_LeftValue(!Left)
  } 
  .property instance !Right RightValue()
  {
    .get instance !Right Halcyon.ValuePair`2::get_RightValue()
    .set instance void Halcyon.ValuePair`2::set_RightValue(!Right)
  } 
} 