//generate url for mvnc
function generateUrl(url) {
    var current = window.location.pathname;
    var hostName = window.location.host;
    var patt1 = /\/weixin\/[a-z]*\//i;
    var n = current.search(patt1);
    console.log(n);
    if (n >= 0) return "../" + url;
    
    return url;
}

//��ȡurl�еĲ���
function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //����һ������Ŀ����������������ʽ����
    var r = window.location.search.substr(1).match(reg);  //ƥ��Ŀ������
    if (r != null) return unescape(r[2]); return null; //���ز���ֵ
}