using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Bracket.Tests
{
    [TestFixture]
    public class RubyEnvironmentBehavior
    {
        [Test]
        public void ShouldRunScriptRequiringRubyLibraryWhenLibrariesInArchiveWithZipExtension()
        {
            //Given
            var environment = new RubyEnvironment();
            string RUBY_SCRIPT = @"
default_load_path = ['.', 'IronRuby.zip/IronRuby/lib/IronRuby/gems/1.8', 'IronRuby.zip/IronRuby/lib/IronRuby', 'IronRuby.zip/ironruby/lib/ruby/site_ruby/1.8', 'IronRuby.zip/IronRuby/lib/ruby/1.8']
default_load_path.each {|path| $LOAD_PATH << path if !$LOAD_PATH.include?(path)}

require 'ostruct'

o = OpenStruct.new
o.MyTest = 'hello world'

return o.MyTest
            ";

            //When
            var result = environment.Engine.Execute(RUBY_SCRIPT).ToString();

            //Then
            Assert.AreEqual("hello world", result);
        }

        [Test]
        public void ShouldRunScriptRequiringRubyLibraryWhenLibrariesInArchiveWithDlzExtension()
        {
            //Given
            var environment = new RubyEnvironment();
            string RUBY_SCRIPT = @"
default_load_path = ['.', 'IronRuby.dlz/IronRuby/lib/IronRuby/gems/1.8', 'IronRuby.dlz/IronRuby/lib/IronRuby', 'IronRuby.dlz/ironruby/lib/ruby/site_ruby/1.8', 'IronRuby.dlz/IronRuby/lib/ruby/1.8']
default_load_path.each {|path| $LOAD_PATH << path if !$LOAD_PATH.include?(path)}

require 'ostruct'

o = OpenStruct.new
o.MyTest = 'hello world'

return o.MyTest
            ";

            if (!File.Exists("IronRuby.dlz"))
                File.Copy("IronRuby.zip", "IronRuby.dlz");

            //When
            var result = environment.Engine.Execute(RUBY_SCRIPT).ToString();

            //Then
            Assert.AreEqual("hello world", result);
        }
    }
}
