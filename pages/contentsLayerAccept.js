var $url = '/pages/contentsLayerAccept';

var data = {
  siteId: parseInt(utils.getQueryString('siteId')),
  contentIds: utils.getQueryString('contentIds'),
  pageLoad: false,
  pageAlert: null,
  contents: null,
  departmentInfoList: null,
  departmentId: 0
};

var methods = {
  loadConfig: function () {
    var $this = this;

    $api.get($url, {
      params: {
        siteId: $this.siteId,
        contentIds: $this.contentIds
      }
    }).then(function (response) {
      var res = response.data;

      $this.contents = res.value;
      $this.departmentInfoList = res.departmentInfoList;
      $this.pageAlert = {
        type: 'warning',
        html: '此操作将受理以下 <strong>' + $this.contents.length + '</strong> 项信件，受理信件后信息将变为待办理状态，确定吗？'
      };
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  btnSubmitClick: function () {
    var $this = this;

    utils.loading(true);
    $api.post($url + '?siteId=' + this.siteId, {
      contentIds: $this.contentIds,
      departmentId: $this.departmentId
    }).then(function (response) {
      var res = response.data;

      parent.location.reload(true);
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    });
  }
};

new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.loadConfig();
  }
});