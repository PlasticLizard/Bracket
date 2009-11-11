 # shrimp.rb
    class Shrimp
      SHRIMP_STRING = %q{
              |///           
     .*----___//     <-- it was supposed to be a walking shrimp...
    <----/|/|/|
    }

      def initialize(app)
        @app = app
      end

      def call(env)
        puts SHRIMP_STRING
        status, headers, response = @app.call(env)

        response_body = ""
        response.each { |part| response_body += part }
        response_body += "<pre>#{SHRIMP_STRING}</pre>"

        headers["Content-Length"] = response_body.length.to_s

        [status, headers, response_body]
      end
    end
