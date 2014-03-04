Jasmine setup with Istanbul code coverage
=============

Since I run through these steps on nearly every project here's a base line setup to build on. It uses Karma, Jasmine
and the global name space checker spec

Install [karma](http://karma-runner.github.com/0.8/index.html) (formerly testacular): npm install karma

If you are on windows (like I am these days) run the above command from the Visual Studio command line, otherwise PhantomJS might not build

If you want to plug this into your WebStorm IDE:

 1. Go to "Edit configurations"
 2. Add a new configuration for Node.js
 3. Give it name e.g.: "Karma"
 4. Specify the fully qualified path to your Node executable: "C:\Program Files\nodejs\node.exe"
 5. Full path to your working directory
 6. Path to your kharma installation, e.g.: C:\Users\username\AppData\Roaming\npm\node_modules\karma\bin\karma
 7. Application start parameters: start .\karma.conf.js

Then again maybe I should just be using grunt.js
