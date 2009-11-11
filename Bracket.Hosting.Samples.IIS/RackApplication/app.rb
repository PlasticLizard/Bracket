require 'erb'

class App 
  def call env
    request  = Rack::Request.new env
    response = Rack::Response.new

    response.header['Content-Type'] = 'text/html'

       
    response.write "<html><body>"

    @msg1 = "Hello"
    @msg2 = "World"
    msg = ERB.new('IronRuby running Rack says "<%= @msg1 %>, <b><%= @msg2 %></b>" at <%= Time.now %>').result(binding)

    response.write msg + "<form action='tryme' method='post'><input type='submit' name='howdy' value='ha'/></form>"
    
    if request.post?
		#response.write request.post + "<BR>"
		#response.write(env["rack.input"].read)
		response.write response["howdy"]
    end
    
    response.write "</body></html>"

    response.finish
  end
end
