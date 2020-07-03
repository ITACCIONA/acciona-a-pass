const PROXY_CONFIG_MOCK = [{
    context: ['/api'],
    target: 'https://foo-webapi.bar/',
    changeOrigin: true
  },
  {
    context: ['/identity'],
    target: 'https://foo-idservice.bar/api/',
    changeOrigin: true,
    pathRewrite: {
      "^/identity": ""
    }
  },
];

module.exports = PROXY_CONFIG_MOCK;
