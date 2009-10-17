Bracket
=============
Run Rack-based web applications on IIS, or via an embedded / alternative .NET web server,
based on the [IronRuby.Rack sample](http://github.com/ironruby/ironruby/tree/master/Merlin/Main/Hosts/IronRuby.Rack/), which is part of the [IronRuby project](http://github.com/ironruby/ironruby).
In addition to the IIS support copied from the IronRuby.Rack sample,
Bracket currently supports hosting Ruby Rack applications using [System.Web.HttpListener](http://www.paraesthesia.com/archive/2008/07/16/simplest-embedded-web-server-ever-with-httplistener.aspx), [C# Web Server](http://www.codeplex.com/webserver)
or via the (still emerging) [Kayak web server](http://runkayak.com/). If you have some other form of HTTP input
you would like to use to feed Rack applications (Perhaps the .NET Service Bus), it is trivial to create a new adapter using this library.

Setup
-----
The Bracket samples are configured to rely on a copy of the IronRuby standard library and required gems stored in a zip 
file bundled with each sample. If you want to use your own copy of the IronRuby standard libraries, simply
change the App.config (embedded sample) or Web.config (IIS sample) so that the library paths point to
your own install location. However, you do not need IronRuby installed on your machine to run the samples.

Bracket assumes that at a minimum Rack is installed. To run the Sinatra or Rails samples, you'll need to make sure
those gems are installed as well.

1. Install Rack
> igem install rack

2. Install other gems if desired
> igem install sinatra
> igem install rails

3. Install IIS (if you want to run the IIS samples)
	-Instructions on setting up IIS to run the sample are the same for Bracket as for the original sample,
	 and can be found [here](http://github.com/ironruby/ironruby/tree/master/Merlin/Main/Hosts/IronRuby.Rack/).

4. Open Bracket.sln in Visual Studio
   - Click "OK" to prompts about creating virtual directories, otherwise not 
     all the project files will load.

Building
--------
Simply build the solution in Visual Studio.

Note: make sure your running Visual Studio as an administrator, and you have
IIS installed. Otherwise, the example app "Bracket.Hosting.Samples.IIS" won't work. However,
you can still run the embedded web server samples with or without IIS.

Running
-------
Unless you want to break in the debugger to check out how some specific execution path
is working, it is recommended that you play with the samples without the debugger attached 
as they will run much, much faster (IronRuby/DLR is very prolific with its tracing statements, 
and updating the output window in IIS slows down execution). To run without debugging,
click "Debug" -> "Start without Debugging" to either the embedded or IIS samples.


