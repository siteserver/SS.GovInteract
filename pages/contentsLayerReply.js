var $url = '/pages/contentsLayerReply';
var $uploadUrl = utils.getQueryString('apiUrl') + '/ss.application/pages/contentsLayerReply/actions/upload';

var data = {
  siteId: parseInt(utils.getQueryString('siteId')),
  contentId: utils.getQueryString('contentId'),
  pageLoad: false,
  pageAlert: null,
  dataInfo: null,
  fileInfoList: []
};

var methods = {
  loadConfig: function () {
    var $this = this;

    $api.get($url + '/' + this.contentId + '?siteId=' + this.siteId).then(function (response) {
      var res = response.data;

      $this.dataInfo = res.value;
      $this.fileInfoList = res.fileInfoList;
      $this.pageAlert = {
        type: 'warning',
        html: '办理信件后信息将变为待审核状态。'
      };
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
      setTimeout(function () {
        $this.loadUploader()
      }, 100);
    });
  },

  loadUploader: function () {
    var $this = this;

    var E = Q.event,
      Uploader = Q.Uploader;

    var boxDropArea = document.getElementById("drop-area");

    var uploader = new Uploader({
      url: $uploadUrl + '?siteId=' + $this.siteId,
      target: document.getElementById("drop-area"),
      on: {
        add: function () {
          utils.loading(true);
        },
        complete: function (task) {
          utils.loading(false);
          var res = task.json;
          if (!res || !res.value) {
            return swal({
              title: "上传失败！",
              type: 'warning',
              confirmButtonText: '关 闭'
            });
          }

          $this.fileInfoList.push(res.fileInfo);
        }
      }
    });

    if (!Uploader.support.html5 || !uploader.html5) {
      boxDropArea.innerHTML = "点击批量上传附件";
      return;
    }

    E.add(boxDropArea, "dragleave", E.stop);
    E.add(boxDropArea, "dragenter", E.stop);
    E.add(boxDropArea, "dragover", E.stop);

    E.add(boxDropArea, "drop", function (e) {
      E.stop(e);
      var files = e.dataTransfer.files;
      uploader.addList(files);
    });
  },

  deleteFile: function (fileInfo) {
    this.fileInfoList.splice(this.fileInfoList.indexOf(fileInfo), 1);
  },

  btnSubmitClick: function () {
    var $this = this;
    this.$validator.validate().then(function (result) {
      if (result) {
        utils.loading(true);
        $api.post($url + '/' + $this.contentId + '?siteId=' + $this.siteId, {
          replyContent: $this.dataInfo.replyContent,
          fileInfoList: $this.fileInfoList
        }).then(function (response) {
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