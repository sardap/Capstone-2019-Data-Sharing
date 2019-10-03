const path = require("path");
const webpack = require("webpack");

module.exports = {
  devtool: "inline-source-map",
  devServer: {
    contentBase: "../wwwroot/dist",
    hot: true
  },
  entry: "./index.js",
  module: {
    rules: [
      {
        test: /\.(js|jsx)$/,
        exclude: /node_modules/,
        use: ["babel-loader"]
      },
      { test: /\.css$/, use: "css-loader" }
    ]
  },
  output: {
    path: path.resolve(__dirname, "../wwwroot/dist"),
    filename: "dsp.bundle.js"
  },
  resolve: {
    extensions: ["*", ".js", ".jsx"]
  },
  plugins: [new webpack.HotModuleReplacementPlugin()]
};
