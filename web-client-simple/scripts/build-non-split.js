const rewire = require('rewire')
 const defaults = rewire('react-scripts/scripts/build.js') // If you ejected, use this instead: const defaults = rewire('./build.js')
 let config = defaults.__get__('config')

 config.optimization.splitChunks = {
     cacheGroups: {
         default: false
     }
 }

 config.optimization.runtimeChunk = false;

 // Renames main.00000000.js to botframework.webcontrol.min.js
 config.output.filename = 'static/botcontrol.min.js'

 // Renames main.00000000.css to botframework.webcontrol.min.css
 config.plugins[5].options.filename = 'static/botcontrol.min.css'
 config.plugins[5].options.moduleFilename = () => 'static/botcontrol.min.css'