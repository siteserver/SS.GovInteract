var $url = '/pages/contentsLayerRedo';

var data = {
  siteId: parseInt(utils.getQueryString('siteId')),
  contentIds: utils.getQueryString('contentIds'),
  pageLoad: false,
  pageAlert: null,
  contents: null,
  redoComment: null
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
      $this.pageAlert = {
        type: 'danger',
        html: '此操作将要求重新答复以下 <strong>' + $this.contents.length + '</strong> 项信件。'
      };
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  btnSubmitClick: function () {
    var $this = this;

    this.$validator.validate().then(function (result) {
      if (result) {
        utils.loading(true);
        $api.post($url + '?siteId=' + $this.siteId, {
          contentIds: $this.contentIds,
          redoComment: $this.redoComment
        }).then(function (response) {
          var res = response.data;

          parent.location.reload(true);
        }).catch(function (error) {
          $this.pageAlert = utils.getPageAlert(error);
        });
      }
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