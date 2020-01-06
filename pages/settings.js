var $url = 'pages/settings';

var data = {
  siteId: utils.getQueryString('siteId'),
  apiUrl: utils.getQueryString('apiUrl'),
  pageLoad: false,
  pageAlert: null,
  pageType: 'list',
  settings: null
};

var methods = {
  load: function () {
    var $this = this;

    $api.get($url + '?siteId=' + this.siteId).then(function (response) {
      var res = response.data;

      $this.settings = res.value;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  submit: function () {
    var $this = this;

    var payload = {
      type: this.pageType
    };
    if (this.pageType === 'isClosed') {
      payload.isClosed = $this.settings.isClosed;
    } else if (this.pageType === 'daysWarning') {
      payload.daysWarning = $this.settings.daysWarning;
    } else if (this.pageType === 'daysDeadline') {
      payload.daysDeadline = $this.settings.daysDeadline;
    } else if (this.pageType === 'isDeleteAllowed') {
      payload.isDeleteAllowed = $this.settings.isDeleteAllowed;
    } else if (this.pageType === 'isSelectCategory') {
      payload.isSelectCategory = $this.settings.isSelectCategory;
    } else if (this.pageType === 'isSelectDepartment') {
      payload.isSelectDepartment = $this.settings.isSelectDepartment;
    }

    utils.loading(true);
    $api.post($url + '?siteId=' + this.siteId, payload).then(function (response) {
      swal({
        toast: true,
        type: 'success',
        title: "设置保存成功",
        showConfirmButton: false,
        timer: 2000
      }).then(function () {
        $this.pageType = 'list';
      });
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      utils.loading(false);
    });
  },

  btnSubmitClick: function () {
    var $this = this;
    this.pageAlert = null;

    this.$validator.validate().then(function (result) {
      if (result) {
        $this.submit();
      }
    });
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