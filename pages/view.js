var $url = '/pages/view';

var data = {
  siteId: utils.getQueryString('siteId'),
  apiUrl: utils.getQueryString('apiUrl'),
  dataId: utils.getQueryString('dataId'),
  returnUrl: utils.getQueryString('returnUrl'),
  pageType: null,
  pageLoad: false,
  pageAlert: null,
  dataInfo: null,
  fileInfoList: null,
  logInfoList: null,
  settings: null
};

var methods = {
  load: function () {
    var $this = this;

    $api.get($url, {
      params: {
        siteId: this.siteId,
        dataId: this.dataId
      }
    }).then(function (response) {
      var res = response.data;

      $this.dataInfo = res.value;
      $this.fileInfoList = res.fileInfoList;
      $this.logInfoList = res.logInfoList;
      $this.settings = res.settings;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  btnLayerClick: function (options) {
    this.pageAlert = null;
    var url = "contentsLayer" + options.name + ".html?siteId=" + this.siteId + "&apiUrl=" + encodeURIComponent(this.apiUrl);
    if (options.withContents) {
      url += "&contentIds=" + this.dataId;
    } else if (options.withContent) {
      url += "&contentId=" + this.dataId;
    }
    url += '&returnUrl=' + encodeURIComponent(location.href);

    utils.openLayer({
      title: options.title,
      url: url,
      full: true
    });
  },

  btnReturnClick: function () {
    location.href = this.returnUrl;
  }
};

var $vue = new Vue({
  el: "#main",
  data: data,
  methods: methods,
  created: function () {
    this.load();
  }
});