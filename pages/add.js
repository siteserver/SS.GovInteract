var $url = '/pages/add';

var data = {
  apiUrl: utils.getQueryString('apiUrl'),
  siteId: utils.getQueryString('siteId'),
  returnUrl: utils.getQueryString('returnUrl'),
  pageConfig: null,
  pageLoad: false,
  pageAlert: null,
  categories: null,
  departments: null,
  settings: null,
  dataInfo: {
    name: '',
    gender: '',
    phone: '',
    email: '',
    address: '',
    zip: '',
    title: '',
    content: '',
    categoryId: '',
    departmentId: '',
    authCode: ''
  },
  rules: {
    name: [
      { required: true, message: '请输入姓名', trigger: 'blur' },
      { min: 2, max: 5, message: '长度在 3 到 5 个字符', trigger: 'blur' }
    ],
    gender: [
      { required: true, message: '请选择性别', trigger: 'blur' }
    ],
    email: [
      { required: true, message: '请输入电子邮箱', trigger: 'blur' },
      { type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur' }
    ],
    title: [
      { required: true, message: '请输入信件标题', trigger: 'blur' }
    ],
    content: [
      { required: true, message: '请输入信件正文', trigger: 'blur' }
    ],
    categoryId: [
      { required: true, message: '请选择信件分类', trigger: 'blur' }
    ],
    departmentId: [
      { required: true, message: '请选择提交对象', trigger: 'blur' }
    ],
    authCode: [
      { required: true, message: '请输入验证码', trigger: 'blur' }
    ],
  }
};

var methods = {
  load: function () {
    var $this = this;

    $api.get($url, {
      params: {
        siteId: this.siteId
      }
    }).then(function (response) {
      var res = response.data;

      $this.categories = res.categories;
      $this.departments = res.departments;
      $this.settings = res.settings;
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  apiSubmit: function () {
    var $this = this;

    utils.loading(true);
    $api.post($url, _.assign({}, this.dataInfo, {
      siteId: this.siteId
    })).then(function (response) {
      $this.$message.success('信件提交成功');
      setTimeout(function () {
        location.href = 'contents.html?siteId=' + $this.siteId + '&state=New&apiUrl=' + encodeURIComponent($this.apiUrl);
      }, 2000);
    }).catch(function (error) {
      $this.pageAlert = utils.getPageAlert(error);
    }).then(function () {
      utils.loading(false);
    });
  },

  btnSubmitClick: function(formName) {
    var $this = this;
    this.$refs[formName].validate((valid) => {
      if (valid) {
        $this.apiSubmit();
      }
    });
  },

  btnResetClick: function(formName) {
    this.$refs[formName].resetFields();
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