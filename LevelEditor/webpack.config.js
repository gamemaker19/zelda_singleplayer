const path = require('path');
const webpack = require('webpack');

const ROOT = path.resolve( __dirname, 'code' );
const DESTINATION = path.resolve( __dirname, 'dist' );

module.exports = {
    context: ROOT,

    entry: {
        'main': ['./spriteEditor.ts', './levelEditor.ts', './imageEditor.ts', './lttpSpriteCreator.ts']
    },
    
    output: {
        filename: '[name].bundle.js',
        path: DESTINATION
    },

    resolve: {
        extensions: ['.ts', '.js'],
        modules: [
            ROOT,
            'node_modules'
        ],
        alias: {
          vue: 'vue/dist/vue.js'
        }
    },

    module: {
        rules: [
            /****************
            * PRE-LOADERS
            *****************/
            {
                enforce: 'pre',
                test: /\.js$/,
                use: 'source-map-loader'
            },
            /*{
                enforce: 'pre',
                test: /\.ts$/,
                exclude: /node_modules/,
                use: 'tslint-loader'
            },
            */
            /****************
            * LOADERS
            *****************/
            {
                test: /\.ts$/,
                exclude: [ /node_modules/ ],
                use: 'awesome-typescript-loader'
            }
        ]
    },

    devtool: 'cheap-module-source-map',
    devServer: {
      contentBase: path.join(__dirname, 'dist'),
      port: 9000,
      inline: true,
      progress: true,
      profile: true,
      colors: true,
      watch: true,
      mode: "development"
      //--port 9000 --inline --progress --profile --colors --watch --mode development

    }
};