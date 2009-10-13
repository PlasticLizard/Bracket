require 'rubygems'
require 'sinatra'

require 'app'

class NilClass
	def empty?
		true
	end
end

run Sinatra::Application
