//What properties do is basically only referencing a getter and/or setter which are, in fact, static full-fledged methods, only hidden from being viewed or directly accessed
//Conditions are:
//Getter must return the same type as proe
.property string ALExecutableName()
  {
    .get string Halcyon.Config::get_ALExecutableName()
    .set void Halcyon.Config::set_ALExecutableName(string)
  } 

  .method public hidebysig specialname static 
          string  get_ALExecutableName() cil managed
  {
    .maxstack  8
    IL_0000:  ldstr      "ALExecutableName"
    IL_0005:  ldstr      "al.exe"
    IL_000a:  call       object Halcyon.Config::GetValue(string,
                                                         object)
    IL_000f:  castclass  [mscorlib]System.String
    IL_0014:  ret
  } 

  .method public hidebysig specialname static 
          void  set_ALExecutableName(string 'value') cil managed
  {
    .maxstack  8
    IL_0000:  ldstr      "ALExecutableName"
    IL_0005:  ldarg.0
    IL_0006:  call       valuetype Halcyon.TryUpdateKeyResult Halcyon.Config::TryUpdateKey(string,
                                                                                           object)
    IL_000b:  pop
    IL_000c:  ret
  } 