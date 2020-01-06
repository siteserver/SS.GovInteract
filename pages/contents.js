var $url = '/pages/contents';

var data = {
  apiUrl: utils.getQueryString('apiUrl'),
  siteId: utils.getQueryString('siteId'),
  pageType: utils.getQueryString('pageType'),
  pageLoad: false,
  pageAlert: null,
  isSearch: false,
  state: utils.getQueryString('state'),
  departmentId: 0,
  keyword: null,
  page: 1,
  isAllChecked: false,
  pageContents: [],
  count: null,
  pages: null,
  pageOptions: null,
  departmentInfoList: null,
  countDict: null,
  settings: null
};

var methods = {
  getDepartmentText: function (departmentId) {
    if (!departmentId || !this.departmentInfoList) return '';
    var index = _.findIndex(this.departmentInfoList, function (o) {
      return o.id == departmentId;
    });
    if (index == -1) return '';
    return this.departmentInfoList[index].departmentName;
  },

  btnSearchClick: function () {
    this.isSearch = true;
    this.loadPage(1);
  },

  btnStateClick: function (state) {
    this.state = state;
    this.isSearch = true;
    this.loadPage(1);
  },

  getReplyContentId: function () {
    if (this.selectedContentIds.length !== 1) return 0;
    for (var i = 0; i < this.pageContents.length; i++) {
      if (this.pageContents[i].isSelected && (this.pageContents[i].state == 'Accepted' || this.pageContents[i].state == 'Redo')) {
        return this.pageContents[i].id;
      }
    }
    return 0;
  },

  btnLayerClick: function (options, e) {
    e.stopPropagation();

    this.pageAlert = null;
    var url = "contentsLayer" + options.name + ".html?siteId=" + this.siteId + '&state=' + this.state + "&apiUrl=" + encodeURIComponent(this.apiUrl);
    if (options.withContents) {
      if (this.selectedContentIds.length === 0) return;
      url += "&contentIds=" + this.selectedContentIds.join(",")
    } else if (options.withContent) {
      if (options.contentId === 0) return;
      url += "&contentId=" + options.contentId;
    }
    url += '&returnUrl=' + encodeURIComponent(location.href);

    utils.openLayer({
      title: options.title,
      url: url,
      full: options.full,
      width: options.width ? options.width : 700,
      height: options.height ? options.height : 500
    });
  },

  scrollToTop: function () {
    document.documentElement.scrollTop = document.body.scrollTop = 0;
  },

  loadPage: function (page) {
    var $this = this;

    if ($this.pageLoad) {
      utils.loading(true);
    }

    $api.get($url, {
      params: {
        siteId: this.siteId,
        pageType: this.pageType,
        state: this.state,
        page: page,
        isSearch: this.isSearch,
        keyword: this.keyword,
        departmentId: this.departmentId
      }
    }).then(function (response) {
      var res = response.data;

      var pageContents = [];
      for (var i = 0; i < res.value.length; i++) {
        var content = _.assign({}, res.value[i], {
          isSelected: false
        });
        pageContents.push(content);
      }
      $this.pageContents = pageContents;
      $this.count = res.count;
      $this.pages = res.pages;
      $this.page = res.page;
      $this.pageOptions = [];
      for (var i = 1; i <= $this.pages; i++) {
        $this.pageOptions.push(i);
      }
      if ($this.page == 1) {
        $this.departmentInfoList = res.departmentInfoList;
        $this.countDict = res.countDict;
        $this.settings = res.settings;
      }
    }).catch(function (error) {
      this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      if ($this.pageLoad) {
        utils.loading(false);
        $this.scrollToTop();
      } else {
        $this.pageLoad = true;
      }
    });
  },

  btnTitleClick: function (content) {
    location.href = 'view.html?siteId=' + this.siteId + '&dataId=' + content.id + '&apiUrl=' + encodeURIComponent(utils.getQueryString('apiUrl')) + '&returnUrl=' + encodeURIComponent(location.href);
  },

  btnExportClick: function () {
    utils.loading(true);
    $api.post({
      siteId: config.siteId,
      channelId: config.channelId,
      contentId: config.contentId,
      formId: config.formId
    }, function (err, res) {
      utils.loading(false);
      if (err || !res || !res.value) return;

      alert({
        toast: true,
        type: 'success',
        title: "数据导出成功",
        showConfirmButton: false,
        timer: 1000
      }).then(function () {
        window.open(res.value);
      });
    }, 'actions/export');
  },

  loadFirstPage: function () {
    if (this.page === 1) return;
    this.loadPage(1);
  },

  loadPrevPage: function () {
    if (this.page - 1 <= 0) return;
    this.loadPage(this.page - 1);
  },

  loadNextPage: function () {
    if (this.page + 1 > this.pages) return;
    this.loadPage(this.page + 1);
  },

  loadLastPage: function () {
    if (this.page + 1 > this.pages) return;
    this.loadPage(this.pages);
  },

  onPageSelect(option) {
    this.loadPage(option);
  },

  toggleChecked: function (content) {
    content.isSelected = !content.isSelected;
    if (!content.isSelected) {
      this.isAllChecked = false;
    }
  },

  selectAll: function () {
    this.isAllChecked = !this.isAllChecked;
    for (var i = 0; i < this.pageContents.length; i++) {
      this.pageContents[i].isSelected = this.isAllChecked;
    }
  }
};

Vue.component("multiselect", window.VueMultiselect.default);

var $vue = new Vue({
  el: '#main',
  data: data,
  methods: methods,
  computed: {
    selectedContentIds: function () {
      var retval = [];
      if (this.pageContents) {
        for (var i = 0; i < this.pageContents.length; i++) {
          if (this.pageContents[i].isSelected) {
            retval.push(this.pageContents[i].id);
          }
        }
      }
      return retval;
    },
    singleContentId: function () {
      if (this.selectedContentIds.length == 1) {
        return this.selectedContentIds[0];
      }
      return 0;
    }
  },
  created: function () {
    this.loadPage(1);
  }
});