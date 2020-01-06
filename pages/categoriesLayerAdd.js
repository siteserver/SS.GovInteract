var $url = '/pages/categoriesLayerAdd';

var data = {
  siteId: utils.getQueryString('siteId'),
  apiUrl: utils.getQueryString('apiUrl'),
  categoryId: utils.getQueryString('categoryId'),
  pageLoad: false,
  pageAlert: null,
  category: null,
  allUserNames: null,
  userNames: []
};

var methods = {
  load: function () {
    var $this = this;

    if (this.categoryId) {
      $api.get($url + '/' + this.categoryId + '?siteId=' + this.siteId).then(function (response) {
        var res = response.data;
        $this.category = res.value;
        $this.allUserNames = res.allUserNames;
        $this.userNames = $this.category.userNames.split(',');
      }).catch(function (error) {
        $this.pageAlert = utils.getPageAlert(error);
      }).then(function () {
        $this.pageLoad = true;
      });
    } else {
      $api.get($url + '?siteId=' + this.siteId).then(function (response) {
        var res = response.data;
        $this.category = res.value;
        $this.allUserNames = res.allUserNames;
        $this.userNames = $this.category.userNames.split(',');
      }).catch(function (error) {
        $this.pageAlert = utils.getPageAlert(error);
      }).then(function () {
        $this.pageLoad = true;
      });
    }
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$validator.validate().then(function (result) {
      if (result) {
        utils.loading(true);

        if ($this.categoryId) {
          $api.put($url + '/' + $this.categoryId + '?siteId=' + $this.siteId, {
            categoryName: $this.category.categoryName,
            userNames: $this.userNames.join(','),
            taxis: $this.category.taxis
          }).then(function (response) {
            parent.location.reload(true);
          }).catch(function (error) {
            $this.pageAlert = utils.getPageAlert(error);
          }).then(function () {
            utils.loading(false);
          });
        } else {
          $api.post($url + '?siteId=' + $this.siteId, {
            categoryName: $this.category.categoryName,
            userNames: $this.userNames.join(','),
            taxis: $this.category.taxis
          }).then(function (response) {
            parent.location.reload(true);
          }).catch(function (error) {
            $this.pageAlert = utils.getPageAlert(error);
          }).then(function () {
            utils.loading(false);
          });
        }
      }
    });
  }
};

new Vue({
  el: '#main',
  data: data,
  methods: methods,
  created: function () {
    this.load();
  }
});