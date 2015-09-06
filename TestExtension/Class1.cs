//Basic usings added by Visual Studio
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//We need to make use of Halcyon as well. Don't forget to reference Halcyon.exe in your project
using Halcyon;
using Halcyon.Logging;

//The namespace of our extension
namespace MyThing
{

    //ApiVersion Attribute. Indicates for which version of Halcyon is the extension intended. 
    //Halcyon only accepts extensions with the same ApiVersion.  
    //The extension will not be loaded if you forget to add the Attribute.
    [ApiVersion(0, 7)]
    //The class of our extension. Don't forget to make it public
    public class MyExtension : HalcyonExtension
    {
        //These provide information about the extension

        //The name or nickname of the author of this extension
        public override string Author { get { return "Your (nick)name"; } }
        //The name of the extension
        public override string Name { get { return "MyExtension"; } }
        //Short description of the extension
        public override string Description { get { return "A simple extension example"; } }
        //Version of the extension
        public override Version Version { get { return new Version(1, 0); } }

        //You can set some stuff in the constructor
        public MyExtension()
            : base()
        {
            Order = 1;
        }

        //The main entrypoint of the extension, start up your stuff here
        public override void Initialize()
        {
            //Let's add a basic listener for the OnStart event of Halcyon
            Program.OnStart += SaySomeShit;
        }

        //And here comes our listener
        public void SaySomeShit(object sender, EventArgs e)
        {
            Logger.Log("Yay it works!");
        }

        //Don't forget to properly dispose of your plugin once its playtime is over
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}