var getQueryString = function (name) {
  var result = location.search.match(
    new RegExp('[?&]' + name + '=([^&]+)', 'i')
  );
  if (!result || result.length < 1) {
    return '';
  }
  return decodeURIComponent(result[1]);
};

var getPageAlert = function (error) {
  var message = error.message;
  if (error.response && error.response.data) {
    if (error.response.data.exceptionMessage) {
      message = error.response.data.exceptionMessage;
    } else if (error.response.data.message) {
      message = error.response.data.message;
    }
  }

  return {
    type: "danger",
    html: message
  };
};

var $urlCaptcha = '/captcha';

var $api = axios.create({
  baseURL: (getQueryString('apiUrl') || $apiUrl) + '/SS.GovInteract/',
  withCredentials: true
});

var data = {
  siteId: getQueryString('siteId') || $siteId,
  apiUrl: getQueryString('apiUrl') || $apiUrl,
  pageConfig: null,
  pageLoad: false,
  pageType: 'form',
  pageAlert: null,
  categories: null,
  departments: null,
  settings: null,
  captchaUrl: null,
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

    $api.get('?siteId=' + this.siteId).then(function (response) {
      var res = response.data;

      $this.categories = res.categories;
      $this.departments = res.departments;
      $this.settings = res.settings;
      $this.captchaUrl = $api.defaults.baseURL + $urlCaptcha + '?r=' + new Date().getTime();
      if ($this.settings.isClosed) {
        $this.pageType = 'error';
        $this.errorMessage = '互动交流已暂时关闭！';
      }
    }).catch(function (error) {
      $this.pageAlert = getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  submit: function () {
    var $this = this;

    $this.pageLoad = false;
    $api.post(_.assign({}, this.dataInfo, {
      siteId: this.siteId
    })).then(function (response) {
      var res = response.data;
      $this.dataInfo = res.value;
      $this.pageType = 'success';
    }).catch(function (error) {
      $this.captchaUrl = $api.defaults.baseURL + $urlCaptcha + '?r=' + new Date().getTime();
      $this.pageAlert = getPageAlert(error);
    }).then(function () {
      $this.pageLoad = true;
    });
  },

  reload: function () {
    this.captchaUrl = $api.defaults.baseURL + $urlCaptcha + '?r=' + new Date().getTime();
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