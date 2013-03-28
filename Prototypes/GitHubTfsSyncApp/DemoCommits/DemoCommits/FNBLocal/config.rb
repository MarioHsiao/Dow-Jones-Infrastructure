#Markdown
#set :markdown_engine, :redcarpet

#Livereload
activate :livereload

#Sprockets
activate :sprockets

###
# Compass
###

# Susy grids in Compass
# First: gem install compass-susy-plugin
# require 'susy'

# Bootstrap
# First: 
#    gem install sass-rails
#    gem install bootstrap-sass
require 'bootstrap-sass'

# Change Compass configuration
compass_config do |config|
  config.output_style = :expanded
  config.line_comments = true
  config.http_path = "/"
  config.images_dir = "images"
  config.javascripts_dir = "javascripts"
  config.sass_dir = "sass"
end

###
# Page options, layouts, aliases and proxies
###

# Per-page layout changes:
#
# With no layout
# page "/path/to/file.html", :layout => false
#
# With alternative layout
# page "/path/to/file.html", :layout => :otherlayout
#
# A path which all have the same layout
# with_layout :admin do
#   page "/admin/*"
# end

# Proxy (fake) files
# page "/this-page-has-no-template.html", :proxy => "/template-file.html" do
#   @which_fake_page = "Rendering a fake page with a variable"
# end

###
# Helpers
###

# Automatic image dimensions on image_tag helper
activate :automatic_image_sizes

# Methods defined in the helpers block are available in templates
# helpers do
#   def some_helper
#     "Helping"
#   end
# end

helpers do

  def load_css(stylesheets)
    @stylesheets = stylesheets
    return if @stylesheets.nil?
    sheets = @stylesheets.gsub!(/\s*/, "").split(",")
      sheets.each { |sheet|
        content_for :projectstyles do
          stylesheet_link_tag sheet
        end
      }
    return
  end

  def load_scripts(javascripts)
    @javascripts = javascripts
    return if @javascripts.nil?
    scripts = @javascripts.gsub!(/(\s*)/, "").split(",")
    scripts.each { |script|
      content_for :projectscripts do
        javascript_include_tag script
      end
    }
    return
  end

end

set :css_dir, 'stylesheets'

set :js_dir, 'javascripts'

set :images_dir, 'images'

set :js_assets_path, 'javascripts/vendor'

# Build-specific configuration
configure :build do

  # For example, change the Compass output style for deployment
  compass_config do |config|
    config.sass_options = {:debug_info => false}
    config.sass_options = {:line_comments => false}
  end

  activate :minify_css

  # Minify Javascript on build
  #activate :minify_javascript

  # Create favicon/touch icon set from source/favicon_base.png
  #activate :favicon_maker

  # Enable cache buster
  # activate :cache_buster

  # Use relative URLs
  activate :relative_assets

  # Compress PNGs after build
  # First: gem install middleman-smusher
  # require "middleman-smusher"
  # activate :smusher

  # Or use a different image path
  # set :http_path, "/Content/images/"
end