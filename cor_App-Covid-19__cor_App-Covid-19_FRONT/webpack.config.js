const webpack = require('webpack');
const TerserPlugin = require('terser-webpack-plugin');
const CompressionPlugin = require('compression-webpack-plugin');
const zlib = require('zlib');

function gzipMaxLevel(buffer, options, callback) {
  return zlib['gzip'](
    buffer,
    {
      ...options,
      level: 9
    },
    callback
  );
}

// Getting sample Env variable:
const SAMPLE_VAR = process.env.SAMPLE_VAR || '';

const NODE_ENV = process.env.NODE_ENV || '';

const config = {
  plugins: [
    new webpack.DefinePlugin({
      SAMPLE_VAR: JSON.stringify(SAMPLE_VAR)
    })
  ]
};

if (NODE_ENV.trim() !== 'local') {
  config.optimization = {
    minimize: true,
    minimizer: [
      new TerserPlugin({
        cache: true
      })
    ]
  };

  config.plugins.push(
    new CompressionPlugin({
      algorithm: gzipMaxLevel,
      test: /\.css$|\.html$|\.js$|\.map$/,
      threshold: 2 * 1024
    })
  );
}

module.exports = config;
